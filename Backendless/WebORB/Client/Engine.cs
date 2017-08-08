using System;
#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB)
using System.Windows.Controls;
#endif
#if !FULL_BUILD && !UNIVERSALW8 && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8
using System.Security;
using System.Windows.Browser;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Net;
using Weborb.Message;
using Weborb.Protocols.Amf;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util.Logging;
using Weborb.V3Types;

namespace Weborb.Client
{
  public abstract class Engine
  {
    protected const string TIMEOUT_FAULT_MESSAGE = "Request timeout";
    protected const string SECURITY_FAULT_MESSAGE =
      "Unable to connect to the server. Make sure clientaccesspolicy.xml are deployed in the root of the web server the client will communicate with.";

    private CookieContainer _cookies;
    public CookieContainer Cookies
    {
      get { return _cookies; }
      set { _cookies = value; }
    }
    
    private int _timeout = 0;
    public int Timeout {
       get { return _timeout; }
       set { _timeout = value; }
    }

    protected Dictionary<string, Subscription> Subscriptions = new Dictionary<string, Subscription>();
    public Subscription GetSubscription(string clientId)
    {
      if ( Subscriptions.ContainsKey( clientId ) )
        return Subscriptions[clientId];
      return null;
    }
    public void SetSubscription(Subscription subscription )
    {
      if ( Subscriptions.ContainsKey( subscription.ClientId ) )
        Subscriptions[subscription.ClientId] = subscription;
      else
        Subscriptions.Add( subscription.ClientId, subscription );
    }
    public void RemoveSubscription( string clientId )
    {
      if ( Subscriptions.ContainsKey( clientId ) )
        Subscriptions.Remove( clientId );
    }
    
    private Dictionary<string, object> _responder = new Dictionary<string, object>();
    public Responder<T> GetResponder<T>(string clientId)
    {
      if(_responder.ContainsKey(clientId))
        return (Responder<T>)_responder[clientId];
      return null;
    }
    

    private object GetResponder(string clientId)
    {
      if ( _responder.ContainsKey( clientId ) )
        return _responder[clientId];
      return null;
    }

    public void SetResponder<T>(string clientId, Responder<T> responder)
    {
      if ( _responder.ContainsKey( clientId ) )
        _responder[clientId] = responder;
      else
        _responder.Add(clientId, responder);
    }

    public void RemoveResponder(string clientId)
    {
      if ( _responder.ContainsKey( clientId ) )
        _responder.Remove(clientId);
    }
    
    public static Engine Create(string gatewayUrl, IdInfo idInfo)
    {
      if (gatewayUrl.StartsWith("http://") || gatewayUrl.StartsWith("https://"))
        return new HttpEngine(gatewayUrl, idInfo);
#if !UNIVERSALW8 && !PURE_CLIENT_LIB  && !WINDOWS_PHONE8
      if (gatewayUrl.StartsWith("rtmpt://"))
        return new RtmptEngine(gatewayUrl, idInfo);
#endif
#if (!UNIVERSALW8 && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
      if (gatewayUrl.StartsWith("rtmp://"))
        return new RtmpEngine(gatewayUrl, idInfo);
#endif

      throw new ArgumentOutOfRangeException("gatewayUrl", "Unsupported URI scheme in the gateway URL.");
    }

    public String GatewayUrl;
    public IdInfo IdInfo;
    internal const string INTERNAL_CLIENT_EXCEPTION_FAULT_CODE = "Internal client exception";

    internal Engine(string gateway, IdInfo idInfo)
    {
      GatewayUrl = gateway;
      IdInfo = idInfo.MemberwiseClone();
    }

#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB)
    public static Engine Create(string gatewayUrl, IdInfo idInfo, UserControl uiControl)
    {
      Engine engine = Create(gatewayUrl, idInfo);
      engine.UiControl = uiControl;
      return engine;
    }

    internal UserControl UiControl;
#endif

    internal abstract void Invoke<T>(string className, string methodName, object[] args, IDictionary requestHeaders, IDictionary messageHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo);
    public abstract void SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo );
    
    internal void SendRequest<T>( V3Message v3Msg, Responder<T> responder )
    {
      SendRequest( v3Msg, null, null, responder, null );
    }
    
    internal bool IsRTMP()
    {
#if UNIVERSALW8 || PURE_CLIENT_LIB || WINDOWS_PHONE8
     return false;
#else
        return this is BaseRtmpEngine;
#endif
    }

    internal virtual void OnSubscribed(string subTopic, string selector, string clientId)
    {
      GetSubscription(clientId).InvokeSubscribed();
    }
    
    internal virtual void OnUnsubscribed(string clientId)
    {
      RemoveSubscription(clientId);
      RemoveResponder(clientId);
    }

    protected void ReceivedMessage<T>(AckMessage message)
    {
      object responder = GetResponder(message.clientId.ToString());
      if (responder == null)
        return;
      object[] arr = (object[])((ArrayType)message.body.body).getArray();
      foreach (var o in arr)
      {
        IAdaptingType adaptingType = (IAdaptingType)o;
        GetResponder<T>( message.clientId.ToString() ).ResponseHandler( (T)adaptingType.adapt( typeof( T ) ) );
      }
    }

    protected void ReceivedMessage( AsyncMessage message )
    {
      object responder = GetResponder(message.clientId.ToString());
      if (responder != null && responder is ISubscribeResponder )
        ( (ISubscribeResponder)responder ).ResponseHandler( message );
    }

    internal V3Message CreateMessageForInvocation( String className, String methodName, Object[] args, IDictionary headers )
    {
      ReqMessage bodyMessage = new ReqMessage();
      bodyMessage.body = new BodyHolder();
      bodyMessage.body.body = args == null ? new object[0] : args;
      bodyMessage.destination = IdInfo.Destination;
      bodyMessage.headers = headers;

      if ( className != null )
        bodyMessage.source = className;

      bodyMessage.operation = methodName;

      return bodyMessage;
    }

    protected void ProcessV3Message<T>( V3Types.V3Message v3, Responder<T> responder )
    {
      try
      {
        if ( IdInfo != null && IdInfo.DsId == null && v3.headers != null )
        {
          try
          {
            IdInfo.DsId = (String)v3.headers["DSId"];
          }
          catch ( KeyNotFoundException )
          {
          }
        }

//        if ( IdInfo != null && IdInfo.ClientId == null )
//          IdInfo.ClientId = (String)v3.clientId;

        List<T> messagesFirstPhase = new List<T>();

        foreach ( Object returnValueElement in (Object[])v3.body.body )
          ProcessElement( returnValueElement, messagesFirstPhase );

        foreach ( T adaptedObject in messagesFirstPhase )
        {
#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB)
            if (UiControl != null && responder != null)
                      UiControl.Dispatcher.BeginInvoke(delegate()
                      {
                          responder.ResponseHandler(adaptedObject);
                      });
                  else
#endif
          if ( responder != null )
            responder.ResponseHandler( adaptedObject );
        }
      }
      catch ( Exception e )
      {
        ProccessException(e, responder);
      }
    }

    protected void ProccessException<T>(Exception e, Responder<T> responder)
    {
      if ( responder != null )
        {
          Fault fault = new Fault( e.Message, e.StackTrace, INTERNAL_CLIENT_EXCEPTION_FAULT_CODE );
#if (!UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
          if ( e is SecurityException )
            fault = new Fault(SECURITY_FAULT_MESSAGE, e.Message);
#endif
          if ( e is WebException && ( (WebException)e ).Status == WebExceptionStatus.RequestCanceled )
            fault = new Fault( TIMEOUT_FAULT_MESSAGE, TIMEOUT_FAULT_MESSAGE, "timeout" );

          responder.ErrorHandler( fault );
        }
    }

    private void ProcessElement<T>( Object returnValue, List<T> collector )
    {
     // try
      {
        if( returnValue.GetType().IsArray )
        {
          Object[] returnValueArray = (Object[]) returnValue;
          Object[] adaptedValues = new object[returnValueArray.Length];

          if( returnValueArray.Length == 0 && !typeof( T ).IsArray && !IsIListGeneric( typeof( T ) ) )
            throw new InvalidCastException(returnValue.GetType().Name + " cannot be casted to " + typeof(T).Name);
            //collector.Add( (T) returnValue );
          else
          {
            for( int i = 0; i < returnValueArray.Length; i++ )
            {
              Object adaptedObject = ((IAdaptingType) returnValueArray[i]).defaultAdapt();
              adaptedValues[i] = adaptedObject;

              if( adaptedObject is AsyncMessage )
              {
                ((AsyncMessage) adaptedObject)._body.body = ((Object[]) ((AsyncMessage) adaptedObject)._body.body)[0];
                collector.Add( (T) adaptedObject );
              }
              else if( !typeof( T ).IsArray && !IsIListGeneric( typeof( T ) ) )
              {
                collector.Add( (T) adaptedObject );
              }
            }

            if( typeof( T ).IsArray )
            {
              Array arrayInstance = Array.CreateInstance( typeof( T ).GetElementType(), adaptedValues.Length );

              for( int i = 0; i < adaptedValues.Length; i++ )
              {
                arrayInstance.SetValue( adaptedValues[i], i );
              }
              object convertedObject = Convert.ChangeType( arrayInstance, typeof( T ), new NumberFormatInfo() );
              collector.Add( (T) convertedObject );
            }
            else if( IsIListGeneric( typeof( T ) ) )
            {
              object list = Weborb.Util.ObjectFactories.CreateServiceObject( typeof( T ) );
              //object list = Activator.CreateInstance( typeof( T ) );

              for( int i = 0; i < adaptedValues.Length; i++ )
              {
               MethodInfo addMethod = list.GetType().GetMethod( "Add" );
               addMethod.Invoke( list, new object[] {adaptedValues[i]} );
              }

              collector.Add( (T) list );
            }
          }
        }
        else
        {
          T adaptedObject = (T) ((IAdaptingType) returnValue).adapt( typeof( T ) );
          collector.Add( adaptedObject );
        }
      }
     /* catch( Exception e )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Error while proccesing messsage" );
        if( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, e );
        throw e;
      }*/
    }

    private bool IsIListGeneric(Type type)
    {
      if ( type == null )
      {
        throw new ArgumentNullException( "type" );
      }
      foreach ( Type interfaceType in type.GetInterfaces() )
      {
        if ( interfaceType.IsGenericType )
        {
          if ( interfaceType.GetGenericTypeDefinition() == typeof( ICollection<> ) )
          {
            // if needed, you can also return the type used as generic argument
            return true;
          }
        }
      }
      return false;
    }
  }
}