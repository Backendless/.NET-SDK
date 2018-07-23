using System;
using BackendlessAPI.RT.Command;

namespace BackendlessAPI.RT.Messaging
{
  public class MessagingCommandRequest : CommandRequest
  {
    public MessagingCommandRequest( String channel, IRTCallback callback ) : base( MethodTypes.PUB_SUB_COMMAND, callback )
    {
      PutOption( "channel", channel );
    }
  }
}
