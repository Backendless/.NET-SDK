using System;

using System.Collections.Concurrent;

namespace BackendlessAPI.RT
{
  public delegate Boolean TestSubscription( RTSubscription subscriptionToCheck );

  public class RTListenerImpl 
  {
    private IRTClient rt = RTClientFactory.Get();

    private readonly ConcurrentDictionary<String, RTSubscription> subscriptions = new ConcurrentDictionary<string, RTSubscription>();

    protected void AddEventListener( RTSubscription subscription )
    {
      subscriptions.AddOrUpdate( subscription.Id, subscription, ( key, oldValue ) => subscription );
      rt.Subscribe( subscription );
    }

    protected void RemoveEventListener( RTSubscription subscription )
    {
      RemoveEventListener( ( subscriptionToCheck ) => { return subscriptionToCheck.Equals( subscription ); } );
    }

    protected void RemoveEventListener( TestSubscription testSubscription ) 
    {
      foreach( RTSubscription existingSubscription in subscriptions.Values )
      {
        if( testSubscription( existingSubscription ) )
        {
          rt.Unsubscribe( existingSubscription.Id );
          subscriptions.TryRemove( existingSubscription.Id, out _ );
        }
      }
    }
  }
}
