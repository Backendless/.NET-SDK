using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace BackendlessAPI.RT
{
  public delegate Boolean TestSubscription( RTSubscription subscriptionToCheck );

  public class RTListenerImpl 
  {
    private IRTClient rt = RTClientFactory.Get();

    #if NET_35
    private readonly IDictionary<String, RTSubscription> subscriptions = new Dictionary<string, RTSubscription>();
    #else
    private readonly ConcurrentDictionary<String, RTSubscription> subscriptions = new ConcurrentDictionary<string, RTSubscription>();
    #endif

    protected void AddEventListener( RTSubscription subscription )
    {
      #if NET_35
      subscriptions.Add( subscription.Id, subscription );
      #else
      subscriptions.AddOrUpdate( subscription.Id, subscription, ( key, oldValue ) => subscription );
      #endif
      rt.Subscribe( subscription );
    }

    protected void RemoveEventListener( RTSubscription subscription )
    {
      RemoveEventListener( ( subscriptionToCheck ) => { return subscriptionToCheck.Equals( subscription ); } );
    }

    protected void RemoveEventListener( TestSubscription testSubscription ) 
    {
      var subscriptionsToRemove = new List<string>();
      
      foreach( RTSubscription existingSubscription in subscriptions.Values )
        if( testSubscription( existingSubscription ) )
        {
          rt.Unsubscribe( existingSubscription.Id );
          subscriptionsToRemove.Add( existingSubscription.Id );
        }
      
      foreach( var id in subscriptionsToRemove )
          #if NET_35
          subscriptions.Remove( id );
          #else
          subscriptions.TryRemove( id, out _ );
          #endif
    }
  }
}
