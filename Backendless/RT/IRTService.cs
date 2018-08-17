using System;

using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public delegate void ConnectListener();
  public delegate void ReconnectAttemptListener( ReconnectAttempt reconnectAttempt );
  public delegate void ConnectErrorListener( BackendlessFault fault );
  public delegate void DisconnectListener( String cause );
    
  public interface IRTService
  {
    void AddConnectListener( ConnectListener listener );
    void RemoveConnectListener( ConnectListener listener );

    void AddReconnectAttemptListener( ReconnectAttemptListener listener );
    void RemoveReconnectAttemptListener( ReconnectAttemptListener listener );

    void AddConnectErrorListener( ConnectErrorListener listener );
    void RemoveConnectErrorListener( ConnectErrorListener listener );

    void AddDisconnectListener( DisconnectListener listener );
    void RemoveDisconnectListener( DisconnectListener listener );

    void RemoveConnectionListeners();

    void Connect();

    void Disconnect();
  }
}
