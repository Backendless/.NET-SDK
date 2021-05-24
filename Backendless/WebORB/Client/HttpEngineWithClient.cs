using System;
using System.Collections;
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
    public HttpEngineWithClient( String url, IdInfo idInfo ) : base( url, idInfo )
    {
    }

    public override void SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders,
                                         IDictionary httpHeaders, Responder<T> responder,
                                         AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      Task task = SendHttpRequest( v3Msg, requestHeaders, httpHeaders, responder, asyncStreamSetInfo );
      task.Wait();
    }

    internal override void Invoke<T>( string className, string methodName, object[] args, IDictionary requestHeaders,
                                  IDictionary messageHeaders, IDictionary httpHeaders, Responder<T> responder,
                                  AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      var messageForInvocation = CreateMessageForInvocation( className, methodName, args, messageHeaders );
      SendRequest( messageForInvocation, requestHeaders, httpHeaders, responder, asyncStreamSetInfo );
    }

    public override Task<T> SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders,
                                                                  ResponseThreadConfigurator threadConfigurator ) =>
      SendHttpRequest<T>( v3Msg, requestHeaders, httpHeaders, threadConfigurator );

    internal override Task<T> Invoke<T>( string className, string methodName, object[] args, IDictionary requestHeaders,
               IDictionary messageHeaders, IDictionary httpHeaders, ResponseThreadConfigurator threadConfigurator ) =>
      SendRequest<T>( CreateMessageForInvocation( className, methodName, args, messageHeaders ),
                                        requestHeaders, httpHeaders, threadConfigurator );

    private async Task SendHttpRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      byte[] requestBytes = CreateRequest( v3Msg, requestHeaders );
      HttpRequestMessage requestMessage = new HttpRequestMessage
      {
        RequestUri = new Uri( GatewayUrl ),
        Method = HttpMethod.Post,
        Content = new ByteArrayContent( requestBytes )
      };

      requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/x-amf" );
      HttpResponseMessage responseMessage;
      try
      {
        Task<HttpResponseMessage> task = httpClient.SendAsync( requestMessage );
        task.Wait();
        responseMessage = task.Result;
      }
      catch( Exception ex )
      {
        throw new BackendlessAPI.Exception.BackendlessException( ex.Message + "Check your internet connection" );
      }

      asyncStreamSetInfo.responder = responder;

      var streamResponse = await responseMessage.Content.ReadAsStreamAsync();
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
      byte[] requestBytes = CreateRequest( v3Msg, requestHeaders );
      HttpRequestMessage requestMessage = new HttpRequestMessage
      {
        RequestUri = new Uri( GatewayUrl ),
        Method = HttpMethod.Post,
        Content = new ByteArrayContent( requestBytes )
      };

      requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue( "application/x-amf" );

      System.IO.Stream postStream = await requestMessage.Content.ReadAsStreamAsync();

      try
      {
        postStream.Write( requestBytes, 0, requestBytes.Length );
        postStream.Flush();
        postStream.Close();

        HttpResponseMessage responseMessage = await httpClient.SendAsync( requestMessage );
        threadConfigurator?.Invoke();

        var streamResponse = await responseMessage.Content.ReadAsStreamAsync();
        var parser = new RequestParser();
        var responseObject = parser.readMessage( streamResponse );
        var responseData = (Object[]) responseObject.getRequestBodyData();
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
  }
}