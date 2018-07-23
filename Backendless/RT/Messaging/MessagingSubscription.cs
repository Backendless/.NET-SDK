using System;
using BackendlessAPI.RT;

namespace BackendlessAPI.RT.Messaging
{
  public class MessagingSubscription : RTSubscription
  {
    internal MessagingSubscription( SubscriptionNames subscriptionName, IRTCallback callback ) : base( subscriptionName, callback )
    {
    }

    public static MessagingSubscription Connect( String channel, IRTCallback callback )
    {
      if( channel == null || channel.Length == 0 )
        throw CreateChannelNullException();

      MessagingSubscription messagingSubscription = new MessagingSubscription( SubscriptionNames.PUB_SUB_CONNECT, callback );
      messagingSubscription.PutOption( "channel", channel );

      return messagingSubscription;
    }

    private static ArgumentException CreateChannelNullException()
    {
      return new ArgumentException( "channel name can't be null or empty" );
    }

    public static MessagingSubscription Subscribe( String channel, IRTCallback callback )
    {
      if( channel == null || channel.Length == 0 )
        throw CreateChannelNullException();

      MessagingSubscription messagingSubscription = new MessagingSubscription( SubscriptionNames.PUB_SUB_MESSAGES, callback );
      messagingSubscription.PutOption( "channel", channel );

      return messagingSubscription;
    }

    public static MessagingSubscription Subscribe( String channel, String selector, IRTCallback callback )
    {
      MessagingSubscription messagingSubscription = Subscribe( channel, callback );
      messagingSubscription.PutOption( "selector", selector );
      return messagingSubscription;
    }

    public static MessagingSubscription Command( String channel, IRTCallback callback )
    {
      if( channel == null || channel.Length == 0 )
        throw CreateChannelNullException();

      MessagingSubscription messagingSubscription = new MessagingSubscription( SubscriptionNames.PUB_SUB_COMMANDS, callback );
      messagingSubscription.PutOption( "channel", channel );

      return messagingSubscription;
    }

    public static MessagingSubscription UserStatus( String channel, IRTCallback callback )
    {
      if( channel == null || channel.Length == 0 )
        throw CreateChannelNullException();

      MessagingSubscription messagingSubscription = new MessagingSubscription( SubscriptionNames.PUB_SUB_USERS, callback );
      messagingSubscription.PutOption( "channel", channel );

      return messagingSubscription;
    }

    public String Selector
    {
      get
      {
        return (String) GetOption( "selector" );
      }
    }
  }
}
