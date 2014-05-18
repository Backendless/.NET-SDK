using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class DeliveryOptions
  {
    public const int IOS = 1;
    public const int ANDROID = 1 << 1;
    public const int WP = 1 << 2;
    public const int ALL = IOS | ANDROID | WP;

    private PushPolicyEnum _pushPolicy = PushPolicyEnum.ALSO;

    // accepts a mask value used by Backendless to route the message
    // to the registered devices with the specified operating system. 
    // The mask value may consist of the following values:
    // DeliveryOptions.IOS, DeliveryOptions.ANDROID, DeliveryOptions.WP and PushBroadcast.ALL
    [SetClientClassMemberName( "pushBroadcast" )]
    public int PushBroadcast { get; set; }

    // configures a list of registered device IDs to deliver the message to
    [SetClientClassMemberName( "pushSinglecast" )]
    public List<string> PushSinglecast { get; set; }

    // sets the time when the message should be published
    [SetClientClassMemberName( "publishAt" )]
    public DateTime? PublishAt { get; set; }

    // sets the interval as the number of milliseconds repeated message publications.
    // When a value is set Backendless re-publishes the message with the interval.
    [SetClientClassMemberName( "repeatEvery" )]
    public long RepeatEvery { get; set; }

    // sets the time when the message republishing configured with "repeatEvery" should stop
    [SetClientClassMemberName( "repeatExpiresAt" )]
    public DateTime? RepeatExpiresAt { get; set; }

    // Controls if the published messages are distributed as a push notification ONLY or
    // both push notification and ALSO as regular publish/subscribe messages.
    // There are two values ONLY and ALSO which correspond to the described scenarios.
    [SetClientClassMemberNameAttribute( "pushPolicy" )]
    public PushPolicyEnum PushPolicy
    {
      get { return _pushPolicy; }
      set { _pushPolicy = value; }
    }
  }
}
