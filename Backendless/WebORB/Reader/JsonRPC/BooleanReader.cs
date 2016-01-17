using System;
using System.Diagnostics;
using System.Globalization;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
  {
  public class BooleanReader : IJsonReader
    {
    public IAdaptingType read( JsonReader reader )
      {
      IAdaptingType val = new BooleanType( reader.Token.Text.Trim().ToLower().Equals( "true" ) );
      // move to next token
      reader.Read();
      return val;
      }
    }
  }