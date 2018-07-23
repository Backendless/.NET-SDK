using System;
using System.Collections.Generic;
namespace BackendlessAPI.RT.Data
{
  public class EventHandlerFactory //: AbstractListenerFactory<IEventHandler<T>>
  {
    public static IEventHandler<T> Of<T>()
    {
      return new EventHandlerImpl<T>( typeof( T ) );
    }

    public static IEventHandler<Dictionary<string, object>> Of( string tableName )
    {
      return new EventHandlerImpl<Dictionary<string, object>>( tableName );
    }
  }
}
