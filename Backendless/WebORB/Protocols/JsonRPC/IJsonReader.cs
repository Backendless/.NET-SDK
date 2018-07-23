using System;
using System.Collections;
using Weborb.Types;
using Weborb.Reader;

namespace Weborb.Protocols.JsonRPC
{
  public interface IJsonReader
  {
    IAdaptingType read( JsonReader reader, ParseContext parseContext );
  }
}