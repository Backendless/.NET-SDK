using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;

namespace BackendlessAPI.Utils
{
  public class UnderflowStore
  {
    private static Dictionary<object, IDictionary<string, object>> objectStore = new Dictionary<object,IDictionary<string, object>>();

    internal static void ReportObjectUnderFlow( object obj, System.Collections.IDictionary props )
    {
      IDictionary<string, object> properties = new Dictionary<string, object>();

      foreach( object key in props.Keys )
      {
        IAdaptingType adaptingType = (IAdaptingType) props[ key ];
        properties[ (string) key ] = adaptingType.defaultAdapt();
      }

      objectStore.Add( obj, properties );
    }

    public static IDictionary<string, object> GetObjectUnderflow( object obj )
    {
      if( objectStore.ContainsKey( obj ) )
        return objectStore[ obj ];

      return null;
    }
  }
}
