using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BackendlessAPI.Utils
{
  internal static class DeviceCheck
  {
    private static Dictionary<String, Type> cashedTypes = new Dictionary<String, Type>();
    private static Dictionary<String, String> notFoundTypes = new Dictionary<String, String>();

    internal static Boolean DeepTypeExists( String fullname )
    {
      if( cashedTypes.ContainsKey( fullname ) )
        return true;

      if( notFoundTypes.ContainsKey( fullname ) )
        return false;

      foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
      {
        foreach( Type type in assembly.GetTypes() )
        {
          if( type.FullName == fullname )
          {
            cashedTypes[ fullname ] = type;
            return true;
          }
        }
      }

      notFoundTypes[ fullname ] = null;
      return false;
    }

    internal static Type DeepLoadType( String fullname )
    {
      if( cashedTypes.ContainsKey( fullname ) )
        return cashedTypes[ fullname ];

      if( notFoundTypes.ContainsKey( fullname ) )
        return null;

      foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() )
        foreach( Type type in assembly.GetTypes() )
          if( type.FullName == fullname )
            return type;

      return null;
    }
  }
}
