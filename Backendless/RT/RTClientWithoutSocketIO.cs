using System;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public class RTClientWithoutSocketIO : IRTClient
  {
    public void Connect()
    {
      throw new NoSocketIOException();
    }

    public void Disconnect()
    {
      throw new NoSocketIOException();
    }

    public void Invoke( RTMethodRequest methodRequest )
    {
      throw new NoSocketIOException();
    }

    public bool IsAvailable()
    {
      throw new NoSocketIOException();
    }

    public bool IsConnected()
    {
      throw new NoSocketIOException();
    }

    public void SetConnectErrorEventListener( ResultHandler<BackendlessFault> fault )
    {
      throw new NoSocketIOException();
    }

    public void SetConnectEventListener( ResultHandler<object> callback )
    {
      throw new NoSocketIOException();
    }

    public void SetDisconnectEventListener( ResultHandler<string> callback )
    {
      throw new NoSocketIOException();
    }

    public void SetReconnectAttemptEventListener( ResultHandler<ReconnectAttempt> callback )
    {
      throw new NoSocketIOException();
    }

    public void Subscribe( RTSubscription subscription )
    {
      throw new NoSocketIOException();
    }

    public void Unsubscribe( string subscriptionId )
    {
      throw new NoSocketIOException();
    }

    public void UserLoggedIn( string userToken )
    {
      throw new NoSocketIOException();
    }

    public void UserLoggedOut()
    {
      throw new NoSocketIOException();
    }
  }
}
