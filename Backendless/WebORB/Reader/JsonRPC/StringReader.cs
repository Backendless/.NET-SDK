using System;
using System.Diagnostics;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.JsonRPC
  {
  public sealed class StringReader : IJsonReader
    {
    public IAdaptingType read( JsonReader reader )
      {
      string res = reader.Text;
      reader.Read();
      return new StringType( res );
      }
    }
  }