using System;
using System.Collections.Generic;

namespace Weborb.V3Types.Core
{
    public class RemotingHandler : IServiceHandler
    {
        #region IServiceHandler Members

        public void ProcessMessage(Object message)
        {
        }

        public void HandleSubscribe( Weborb.Messaging.PubSub.Subscriber subscriber, String clientId, CommandMessage commandMessage )
        {
        }

        public void HandleUnsubscribe( Weborb.Messaging.PubSub.Subscriber subscriber, String clientId, CommandMessage message )
        {
        }

        public LinkedList<V3Message> GetMessages(Weborb.Messaging.PubSub.Subscriber subscriber)
        {
            return new LinkedList<V3Message>();
        }

        public void Initialize(IDestination destination)
        {
        }

        public void AddMessage(System.Collections.Hashtable properties, object message)
        {
        }

        #endregion
    }
}
