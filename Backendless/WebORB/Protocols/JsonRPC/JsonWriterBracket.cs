using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Weborb.Protocols.JsonRPC
{
    [ Serializable ]
    public enum JsonWriterBracket 
    {
        Pending,
        Array,
        Object,
        Member,
        Closed
    };
}