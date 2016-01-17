using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Util;
using Weborb.Config;

namespace Weborb.Registry
{
  public class ServiceRegistry
  {
    private Dictionary<String, String> namedServices = new Dictionary<String, String>();
    private Dictionary<String, String> reversedMapping = new Dictionary<String, String>();
#if (FULL_BUILD)
    private Hashtable contexts = new Hashtable();
#endif
    public static string GetMapping( string name )
    {
      //return ThreadContext.getORBConfig().getServiceRegistry()._GetMapping( name );
      return ORBConfig.GetInstance().GetServiceRegistry()._GetMapping( name );
    }

    public static bool ContainsMappingFor( string name )
      {
      return ORBConfig.GetInstance().GetServiceRegistry()._ContainsMappingFor( name );
      }

    public bool _ContainsMappingFor( string name )
      {      
      return namedServices.ContainsKey( name );
      }

    public string _GetMapping( string name )
    {
      string mappedValue;

      namedServices.TryGetValue( name, out mappedValue );

      if( mappedValue == null )
        return name;
      else
        return mappedValue;
    }

    public static string GetReverseMapping( string mappedName )
    {
      //return ThreadContext.getORBConfig().getServiceRegistry()._GetReverseMapping( mappedName );
      return ORBConfig.GetInstance().GetServiceRegistry()._GetReverseMapping( mappedName );
    }

    public string _GetReverseMapping( string mappedName )
    {
      string mappedValue;

      reversedMapping.TryGetValue( mappedName, out mappedValue );

      if( mappedValue == null )
        return mappedName;
      else
        return mappedValue;
    }

    public static void AddMapping( string name, string mappedName )
    {
      AddMapping( name, mappedName, null );
    }

    public static void AddMapping( string name, string mappedName, IDictionary context )
    {
      //ThreadContext.getORBConfig().getServiceRegistry()._AddMapping( name, mappedName, context );
      ORBConfig.GetInstance().GetServiceRegistry()._AddMapping( name, mappedName, context );
    }

    public void _AddMapping( string name, string mappedName, IDictionary context )
    {
      namedServices[ name ] = mappedName;
      reversedMapping[ mappedName ] = name;

#if (FULL_BUILD)
      if( context != null )
        contexts[ mappedName ] = context;
#endif
    }

    public static void RemoveMapping( string name )
    {
      //ThreadContext.getORBConfig().getServiceRegistry()._RemoveMapping( name );
      ORBConfig.GetInstance().GetServiceRegistry()._RemoveMapping( name );

    }

    public void _RemoveMapping( string name )
    {
      string mappedName;

      namedServices.TryGetValue( name, out mappedName );

      if( mappedName != null )
      {
        namedServices.Remove( name );
        reversedMapping.Remove( mappedName );
      }
    }

#if (FULL_BUILD)
    public static Hashtable GetContext( string type )
    {
      //return ThreadContext.getORBConfig().getServiceRegistry()._GetContext( type );
      return ORBConfig.GetInstance().GetServiceRegistry()._GetContext( type );
    }

    public Hashtable _GetContext( string type )
    {
      return (Hashtable) contexts[ type ];
    }
#endif
  }
}
