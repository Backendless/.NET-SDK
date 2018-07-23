using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
{
  public class ObjectReader : IJsonReader
  {
    public static String CLASSNAMEPROPERTY = "___jsonclass";
    public static String ARRAYREF = "___arrayref";
    public static String DATEREF = "___dateref";
    public static String OBJECTREF = "___objectref";
    public static String STRINGREF = "___stringref";
    public static String DATESMETA = "___dates___";
    private static HashSet<String> reservedKeyNames = new HashSet<String>() { OBJECTREF, STRINGREF, DATEREF, ARRAYREF };

    public IAdaptingType read( JsonReader reader, ParseContext parseContext )
    {
      CacheableAdaptingTypeWrapper typeProxy = new CacheableAdaptingTypeWrapper();
      IDictionary properties = new Hashtable();

      Boolean referencedAdded = false;
      // skip '[' and move to next element
      reader.Read();

      while( reader.TokenClass != JsonTokenClass.EndObject )
      {
        string name = reader.ReadMember();

        if( !referencedAdded && !reservedKeyNames.Contains( name ) )
        {
          referencedAdded = true;
          parseContext.addReference( typeProxy );
        }

        if( name.Equals( DATESMETA ) )
          parseContext.TurnOnIgnoreMode();

        properties.Add( name, RequestParser.Read( reader ) );

        if( name.Equals( DATESMETA ) )
          parseContext.TurnOffIgnoreMode();

        if( properties.Count == 1 )
        {
          if( properties.Contains( OBJECTREF ) )
          {
            NumberObject objectRef = (NumberObject) properties[ OBJECTREF ];
            properties.Remove( OBJECTREF );
            int refId = (int) objectRef.defaultAdapt();
            reader.Read();
            return parseContext.getReference( refId );
          }
          else if( properties.Contains( STRINGREF ) )
          {
            NumberObject objectRef = (NumberObject) properties[ STRINGREF ];
            properties.Remove( STRINGREF );
            int refId = (int) objectRef.defaultAdapt();
            reader.Read();
            return new StringType( parseContext.getStringReference( refId >> 1 ) );
          }
          else if( properties.Contains( DATEREF ) )
          {
            NumberObject objectRef = (NumberObject) properties[ DATEREF ];
            properties.Remove( DATEREF );
            int refId = (int) objectRef.defaultAdapt();
            reader.Read();
            return parseContext.getReference( refId );
          }
          else if( properties.Contains( ARRAYREF ) )
          {
            NumberObject objectRef = (NumberObject) properties[ ARRAYREF ];
            properties.Remove( ARRAYREF );
            int refId = (int) objectRef.defaultAdapt();
            reader.Read();
            return parseContext.getReference( refId );
          }

          // if we got here, it is not a reference, but an object (anonymous or named)
          // so let's add it to reference cache
          //parseContext.addReference( typeProxy );
        }
      }

      // if properties is empty, it might be an empty dictionary, which may have a pointer to it elsewhere
      if( properties.Count == 0 )
        parseContext.addReference( typeProxy );
      // skip ']'
      reader.Read();

      return new AnonymousObject( properties );
    }
  }
}