using System;
using Weborb.Messaging.PubSub;
using System.Collections;
using System.Collections.Generic;

namespace Weborb.V3Types.Core
{
  public interface IServiceHandler
  {
    void HandleSubscribe( Subscriber subscriber, String clientId, CommandMessage message );

    void HandleUnsubscribe( Subscriber subscriber, String clientId, CommandMessage message );

    LinkedList<V3Message> GetMessages( Subscriber subscriber );

    void Initialize( IDestination destination );

    void AddMessage( Hashtable properties, Object message );
  }
}
