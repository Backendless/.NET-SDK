using System;
using System.Collections;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
  {
  public class ObjectReader : IJsonReader
    {
    public IAdaptingType read( JsonReader reader )
      {
      IDictionary properties = new Hashtable();

      // skip '[' and move to next element
      reader.Read();

      while ( reader.TokenClass != JsonTokenClass.EndObject )
        {
        string name = reader.ReadMember();

        properties.Add( name, RequestParser.Read( reader ) );
        }

      // skip ']'
      reader.Read();

      return new AnonymousObject( properties );
      }   
    }
  }