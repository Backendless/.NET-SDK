using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
  {
  public sealed class ArrayReader : IJsonReader
    {
    public IAdaptingType read( JsonReader reader )
      {
      // skip '{' and move to first element
      reader.Read();

      ArrayList list = new ArrayList();

      while ( reader.TokenClass != JsonTokenClass.EndArray )        
        list.Add( RequestParser.Read( reader ) );        

      // skip '}'
      reader.Read();      

      return new ArrayType( list.ToArray() );
      }
    }
  }