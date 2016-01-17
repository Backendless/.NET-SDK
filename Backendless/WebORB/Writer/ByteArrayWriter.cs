using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Writer.Amf;

namespace Weborb.Writer
{
    public class ByteArrayWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            ((AmfV3Formatter) formatter).WriteByteArray( (byte[]) obj );
        }

        #endregion
    }
}
