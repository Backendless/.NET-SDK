using System;
using System.Collections.Immutable;
using System.Threading;
using BackendlessAPI.Utils;
using BackendlessAPI.Engine;
using Quobject.SocketIoClientDotNet.Client;
using Weborb.Util.Logging;
using IOSocket = Quobject.SocketIoClientDotNet.Client.IO;

namespace BackendlessAPI.RT
{
  internal abstract class SocketIOConnectionManager
  {
    private RTLookupService rtLookupService;
    private ITimeoutManager timeoutManager = new TimeoutManagerImpl();
    private Socket socket;
    internal Boolean connected = false;

    internal SocketIOConnectionManager()
    {
      rtLookupService = new RTLookupService( result =>
      {
        ReconnectAttempt( result.Attempt, result.Timeout );
        ConnectError( result.Error );
      }, timeoutManager );
    }

    internal Socket Socket
    {
      get
      {
        if( IsConnected() )
          return socket;

        IOSocket.Options opts = new IOSocket.Options();
        opts.Reconnection = false;
        opts.Path = "/" + Backendless.AppId;
        opts.Query = new System.Collections.Generic.Dictionary<string, string>();
        opts.Query[ "apiKey" ] = Backendless.APIKey;
        opts.Query[ "clientId" ] = Backendless.Messaging.DeviceID;
        opts.Query[ "binary" ] = "true";
        opts.Transports = ImmutableList.Create<String>( "websocket" );
        String host = rtLookupService.Lookup() + opts.Path;

        //if( host.StartsWith( "https://" ) )
        //  host = "ws://" + host.Substring( "https://".Length );

        if( HeadersManager.GetInstance().Headers.ContainsKey( HeadersEnum.USER_TOKEN_KEY.Header ) )
        {
          String userToken = HeadersManager.GetInstance().Headers[ HeadersEnum.USER_TOKEN_KEY.Header ];

          if( userToken != null && userToken.Length > 0 )
            opts.Query[ "userToken" ] = userToken;
        }

        try
        {
          socket = IOSocket.Socket( host, opts );
        }
        catch( System.Exception e )
        {
          ConnectError( e.Message );
          return Socket;
        }

        socket.On( Socket.EVENT_CONNECT, ( fn ) =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "Connected event " + fn );
          connected = true;
          timeoutManager.Reset();
          Connected();
        } ).On( Socket.EVENT_DISCONNECT, ( fn ) =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "Disconnected event {0}", fn );
          connected = false;
          Disconnected( fn.ToString() );
          Reconnect();
        } ).On( Socket.EVENT_CONNECT_ERROR, ( fn ) =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "Connection failed {0}", fn );
          connected = false;
          ConnectError( fn.ToString() );
          Reconnect();
        } ).On( "SUB_RES", ( fn ) =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "Got sub res" );
          SubscriptionResult( fn );
        } ).On( "MET_RES", ( fn ) =>
        {
          Log.log( Backendless.BACKENDLESSLOG, "Got met res" );
          InvocationResult( fn );
        } ).On( Socket.EVENT_ERROR, ( fn ) =>
        {
          connected = false;
          Log.log( Backendless.BACKENDLESSLOG, String.Format( "ERROR from RT server: {0}", fn ) );
          ConnectError( fn.ToString() );
          Reconnect();
        } ).On( Socket.EVENT_CONNECT_TIMEOUT, ( fn ) =>
        {
          connected = false;
          Log.log( Backendless.BACKENDLESSLOG, "timeout" );
        } );

        //socket.Connect();
        return socket;
      }
    }

    public Boolean IsConnected()
    {
      return socket != null && connected;
    }

    private void Reconnect()
    {
      if( socket == null )
        return;

      Disconnect();
      int retryConnectTimeout = timeoutManager.NextTimeout();

      Log.log( Backendless.BACKENDLESSLOG, String.Format( "Wait for {0} before reconnect", retryConnectTimeout ) );

      Thread.Sleep( retryConnectTimeout );
      ReconnectAttempt( timeoutManager.RepeatedTimes(), retryConnectTimeout );
      var tempSocket = Socket;
    }

    internal void Disconnect()
    {
      Log.log( Backendless.BACKENDLESSLOG, "Try to disconnect" );

      if( socket != null )
        socket.Close();

      socket = null;
    }

    protected abstract void Connected();

    protected abstract void ReconnectAttempt( int attempt, int timeout );

    protected abstract void ConnectError( String error );

    protected abstract void Disconnected( String cause );

    protected abstract void SubscriptionResult( params Object[] args );

    protected abstract void InvocationResult( params Object[] args );
  }
}

