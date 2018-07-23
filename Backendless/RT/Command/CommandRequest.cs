using System;
using BackendlessAPI.RT;

namespace BackendlessAPI.RT.Command
{
  public class CommandRequest : RTMethodRequest
  {
    public CommandRequest( MethodTypes methodType, IRTCallback callback ) : base( methodType, callback )
    {
    }

    internal CommandRequest SetData( Object data )
    {
      PutOption( "data", data );
      return this;
    }

    internal CommandRequest SetType( String type )
    {
      PutOption( "type", type );
      return this;
    }
  }
}
