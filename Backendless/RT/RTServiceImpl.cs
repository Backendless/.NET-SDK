using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public class RTServiceImpl : IRTService
  {
    private readonly IRTClient rtClient = RTClientFactory.Get();

    private List<ResultHandler<object>> connectListeners = new List<ResultHandler<object>>();
    private List<ResultHandler<String>> disconnectListeners = new List<ResultHandler<string>>();
    private List<ResultHandler<ReconnectAttempt>> reconnectListeners = new List<ResultHandler<ReconnectAttempt>>();
    private List<ResultHandler<BackendlessFault>> connectErrorListeners = new List<ResultHandler<BackendlessFault>>();

    public RTServiceImpl()
    {
      if( rtClient.IsAvailable() )
      {
        rtClient.SetConnectEventListener( ( result ) =>
        {
          foreach( ResultHandler<object> listener in connectListeners )
          {
            listener( result );
          }
        } );

        rtClient.SetDisconnectEventListener( ( result ) =>
         {
           foreach( ResultHandler<String> listener in disconnectListeners )
           {
             listener( result );
           }
         } );

        rtClient.SetConnectErrorEventListener( ( result ) =>
         {
           foreach( ResultHandler<BackendlessFault> listener in connectErrorListeners )
           {
             listener( result );
           }
         } );

        rtClient.SetReconnectAttemptEventListener( ( result ) =>
         {
           foreach( ResultHandler<ReconnectAttempt> listener in reconnectListeners )
           {
             listener( result );
           }
         } );
      }
    }

    public void AddConnectErrorListener( ResultHandler<BackendlessFault> faultHandler )
    {
      connectErrorListeners.Add( faultHandler );
    }

    public void RemoveConnectErrorListener( ResultHandler<BackendlessFault> listener )
    {
      connectErrorListeners.Remove( listener );
    }

    public void AddConnectListener( ResultHandler<object> connectHandler )
    {
      if( rtClient.IsConnected() )
        connectHandler( null );

      connectListeners.Add( connectHandler );
    }

    public void RemoveConnectListener( ResultHandler<object> listener )
    {
      connectListeners.Remove( listener );
    }

    public void AddDisconnectListener( ResultHandler<string> disconnectHandler )
    {
      disconnectListeners.Add( disconnectHandler );
    }

    public void RemoveDisconnectListener( ResultHandler<String> listener )
    {
      disconnectListeners.Remove( listener );
    }

    public void AddReconnectAttemptListener( ResultHandler<ReconnectAttempt> reconnectAttemptHandler )
    {
      reconnectListeners.Add( reconnectAttemptHandler );
    }

    public void RemoveReconnectAttemptListener( ResultHandler<ReconnectAttempt> listener )
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
