using System;
using System.Linq;
#if !(NET_40 || NET_35)
using System.CodeDom;
using System.Collections.Immutable;
#endif
using System.Threading;
using BackendlessAPI.Utils;
using BackendlessAPI.Engine;
using Weborb.Util.Logging;
using SocketIOClient;
using SocketIOClient.Transport;
using System.Collections.Generic;

namespace BackendlessAPI.RT
{
  internal abstract class SocketIOConnectionManager
  {
    private readonly RTLookupService rtLookupService;
    private readonly ITimeoutManager timeoutManager = new TimeoutManagerImpl();
    private SocketIO socket;
    internal bool connected = false;

    internal SocketIOConnectionManager()
    {
      rtLookupService = new RTLookupService( result =>
      {
        ReconnectAttempt( result.Attempt, result.Timeout );
        ConnectError( result.Error );
      }, timeoutManager );
    }

    internal SocketIO Socket
    {
      get
      {
        if( IsConnected() )
          return socket;

        var opts = new SocketIOOptions
        {
          Reconnection = false,
          Path = "/" + Backendless.AppId,
          Transport = TransportProtocol.WebSocket,
          EIO = EngineIO.V3,
          Query = new Dictionary<String, String>
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
            //opts.Transports = ImmutableList.Create( "websocket" );
        #else
        opts.Transports = (new string[] {"websocket"}).ToList();
        #endif
        var host = rtLookupService.Lookup() + opts.Path;
        //if( host.StartsWith( "https://" ) )
        //  host = "http://" + host.Substring( "https://".Length );

        if (HeadersManager.GetInstance().Headers.ContainsKey(HeadersEnum.USER_TOKEN_KEY.Header))
        {
          String userToken = HeadersManager.GetInstance().Headers[HeadersEnum.USER_TOKEN_KEY.Header];

          if (!string.IsNullOrEmpty(userToken))
            opts.Query.Concat(new Dictionary<String, String> { { "userToken", userToken } });
        }

        try
        {
          socket = new SocketIO(host, opts );
        }
        catch( System.Exception e )
        {
          ConnectError( e.Message );
          return Socket;
        }

        socket.OnConnected += (sender, args) =>
        {
          Log.log(Backendless.BACKENDLESSLOG, "Connected event " + args);
          connected = true;
          timeoutManager.Reset();
          Connected();
        };

        socket.OnDisconnected += (sender, args) =>
        {
          Log.log(Backendless.BACKENDLESSLOG, "Disconnected event {0}", args);
          connected = false;
          Disconnected(args.ToString());
          Reconnect();
        };

        socket.OnError += (sender, args) =>
        {
          Log.log(Backendless.BACKENDLESSLOG, "Connection failed {0}", args);
          connected = false;
          ConnectError(args.ToString());
          Reconnect();
        };

        socket.On("SUB_RES", args =>
        {
          Log.log(Backendless.BACKENDLESSLOG, "Got sub res");
          SubscriptionResult(args);
        });

        socket.On("MET_RES", args =>
        {
          Log.log(Backendless.BACKENDLESSLOG, "Got met res");
          InvocationResult(args);
        });

        socket.ConnectAsync();
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

    internal async void Disconnect()
    {
      Log.log( Backendless.BACKENDLESSLOG, "Try to disconnect" );

      if (socket != null)
      {
        SocketIO tempSocket = socket;
        socket = null;
        await tempSocket.DisconnectAsync();
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

