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
      new Thread( () => rtClient.Connect() ).Start();
    }

    public void Disconnect()
    {
      rtClient.Disconnect();
    }

    public void Invoke( RTMethodRequest methodRequest )
    {
      new Thread( () => rtClient.Invoke( methodRequest ) ).Start();
    }

    public bool IsAvailable()
    {
      return rtClient.IsAvailable();
    }

    public bool IsConnected()
    {
      return rtClient.IsConnected();
    }

    public void SetConnectErrorEventListener( ConnectErrorListener fault )
    {
      rtClient.SetConnectErrorEventListener( fault );
    }

    public void SetConnectEventListener( ConnectListener callback )
    {
      rtClient.SetConnectEventListener( callback );
    }

    public void SetDisconnectEventListener( DisconnectListener callback )
    {
      rtClient.SetDisconnectEventListener( callback );
    }

    public void SetReconnectAttemptEventListener( ReconnectAttemptListener callback )
    {
      rtClient.SetReconnectAttemptEventListener( callback );
    }

    public void Subscribe( RTSubscription subscription )
    {
      Thread thread = new Thread( () => rtClient.Subscribe( subscription ) );
      thread.Start();
    }

    public void Unsubscribe( string subscriptionId )
    {
      Thread thread = new Thread( () => rtClient.Unsubscribe( subscriptionId ) );
      thread.Start();
    }

    public void UserLoggedIn( string userToken )
    {
      Thread thread = new Thread( () => rtClient.UserLoggedIn( userToken ) );
      thread.Start();
    }

    public void UserLoggedOut()
    {
      Thread thread = new Thread( () => rtClient.UserLoggedOut() );
      thread.Start();
    }
  }
}
