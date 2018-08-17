using System;
using System.Collections.Generic;
#if !NET_35
using System.Collections.Concurrent;
#endif  
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Utils;
using Weborb.Util.Logging;
using Weborb.Types;
using Weborb.Reader;
using Quobject.EngineIoClientDotNet.ComponentEmitter;
using Quobject.SocketIoClientDotNet.Client;

namespace BackendlessAPI.RT
{
  public class RTClientSocketIO : IRTClient
  {
    private readonly SocketIOConnectionManager connectionManager;

    #if NET_35
    private IDictionary<String, RTSubscription> subscriptions = new Dictionary<string, RTSubscription>();
    private IDictionary<String, RTMethodRequest> sentRequests = new Dictionary<string, RTMethodRequest>();
    private Queue<RTMethodRequest> methodsToSend = new Queue<RTMethodRequest>();
  #else
    private ConcurrentDictionary<String, RTSubscription> subscriptions = new ConcurrentDictionary<string, RTSubscription>();
    private ConcurrentDictionary<String, RTMethodRequest> sentRequests = new ConcurrentDictionary<string, RTMethodRequest>();
    private ConcurrentQueue<RTMethodRequest> methodsToSend = new ConcurrentQueue<RTMethodRequest>();
    #endif
    private ConnectListener connectCallback;
    private ConnectErrorListener connectErrorCallback;
    private DisconnectListener disconnectCallback;
    private ReconnectAttemptListener reconnectAttemptCallback;

    public RTClientSocketIO()
    {
      connectionManager = new ConnectionManager( this );
    }

    public void Connect()
    {
      var socket = connectionManager.Socket;
    }

    public void Disconnect()
    {
      connectionManager.Disconnect();
    }

    public void Invoke( RTMethodRequest methodRequest )
    {
      sentRequests[ methodRequest.Id ] = methodRequest;

      if( connectionManager.IsConnected() )
        MetReq( methodRequest );
      else
        methodsToSend.Enqueue( methodRequest );
    }

    public bool IsAvailable()
    {
      return true;
    }

    public bool IsConnected()
    {
      return connectionManager.IsConnected();
    }

    public void SetConnectErrorEventListener( ConnectErrorListener fault )
    {
      connectErrorCallback = fault;
    }

    public void SetConnectEventListener( ConnectListener callback )
    {
      connectCallback = callback;
    }

    public void SetDisconnectEventListener( DisconnectListener callback )
    {
      disconnectCallback = callback;
    }

    public void SetReconnectAttemptEventListener( ReconnectAttemptListener callback )
    {
      reconnectAttemptCallback = callback;
    }

    public void Subscribe( RTSubscription subscription )
    {
      Log.log( Backendless.BACKENDLESSLOG, String.Format( "try to subscribe {0}", subscription ) );
      subscriptions[ subscription.Id ] = subscription;

      if( connectionManager.Socket != null && connectionManager.connected ) // Socket.Io().ReadyState == Manager.ReadyStateEnum.OPEN )
        SubOn( subscription );
    }

    public void Unsubscribe( string subscriptionId )
    {
      Log.log( Backendless.BACKENDLESSLOG, String.Format( "unsubscribe for {0} subscrition called", subscriptionId ) );

      if( !subscriptions.ContainsKey( subscriptionId ) )
      {

        Log.log( Backendless.BACKENDLESSLOG, String.Format( "subscriber {0} is not subscribed", subscriptionId ) );
        return;
      }

      SubOff( subscriptionId );
      #if NET_35
      subscriptions.Remove( subscriptionId );
      #else
      subscriptions.TryRemove( subscriptionId, out _ );
      #endif
      Log.log( Backendless.BACKENDLESSLOG, "subscription removed" );

      if( subscriptions.Count == 0 && sentRequests.Count == 0 )
        connectionManager.Disconnect();
    }

    public void UserLoggedIn( string userToken )
    {
      if( connectionManager.IsConnected() )
      {
        var rtCallback = new RTCallback<Object>( result =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "user logged in/out successfully" );
        }, error =>
        {
          Log.log( Backendless.BACKENDLESSLOG, String.Format( "got error {0}", error ) );
        } );
        RTMethodRequest methodRequest = new RTMethodRequest( MethodTypes.SET_USER_TOKEN, rtCallback );

        methodRequest.PutOption( "userToken", userToken );
        Invoke( methodRequest );
      }
    }

    public void UserLoggedOut()
    {
      UserLoggedIn( null );
    }

    private void Resubscribe()
    {
      foreach (RTSubscription rtSubscription in subscriptions.Values)
        SubOn(rtSubscription);

      RTMethodRequest methodRequest = null;
#if NET_35
      if( methodsToSend.Count > 0 )
        methodRequest = methodsToSend.Dequeue();
#else
      methodsToSend.TryDequeue( out methodRequest );
      #endif

      while (methodRequest != null)
      {
        MetReq(methodRequest);
#if NET_35
        if( methodsToSend.Count > 0 )
          methodRequest = methodsToSend.Dequeue();
#else
      methodsToSend.TryDequeue( out methodRequest );
      #endif
      }
    }

    private Emitter SubOn( RTSubscription subscription )
    {
      Emitter emitter = connectionManager.Socket.Emit( "SUB_ON", WeborbSerializationHelper.Serialize( subscription.ToArgs() ) );
      Log.log( Backendless.BACKENDLESSLOG, "subOn called" );
      return emitter;
    }

    private Emitter SubOff( String subscriptionId )
    {
      Emitter emitter = connectionManager.Socket.Emit( "SUB_OFF", WeborbSerializationHelper.Serialize( subscriptionId ) );
      Log.log( Backendless.BACKENDLESSLOG, "subOff called" );
      return emitter;
    }

    private Emitter MetReq( RTMethodRequest methodRequest )
    {
      Emitter emitter = connectionManager.Socket.Emit( "MET_REQ", WeborbSerializationHelper.Serialize( methodRequest.ToArgs() ) );
      Log.log( Backendless.BACKENDLESSLOG, "metReq called" );
      return emitter;
    }


    private IRTRequest HandleResult<T>( Object[] args, IDictionary<String, T> requestMap, String resultKey ) where T : IRTRequest
    {
      if( args == null || args.Length < 1 )
      {
        Log.log( Backendless.BACKENDLESSLOG, "subscription result is null or empty" );
        return null;
      }

      AnonymousObject result = (AnonymousObject) WeborbSerializationHelper.Deserialize( (byte[]) args[ 0 ] );

      String id = WeborbSerializationHelper.AsString( result, "id" );

      Log.log( Backendless.BACKENDLESSLOG, String.Format( "Got result for subscription {0}", id ) );

      IRTRequest request = requestMap[ id ];

      if( request == null )
      {
        Log.log( Backendless.BACKENDLESSLOG, String.Format( "There is no handler for subscription {0}", id ) );
        return null;
      }

      Object error = WeborbSerializationHelper.AsObject( result, "error" );

      if( error != null )
      {
        Log.log( Backendless.BACKENDLESSLOG, String.Format( "got error {0}", error ) );
        BackendlessFault fault = new BackendlessFault( error.ToString() );
        request.Callback.errorHandler( fault );
        return request;
      }

      IAdaptingType data = WeborbSerializationHelper.AsAdaptingType( result, resultKey );
      request.Callback.responseHandler( data );
      return request;
    }

    internal class ConnectionManager : SocketIOConnectionManager
    {
      RTClientSocketIO parent;

      internal ConnectionManager( RTClientSocketIO parent )
      {
        this.parent = parent;
      }

      protected override void Connected()
      {
        parent.Resubscribe();
        parent.connectCallback();
      }

      protected override void ConnectError( string error )
      {
        parent.connectErrorCallback( new BackendlessFault( error ) );
      }

      protected override void Disconnected( string cause )
      {
        parent.disconnectCallback( cause );
      }

      protected override void InvocationResult( params object[] args )
      {
        IRTRequest request = parent.HandleResult( args, parent.sentRequests, "result" );

        if( request != null )
          #if NET_35
          parent.sentRequests.Remove( request.Id );
          #else
          parent.sentRequests.TryRemove( request.Id, out _ );
          #endif
      }

      protected override void ReconnectAttempt( int attempt, int timeout )
      {
        parent.reconnectAttemptCallback( new ReconnectAttempt( attempt, timeout ) );
      }

      protected override void SubscriptionResult( params object[] args )
      {
        parent.HandleResult( args, parent.subscriptions, "data" );
      }
    }
  }
}
