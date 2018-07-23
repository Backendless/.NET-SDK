using System;

using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public interface IRTService
  {
    void AddConnectListener( ResultHandler<object> listener );
    void RemoveConnectListener( ResultHandler<object> listener );

    void AddReconnectAttemptListener( ResultHandler<ReconnectAttempt> listener );
    void RemoveReconnectAttemptListener( ResultHandler<ReconnectAttempt> listener );

    void AddConnectErrorListener( ResultHandler<BackendlessFault> listener );
    void RemoveConnectErrorListener( ResultHandler<BackendlessFault> listener );

    void AddDisconnectListener( ResultHandler<String> listener );
    void RemoveDisconnectListener( ResultHandler<String> listener );

    void RemoveConnectionListeners();

    void Connect();

    void Disconnect();
  }
}
