using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public class GuidWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            MessageWriter.writeObject( ((Guid) obj).ToString(), formatter );
        }

        #endregion
    }
}
