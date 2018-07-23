using System;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public interface IRTClient
  {
    void Subscribe( RTSubscription subscription );

    void Unsubscribe( String subscriptionId );

    void UserLoggedIn( String userToken );

    void UserLoggedOut();

    void Invoke( RTMethodRequest methodRequest );

    void SetConnectEventListener( ResultHandler<object> callback );

    void SetReconnectAttemptEventListener( ResultHandler<ReconnectAttempt> callback );

    void SetConnectErrorEventListener( ResultHandler<BackendlessFault> fault );

    void SetDisconnectEventListener( ResultHandler<String> callback );

    Boolean IsConnected();

    void Connect();

    void Disconnect();

    Boolean IsAvailable();
  }
}
