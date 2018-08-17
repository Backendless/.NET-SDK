using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public class RTServiceImpl : IRTService
  {
    private readonly IRTClient rtClient = RTClientFactory.Get();

    private readonly List<ConnectListener> connectListeners = new List<ConnectListener>();
    private readonly List<DisconnectListener> disconnectListeners = new List<DisconnectListener>();
    private readonly List<ReconnectAttemptListener> reconnectListeners = new List<ReconnectAttemptListener>();
    private readonly List<ConnectErrorListener> connectErrorListeners = new List<ConnectErrorListener>();

    public RTServiceImpl()
    {
      if( rtClient.IsAvailable() )
      {
        rtClient.SetConnectEventListener( () =>
        {
          foreach( ConnectListener listener in connectListeners )
          {
            listener();
          }
        } );

        rtClient.SetDisconnectEventListener( ( result ) =>
         {
           foreach( DisconnectListener listener in disconnectListeners )
           {
             listener( result );
           }
         } );

        rtClient.SetConnectErrorEventListener( ( result ) =>
         {
           foreach( ConnectErrorListener listener in connectErrorListeners )
           {
             listener( result );
           }
         } );

        rtClient.SetReconnectAttemptEventListener( ( result ) =>
         {
           foreach( ReconnectAttemptListener listener in reconnectListeners )
           {
             listener( result );
           }
         } );
      }
    }

    public void AddConnectErrorListener( ConnectErrorListener faultHandler )
    {
      connectErrorListeners.Add( faultHandler );
    }

    public void RemoveConnectErrorListener( ConnectErrorListener listener )
    {
      connectErrorListeners.Remove( listener );
    }

    public void AddConnectListener( ConnectListener connectListener )
    {
      if( rtClient.IsConnected() )
        connectListener();

      connectListeners.Add( connectListener );
    }

    public void RemoveConnectListener( ConnectListener listener )
    {
      connectListeners.Remove( listener );
    }

    public void AddDisconnectListener( DisconnectListener disconnectHandler )
    {
      disconnectListeners.Add( disconnectHandler );
    }

    public void RemoveDisconnectListener( DisconnectListener listener )
    {
      disconnectListeners.Remove( listener );
    }

    public void AddReconnectAttemptListener( ReconnectAttemptListener reconnectAttemptHandler )
    {
      reconnectListeners.Add( reconnectAttemptHandler );
    }

    public void RemoveReconnectAttemptListener( ReconnectAttemptListener listener )
    {
      reconnectListeners.Remove( listener );
    }


    public void Connect()
    {
      rtClient.Connect();
    }

    public void Disconnect()
    {
      rtClient.Disconnect();
    }

    public void RemoveConnectionListeners()
    {
      connectListeners.Clear();
    }
  }
}
