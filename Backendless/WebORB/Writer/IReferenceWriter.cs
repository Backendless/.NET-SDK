using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public interface IReferenceWriter
    {
        void write( object obj, ITypeWriter unrerencedObjectWriter, IProtocolFormatter formatter );
    }
}
