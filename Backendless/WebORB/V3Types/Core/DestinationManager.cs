using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb.Management.Messaging;
using Weborb.Util;
using Weborb.Config;

namespace Weborb.V3Types.Core
{
  internal class DestinationManager
  {
    private Dictionary<String, IDestination> destinations = new Dictionary<String, IDestination>();

    public static DestinationManager GetInstance()
    {
      return ORBConfig.GetInstance().GetDataServices().GetDestinationManager();
    }

    public void RemoveDestination( string id )
    {
      IDestination destination = (IDestination)destinations[id];
      destinations.Remove( id );
      DestinationMessageCountWatcher.getInstance().unregister( destination );
    }

    public void AddDestination( string id, IDestination destination )
    {
      destinations[id] = destination;
      DestinationMessageCountWatcher.getInstance().register( destination );
    }

    public IDestination GetDestination( string id )
    {
      if ( destinations.ContainsKey( id ) )
        return destinations[id];
      else
        return null;
    }

    public List<T> GetDestinations<T>()
    {
      List<T> result = new List<T>();

      foreach ( IDestination destination in destinations.Values )
      {
        if ( destination is T )
          result.Add( (T)destination );
      }

      return result;
    }

    public Dictionary<String, IDestination> GetDestinations()
    {
      return destinations;
    }
  }
}
