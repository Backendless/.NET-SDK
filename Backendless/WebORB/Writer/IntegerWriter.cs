using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class IntegerWriter : AbstractUnreferenceableTypeWriter
    {
        public override void write( object obj, IProtocolFormatter writer )
        {
            writer.WriteInteger( Convert.ToInt32(obj) );
        }
    }
}
