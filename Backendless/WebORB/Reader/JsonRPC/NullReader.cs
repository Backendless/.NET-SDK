using System;
using System.Globalization;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Reader.JsonRPC
{
    public class NullReader : IJsonReader
    {
        public IAdaptingType read(JsonReader reader)
        {
            reader.Read();
            return new NullType();
        }
    }
}