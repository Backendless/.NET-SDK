using System;
using System.Threading;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class Subscription
  {
    private Timer _timer;

    private int _pollingInterval = 1000;

    public Subscription()
    {
    }

    public Subscription( int pollingInterval )
    {
      this._pollingInterval = pollingInterval;
    }

    [SetClientClassMemberName( "subscriptionId" )]
    public string SubscriptionId { get; set; }

    [SetClientClassMemberName( "channelName" )]
    public string ChannelName { get; set; }

    [SetClientClassMemberNameAttribute( "pollingInterval" )]
    public int PollingInterval
    {
      get { return this._pollingInterval; }
      set { this._pollingInterval = value; }
    }

    public bool CancelSubscription()
    {
      if( _timer != null )
      {
        _timer.Change( Timeout.Infinite, Timeout.Infinite );
        _timer = null;
      }

      SubscriptionId = null;
      return true;
    }

    public void PauseSubscription()
    {
      if( _timer != null )
      {
        _timer.Change( Timeout.Infinite, Timeout.Infinite );
      }
    }

    public void ResumeSubscription()
    {
      if( SubscriptionId == null || ChannelName == null || _timer == null )
        throw new ArgumentNullException( ExceptionMessage.WRONG_SUBSCRIPTION_STATE );

      _timer.Change( 0, _pollingInterval );
    }

    public void OnSubscribe( AsyncCallback<List<Message>> callback )
    {
      _timer = new Timer( c =>
      {

        var message = Backendless.Messaging.PollMessages( ChannelName, SubscriptionId );
        if( message.Count == 0 )
          return;

        var callback1 = (AsyncCallback<List<Message>>) c;
        callback1.ResponseHandler.Invoke( message );
      }, callback, 0, _pollingInterval );
    }
  }
}
