using System;
using System.Threading;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace BackendlessAPI.RT
{
  public class AsyncRTClient : IRTClient
  {
    private static readonly IRTClient rtClient = new RTClientSocketIO();

    public void Connect()
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.Connect() ) );
    }

    public void Disconnect()
    {
      rtClient.Disconnect();
    }

    public void Invoke( RTMethodRequest methodRequest )
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.Invoke( methodRequest ) ) );
    }

    public bool IsAvailable()
    {
      return rtClient.IsAvailable();
    }

    public bool IsConnected()
    {
      return rtClient.IsConnected();
    }

    public void SetConnectErrorEventListener( ResultHandler<BackendlessFault> fault )
    {
      rtClient.SetConnectErrorEventListener( fault );
    }

    public void SetConnectEventListener( ResultHandler<object> callback )
    {
      rtClient.SetConnectEventListener( callback );
    }

    public void SetDisconnectEventListener( ResultHandler<string> callback )
    {
      rtClient.SetDisconnectEventListener( callback );
    }

    public void SetReconnectAttemptEventListener( ResultHandler<ReconnectAttempt> callback )
    {
      rtClient.SetReconnectAttemptEventListener( callback );
    }

    public void Subscribe( RTSubscription subscription )
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.Subscribe( subscription ) ) );
      thread.Start();
    }

    public void Unsubscribe( string subscriptionId )
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.Unsubscribe( subscriptionId ) ) );
      thread.Start();
    }

    public void UserLoggedIn( string userToken )
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.UserLoggedIn( userToken ) ) );
      thread.Start();
    }

    public void UserLoggedOut()
    {
      Thread thread = new Thread( new ThreadStart( () => rtClient.UserLoggedOut() ) );
      thread.Start();
    }
  }
}
