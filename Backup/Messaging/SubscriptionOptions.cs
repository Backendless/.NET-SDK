using Weborb.Service;

namespace BackendlessAPI.Messaging
{
    public class SubscriptionOptions
    {
      public SubscriptionOptions()
        {
        }

        public SubscriptionOptions(string subscriberId)
        {
            this.SubscriberId = subscriberId;
        }

        public SubscriptionOptions(string subscriberId, string subtopic)
        {
            this.SubscriberId = subscriberId;
            this.Subtopic = subtopic;
        }

        public SubscriptionOptions(string subscriberId, string subtopic, string selector)
        {
            this.SubscriberId = subscriberId;
            this.Subtopic = subtopic;
            this.Selector = selector;
        }

      [SetClientClassMemberName( "subscriberId" )]
      public string SubscriberId { get; set; }

      [SetClientClassMemberName( "subtopic" )]
      public string Subtopic { get; set; }

      [SetClientClassMemberName( "selector" )]
      public string Selector { get; set; }
    }
}
