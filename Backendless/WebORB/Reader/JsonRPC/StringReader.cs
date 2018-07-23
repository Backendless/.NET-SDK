using System;
using System.Diagnostics;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
{
  public sealed class StringReader : IJsonReader
  {
    public IAdaptingType read( JsonReader reader, ParseContext parseContext )
    {
      string res = reader.Text;
      reader.Read();

      if( res.Length > 0 )
        parseContext.addStringReference( res );

      return new StringType( res );
    }
  }
}