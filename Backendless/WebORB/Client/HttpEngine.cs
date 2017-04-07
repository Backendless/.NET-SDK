using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
#if !UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8
using System.Security;
using System.Windows.Browser;
#endif
using System.Threading;
using Weborb.Message;
using Weborb.Protocols.Amf;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.V3Types;

namespace Weborb.Client
{
  public class HttpEngine : Engine
  {
    private Timer _subscriptionTimer;
    private int _pollingInterval = 1000;
    private HttpWebRequest _request;

    public HttpEngine(string url, IdInfo idInfo) : base(url,idInfo)
    {
    }

    internal override void Invoke<T>(string className, 
      string methodName, 
      object[] args, 
      IDictionary requestHeaders, 
      IDictionary messageHeaders, 
      IDictionary httpHeaders, 
      Responder<T> responder, 
      AsyncStreamSetInfo<T> asyncStreamSetInfo)
    {
      SendRequest(CreateMessageForInvocation(className, methodName, args, messageHeaders), requestHeaders, httpHeaders, responder, asyncStreamSetInfo );
    }
    
    public override void SendRequest<T>(V3Message v3Msg, 
      IDictionary requestHeaders, 
      IDictionary httpHeaders, 
      Responder<T> responder,
      AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      ThreadPool.QueueUserWorkItem( state => SendHttpRequest( v3Msg, requestHeaders, httpHeaders, responder, asyncStreamSetInfo ) );
    }

    private void SendHttpRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      HttpWebRequest webReq;
      try
      {
        webReq = GetWebRequest();
      }
      catch (Exception e)
      {
        HandleException( asyncStreamSetInfo, e );
        return;
      }

      if(httpHeaders != null)
        foreach ( DictionaryEntry header in httpHeaders )
        {
          webReq.Headers[header.Key.ToString()] = header.Value.ToString();
        }

      webReq.CookieContainer = Cookies;

      byte[] requestBytes = CreateRequest(v3Msg, requestHeaders);

      asyncStreamSetInfo.requestBytes = requestBytes;
      asyncStreamSetInfo.request = webReq;
      asyncStreamSetInfo.responder = responder;

      // Set the ContentType property. 
      webReq.ContentType = "application/x-amf";
      // Set the Method property to 'POST' to post data to the URI.
      webReq.Method = "POST";
      // Start the asynchronous operation.    

      try
      {
        webReq.BeginGetRequestStream(new AsyncCallback(SetStreamDataCallback<T>), asyncStreamSetInfo);
      }
      catch (Exception e1)
      {
        HandleException(asyncStreamSetInfo, e1);
      }
    }

    private void HandleException<T>(AsyncStreamSetInfo<T> asyncStreamSetInfo, Exception e)
    {
      String error = e.Message;

#if (!UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                if (e is SecurityException)
                {
                    if (WeborbClient.Uri.Scheme.ToLower().StartsWith("file"))
                      error = SECURITY_FAULT_MESSAGE;
                }

                if (UiControl != null)
                    UiControl.Dispatcher.BeginInvoke(delegate()
                    {
                        HtmlPage.Window.Alert(error);
                    });
#endif
      if ( asyncStreamSetInfo != null )
      {
        Fault fault = new Fault( error, e.StackTrace, INTERNAL_CLIENT_EXCEPTION_FAULT_CODE );
        if ( asyncStreamSetInfo.responder != null )
        {
          if ( e is WebException && ( (WebException)e ).Status == WebExceptionStatus.RequestCanceled )
            fault = new Fault( TIMEOUT_FAULT_MESSAGE, TIMEOUT_FAULT_MESSAGE );
          asyncStreamSetInfo.responder.ErrorHandler( fault );
        }
      }
    }

    private HttpWebRequest GetWebRequest()
    {
//      if ( _request != null )
        //        return _request;
#if (FULL_BUILD || WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8)
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


        if (Uri.IsWellFormedUriString(GatewayUrl, UriKind.Absolute))
            {
              _request = (HttpWebRequest)WebRequest.Create(new Uri(GatewayUrl));
            }
            else
            {
                Uri finalURI;
                Uri.TryCreate(WeborbClient.Uri, GatewayUrl, out finalURI);
                _request = (HttpWebRequest)WebRequest.Create(finalURI);
            }
#endif

      return _request;
    }

    protected byte[] CreateRequest( V3Message v3Msg, IDictionary headers )
    {
      Header[] headersArray = null;

      if (headers != null)
      {
        headersArray = new Header[headers.Count];
        int i = 0;

        foreach (String headerName in headers.Keys)
          headersArray[i] = new Header(headerName, false, -1, new ConcreteObject(headers[headerName]));
      }
      else
      {
        headersArray = new Header[0];
      }

      Body[] bodiesArray = new Body[1];
      bodiesArray[0] = new Body("null", "null", -1, null);

      Request request = new Request(3, headersArray, bodiesArray);
      request.setResponseBodyData(new object[] {v3Msg});

      return AMFSerializer.SerializeToBytes(request);
    }

    private void SetStreamDataCallback<T>(IAsyncResult asynchronousResult)
    {
      AsyncStreamSetInfo<T> asyncStreamSetInfo = (AsyncStreamSetInfo<T>)asynchronousResult.AsyncState;
      try
      {
        HttpWebRequest request = asyncStreamSetInfo.request;
        // End the operation.
        Stream postStream = request.EndGetRequestStream(asynchronousResult);
        postStream.Write(asyncStreamSetInfo.requestBytes, 0, asyncStreamSetInfo.requestBytes.Length);
        postStream.Flush();
        postStream.Close();
        asyncStreamSetInfo.messageSentTime = DateTime.Now.Ticks;
        request.BeginGetResponse(new AsyncCallback(ProcessAMFResponse<T>), asyncStreamSetInfo);
        if ( Timeout > 0 )
        {
          Timer timer = new Timer( state =>
          {
            if ( !request.HaveResponse )
              request.Abort();
          } );
          timer.Change( Timeout, System.Threading.Timeout.Infinite );
        }
        // allDone.Set();
      }
      catch (Exception exception)
      {
        String error = exception.Message;
        if ( exception is WebException && ( (WebException)exception ).Status == WebExceptionStatus.RequestCanceled )
          error = TIMEOUT_FAULT_MESSAGE;

#if (!UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                if( exception is SecurityException )
                {
                  if ( WeborbClient.Uri.Scheme.ToLower().StartsWith( "file" ) )
                  {
                    error = SECURITY_FAULT_MESSAGE;
                  }
                }

                if( UiControl != null )
                    UiControl.Dispatcher.BeginInvoke( delegate()
                    {
                        HtmlPage.Window.Alert( error );
                    } );
#endif
        if ( asyncStreamSetInfo != null )
        {
          Fault fault = new Fault( error, exception.StackTrace, INTERNAL_CLIENT_EXCEPTION_FAULT_CODE );
          if ( asyncStreamSetInfo.responder != null )
          {

            asyncStreamSetInfo.responder.ErrorHandler( fault );
          }
        }
      }
    }
    
    private void ProcessAMFResponse<T>(IAsyncResult asyncResult)
    {
      try
      {
        AsyncStreamSetInfo<T> asyncStreamSetInfo = (AsyncStreamSetInfo<T>)asyncResult.AsyncState;

        if( asyncStreamSetInfo.responseThreadConfigurator != null )
          asyncStreamSetInfo.responseThreadConfigurator();

        HttpWebRequest request = asyncStreamSetInfo.request;
        HttpWebResponse response = (HttpWebResponse)request.EndGetResponse( asyncResult );
        
        if(Cookies != null)
        foreach(Cookie cookie in response.Cookies)
        {
          Cookies.Add(new Uri(GatewayUrl), cookie); 
        }

        Stream streamResponse = response.GetResponseStream();
        long curTime = DateTime.Now.Ticks;
        long roundTrip = (curTime - asyncStreamSetInfo.messageSentTime) / TimeSpan.TicksPerMillisecond;
        RequestParser parser = new RequestParser();
        Request responseObject = parser.readMessage(streamResponse);
        object[] responseData = (object[])responseObject.getRequestBodyData();
        V3Message v3 = (V3Message)((IAdaptingType)responseData[0]).defaultAdapt();

        if( v3.isError )
        {
          ErrMessage errorMessage = (ErrMessage) v3;
          Fault fault = new Fault( errorMessage.faultString, errorMessage.faultDetail, errorMessage.faultCode );

          if( asyncStreamSetInfo.responder != null )
            asyncStreamSetInfo.responder.ErrorHandler( fault );

          return;
        }

        IAdaptingType body = (IAdaptingType)( (AnonymousObject) ( (NamedObject) responseData[ 0 ] ).TypedObject ).Properties[ "body" ];
        T result = (T)body.adapt( typeof( T ) );

        if( asyncStreamSetInfo.responder != null )
          asyncStreamSetInfo.responder.ResponseHandler( result );

      //  ProcessV3Message( v3, asyncStreamSetInfo.responder );
      }
      catch (Exception e)
      {
        AsyncStreamSetInfo<T> asyncStreamSetInfo = (AsyncStreamSetInfo<T>)asyncResult.AsyncState;
        ProccessException( e, asyncStreamSetInfo.responder );
      }
    }

    internal override void OnSubscribed(string subTopic, string selector, string clientId)
    {
      base.OnSubscribed(subTopic, selector, clientId);
      if ( _subscriptionTimer == null )
      {
        _subscriptionTimer = new Timer(o => RecieveMessages(subTopic, selector, clientId));
        _subscriptionTimer.Change(0, _pollingInterval);
      }
    }

    internal override void OnUnsubscribed(string clientId)
    {
      base.OnUnsubscribed(clientId);
      if ( Subscriptions.Count < 1 )
      {
        _subscriptionTimer.Dispose();
        _subscriptionTimer = null;
      }
    }

    protected void RecieveMessages(string SubTopic, string Selector, string clientId)
    {
      try
      {
        CommandMessage message = new CommandMessage();
        message.operation = CommandMessage.POLL_OPERATION;
        Subscription.InitCommandMessage(message, SubTopic, Selector, IdInfo, clientId);
        SendRequest(message, null, null, new Responder<V3Message>(
                                           result =>
                                             {
                                               if (result != null)
                                               {
                                                 if(!(result is AsyncMessage) && result.body.body is ArrayType)
                                                 {
                                                   object[] arr = (object[])( (ArrayType)result.body.body ).getArray();
                                                   foreach (object o in arr)
                                                   {
                                                     IAdaptingType adaptingType = (IAdaptingType) o;
                                                     base.ReceivedMessage((AsyncMessage)adaptingType.adapt(typeof(AsyncMessage)));
                                                   }
                                                 }
                                                 else
                                                 {
                                                   base.ReceivedMessage( (AsyncMessage)result );
                                                 }
                                               }
                                             },
                                           fault => /*(ISubscribeResponder)GetResponder(clientId).ErrorHandler(fault)*/ { }), null);
      }
      catch (Exception)
      {
        try
        {
          //GetResponder<T>().ErrorHandler(new Fault(Subscription.ReceiveMessagesError, e.Message));
        }
        catch (Exception)
        {
        }
      }
    }
  }
}
