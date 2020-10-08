using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Weborb.V3Types;
using System.Net.Http;
using Weborb.Message;
using Weborb.Util;
using Weborb.Reader;
using System.Net;
using System.Net.Http.Headers;
using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Exceptions;

namespace Weborb.Client
{
  public class HttpEngineWithClient : Engine
  {
    HttpClient httpClient = new HttpClient();
    HttpWebRequest _request;
    public HttpEngineWithClient( String url, IdInfo idInfo ) : base( url, idInfo )
    {
    }

    public override void SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders,
                                         IDictionary httpHeaders, Responder<T> responder,
                                         AsyncStreamSetInfo<T> asyncStreamSetInfo ) =>
      ThreadPool.QueueUserWorkItem( state => SendHttpRequest( v3Msg, requestHeaders, httpHeaders, responder, asyncStreamSetInfo ) );

    internal override void Invoke<T>( string className, string methodName, object[] args, IDictionary requestHeaders,
                                  IDictionary messageHeaders, IDictionary httpHeaders, Responder<T> responder,
                                  AsyncStreamSetInfo<T> asyncStreamSetInfo ) =>
      SendRequest( CreateMessageForInvocation( className, methodName, args, messageHeaders ),
                                requestHeaders, httpHeaders, responder, asyncStreamSetInfo );

    public override Task<T> SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders,
                                                                  ResponseThreadConfigurator threadConfigurator ) =>
      SendHttpRequest<T>( v3Msg, requestHeaders, httpHeaders, threadConfigurator );

    internal override Task<T> Invoke<T>( string className, string methodName, object[] args, IDictionary requestHeaders,
               IDictionary messageHeaders, IDictionary httpHeaders, ResponseThreadConfigurator threadConfigurator ) =>
      SendRequest<T>( CreateMessageForInvocation( className, methodName, args, messageHeaders ),
                                        requestHeaders, httpHeaders, threadConfigurator );

    private async void SendHttpRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      byte[] requestBytes = CreateRequest( v3Msg, requestHeaders );
      HttpRequestMessage requestMessage = new HttpRequestMessage
      {
        RequestUri = new Uri( GatewayUrl ),
        Method = HttpMethod.Post,
        Content = new ByteArrayContent( requestBytes )
      };

      requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/x-amf" );
      HttpResponseMessage responseMessage = await httpClient.SendAsync( requestMessage );

      asyncStreamSetInfo.responder = responder;

      var streamResponse = await responseMessage.Content.ReadAsStreamAsync();
      var curTime = DateTime.Now.Ticks;
      var roundTrip = ( curTime - asyncStreamSetInfo.messageSentTime ) / TimeSpan.TicksPerMillisecond;
      var parser = new RequestParser();
      var responseObject = parser.readMessage( streamResponse );
      var responseData = (object[]) responseObject.getRequestBodyData();
      var v3 = (V3Message) ( (IAdaptingType) responseData[ 0 ] ).defaultAdapt();

      if( v3.isError )
      {
        var errorMessage = (ErrMessage) v3;
        var fault = new Fault( errorMessage.faultString, errorMessage.faultDetail, errorMessage.faultCode );
        asyncStreamSetInfo.responder?.ErrorHandler( fault );
        return;
      }

      var body = (IAdaptingType) ( (AnonymousObject) ( (NamedObject) responseData[ 0 ] ).TypedObject ).Properties[ "body" ];
      var result = (T) body.adapt( typeof( T ) );

      asyncStreamSetInfo.responder?.ResponseHandler( result );
    }

    private async Task<T> SendHttpRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders,
                                          ResponseThreadConfigurator threadConfigurator )
    {
      HttpWebRequest webReq;
      try
      {
        webReq = GetWebRequest();
      }
      catch( Exception e )
      {
        throw new WebORBException( GetFault( e ) );
      }

      if( httpHeaders != null )
        foreach( DictionaryEntry header in httpHeaders )
          webReq.Headers[ header.Key.ToString() ] = header.Value.ToString();

      webReq.CookieContainer = Cookies;

      byte[] requestBytes = CreateRequest( v3Msg, requestHeaders );

      // Set the ContentType property. 
      webReq.ContentType = "application/x-amf";
      // Set the Method property to 'POST' to post data to the URI.
      webReq.Method = "POST";
      // Start the asynchronous operation.    

      System.IO.Stream postStream = await webReq.GetRequestStreamAsync();
      try
      {
        // End the operation.
        postStream.Write( requestBytes, 0, requestBytes.Length );
        postStream.Flush();
        postStream.Close();

        HttpWebResponse response = (HttpWebResponse) await webReq.GetResponseAsync();
        threadConfigurator?.Invoke();

        if( Cookies != null )
          foreach( Cookie cookie in response.Cookies )
            Cookies.Add( new Uri( GatewayUrl ), cookie );

        var streamResponse = response.GetResponseStream();
        var parser = new RequestParser();
        var responseObject = parser.readMessage( streamResponse );
        var responseData = (object[]) responseObject.getRequestBodyData();
        var v3 = (V3Message) ( (IAdaptingType) responseData[ 0 ] ).defaultAdapt();

        if( v3.isError )
        {
          var errorMessage = (ErrMessage) v3;
          var fault = new Fault( errorMessage.faultString, errorMessage.faultDetail, errorMessage.faultCode );
          throw new WebORBException( fault );
        }

        var body =
          (IAdaptingType) ( (AnonymousObject) ( (NamedObject) responseData[ 0 ] ).TypedObject ).Properties[ "body" ];

        var result = (T) body.adapt( typeof( T ) );
        return result;
      }
      catch( Exception exception )
      {
        if( exception is WebORBException )
          throw exception;

        throw new WebORBException( GetFault( exception ) );
      }
    }

    protected byte[] CreateRequest( V3Message v3Msg, IDictionary headers )
    {
      Header[] headersArray = null;

      if( headers != null )
      {
        headersArray = new Header[ headers.Count ];
        int i = 0;

        foreach( String headerName in headers.Keys )
          headersArray[ i ] = new Header( headerName, false, -1, new ConcreteObject( headers[ headerName ] ) );
      }
      else
      {
        headersArray = new Header[ 0 ];
      }

      Body[] bodiesArray = new Body[ 1 ];
      bodiesArray[ 0 ] = new Body( "null", "null", -1, null );

      Request request = new Request( 3, headersArray, bodiesArray );
      request.setResponseBodyData( new object[] { v3Msg } );

      return AMFSerializer.SerializeToBytes( request );
    }

    private Fault GetFault( Exception e )
    {
      Fault fault;

      if( e is WebException exception && exception.Status == WebExceptionStatus.RequestCanceled )
        fault = new Fault( TIMEOUT_FAULT_MESSAGE, TIMEOUT_FAULT_MESSAGE );
      else
        fault = new Fault( e.Message, e.StackTrace, INTERNAL_CLIENT_EXCEPTION_FAULT_CODE );

      return fault;
    }

    private HttpWebRequest GetWebRequest()
    {
      //      if ( _request != null )
      //        return _request;
#if( FULL_BUILD || WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
      try
      {
        Uri uri = new Uri( GatewayUrl );
        _request = (HttpWebRequest) WebRequest.Create( uri );
      }
      catch( Exception e )
      {
        System.Console.WriteLine( e );
      }
#else
      if( Uri.IsWellFormedUriString( GatewayUrl, UriKind.Absolute ) )
      {
        _request = (HttpWebRequest) WebRequest.Create( new Uri( GatewayUrl ) );
      }
      else
      {
        Uri finalURI;
        Uri.TryCreate( WeborbClient.Uri, GatewayUrl, out finalURI );
        _request = (HttpWebRequest) WebRequest.Create( finalURI );
      }
#endif
      return _request;
    }
  }
}
