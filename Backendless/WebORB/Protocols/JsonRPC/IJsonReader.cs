using System;
using System.Collections;
using Weborb.Types;

namespace Weborb.Protocols.JsonRPC
  {
  public interface IJsonReader
    {    
    IAdaptingType read( JsonReader reader );
    }
  }