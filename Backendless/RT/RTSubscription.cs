using System;
namespace BackendlessAPI.RT
{
  public class RTSubscription : AbstractRequest
  {
    public RTSubscription( SubscriptionNames subscriptionName, IRTCallback callback ) : base( callback )
    {
      if( callback == null )
        throw new ArgumentException( "Callback cannot be null" );

      SubscriptionName = subscriptionName;
    }

    public override String Name {
      get {
        return Enum.GetName( typeof( SubscriptionNames ), SubscriptionName );
      }
    }

    public SubscriptionNames SubscriptionName {
      get;
      protected set;
    }

    public override string ToString()
    {
      return "RTSubscription{" + "id='" + Id + '\'' + ", callback=" + Callback + ", subscriptionName=" + SubscriptionName + ", options=" + Options + '}';
    }
  }
}
