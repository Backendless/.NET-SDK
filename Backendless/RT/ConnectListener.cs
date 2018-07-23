using System;
using System.Collections.Generic;
using BackendlessAPI.Async;

namespace BackendlessAPI.RT
{
  public abstract class ConnectListener<T> where T : RTSubscription
  {
    private readonly List<AsyncCallback<Object>> connectedCallbacks = new List<AsyncCallback<Object>>();
    private readonly IRTClient rtClient = RTClientFactory.Get();
    private Boolean connected;
    private T connectSubscription;
    protected String subject;

    public ConnectListener( String subject )
    {
      this.subject = subject;
      this.connectSubscription = CreateConnectSubscription( subject );
    }

    public void Connect()
    {
      rtClient.Subscribe( connectSubscription );
    }

    public void Disconnect()
    {
      rtClient.Unsubscribe( connectSubscription.Id );
      connected = false;
    }

    public Boolean IsConnected()
    {
      return connected;
    }

    public void AddConnectListener( AsyncCallback<Object> callback )
    {
      if( connected )
        callback.ResponseHandler( null );

      connectedCallbacks.Add( callback );
    }

    public void RemoveConnectListener( AsyncCallback<Object> callback )
    {
      connectedCallbacks.Remove( callback );
    }

    public void RemoveConnectListeners()
    {
      connectedCallbacks.Clear();
    }

    private T CreateConnectSubscription( String subject )
    {
      IRTCallback rtCallback = new RTCallback<Object>(
        response =>
        {
          connected = true;

          foreach( AsyncCallback<Object> connectedCallback in connectedCallbacks )
            connectedCallback.ResponseHandler( null );

          Connected();
        },
        fault =>
        {
          connected = false;

          foreach( AsyncCallback<Object> connectedCallback in connectedCallbacks )
            connectedCallback.ErrorHandler( fault );
        } );

      return CreateSubscription( rtCallback );
    }

    public abstract void Connected();
    public abstract T CreateSubscription( IRTCallback callback );
  }
}
