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
    public IAdaptingType read( JsonReader reader )
      {
      IAdaptingType val = new NumberObject( double.Parse( reader.Text.Trim() ) );      
      // move to next token
      reader.Read();
      return val;
      }
    }    
}