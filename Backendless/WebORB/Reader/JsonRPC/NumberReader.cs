using System;
using System.Globalization;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Reader.JsonRPC
{
  public class NumberReader : IJsonReader
  {
    public IAdaptingType read( JsonReader reader, ParseContext parseContext )
    {
      IAdaptingType val;
      string value = reader.Text.Trim();

      if( value.Contains( "." ) )
        val = new JsonNumberObject( double.Parse( value ) );
      else
        val = new JsonNumberObject( long.Parse( value ) );
      // move to next token
      reader.Read();
      return val;
    }
  }
}