using System;
using System.Linq;
#if !(NET_40 || NET_35)
using System.CodeDom;
using System.Collections.Immutable;
#endif
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
    private readonly RTLookupService rtLookupService;
    private readonly ITimeoutManager timeoutManager = new TimeoutManagerImpl();
    private Socket socket;
    internal bool connected = false;

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

        var opts = new IOSocket.Options
        {
          Reconnection = false,
          Path = "/" + Backendless.AppId,
          Query = new System.Collections.Generic.Dictionary<string, string>
          {
            [ "apiKey" ] = Backendless.APIKey,
            [ "clientId" ] = Backendless.Messaging.DeviceID,
            [ "binary" ] = "true"
          }
        };

      #if NET_35
        opts.QueryString =
          $"apiKey={Backendless.APIKey}&clientId={Backendless.Messaging.DeviceID}&binary=true";
        #endif
        
        #if( NET_45 )
          opts.Transports = Quobject.Collections.Immutable.ImmutableList.Create( "websocket" );
        #elif !(NET_40 || NET_35)
            opts.Transports = ImmutableList.Create( "websocket" );
        #else
        opts.Transports = (new string[] {"websocket"}).ToList();
        #endif
        var host = rtLookupService.Lookup() + opts.Path;

        //if( host.StartsWith( "https://" ) )
        //  host = "http://" + host.Substring( "https://".Length );

        if( HeadersManager.GetInstance().Headers.ContainsKey( HeadersEnum.USER_TOKEN_KEY.Header ) )
        {
          String userToken = HeadersManager.GetInstance().Headers[ HeadersEnum.USER_TOKEN_KEY.Header ];

          if( !string.IsNullOrEmpty(userToken) )
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
          Log.log( Backendless.BACKENDLESSLOG, $"ERROR from RT server: {fn}" );
          ConnectError( fn.ToString() );
          Reconnect();
        } ).On( Socket.EVENT_CONNECT_TIMEOUT, ( fn ) =>
        {
          connected = false;
          Log.log( Backendless.BACKENDLESSLOG, "timeout" );
        } );
        
        return socket;
      }
    }

    public bool IsConnected()
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

      if (socket != null)
      {
        Socket tempSocket = socket;
        socket = null;
        tempSocket.Close();
      }
    }

    protected abstract void Connected();

    protected abstract void ReconnectAttempt( int attempt, int timeout );

    protected abstract void ConnectError( string error );

    protected abstract void Disconnected( string cause );

    protected abstract void SubscriptionResult( params object[] args );

    protected abstract void InvocationResult( params object[] args );
  }
}

