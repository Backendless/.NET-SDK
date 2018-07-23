using System;
namespace BackendlessAPI.Exception
{
  public class NoSocketIOException : BackendlessException
  {
    public NoSocketIOException() : base( "To use the real-time functionality add the SocketIO dependency" )
    {
    }
  }
}
