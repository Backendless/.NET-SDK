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

    [SetClientClassMemberName( "pushBroadcast" )]
    public int PushBroadcast { get; set; }

    [SetClientClassMemberName( "pushSinglecast" )]
    public List<string> PushSinglecast { get; set; }

    [SetClientClassMemberName( "publishAt" )]
    public DateTime? PublishAt { get; set; }

    [SetClientClassMemberName( "repeatEvery" )]
    public long RepeatEvery { get; set; }

    [SetClientClassMemberName( "repeatExpiresAt" )]
    public DateTime? RepeatExpiresAt { get; set; }

    [SetClientClassMemberNameAttribute( "pushPolicy" )]
    public PushPolicyEnum PushPolicy
    {
      get { return _pushPolicy; }
      set { _pushPolicy = value; }
    }
  }
}
