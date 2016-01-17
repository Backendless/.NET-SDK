using System;
using System.Collections.Generic;

namespace Weborb.Service
{
  public class PropertyRenaming
  {
    private static Dictionary<Type, Dictionary<String, String>> rules = new Dictionary<Type, Dictionary<string, string>>();

    public static void AddRenamingRule( Type clazz, String propName, String renameTo )
    {
      Dictionary<String, String> nameMappingForClass = null;

      if( !rules.ContainsKey( clazz ) )
      {
        nameMappingForClass = new Dictionary<String, String>();
        rules.Add( clazz, nameMappingForClass );
      }

      nameMappingForClass = rules[ clazz ];
      nameMappingForClass.Add( propName, renameTo );
    }

    public static String GetRenamingRule( Type clazz, String propName )
    {
      if( rules.ContainsKey( clazz ) )
      {
        Dictionary<String, String> nameMappingForClass = rules[ clazz ];

        if( nameMappingForClass.ContainsKey( propName ) )
          return nameMappingForClass[ propName ];
      }

      return propName;
    }
  }
}
