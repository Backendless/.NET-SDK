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

    void SetConnectEventListener( ConnectListener callback );

    void SetReconnectAttemptEventListener( ReconnectAttemptListener callback );

    void SetConnectErrorEventListener( ConnectErrorListener fault );

    void SetDisconnectEventListener( DisconnectListener callback );

    Boolean IsConnected();

    void Connect();

    void Disconnect();

    Boolean IsAvailable();
  }
}
