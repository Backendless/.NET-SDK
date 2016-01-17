using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public abstract class AbstractReferenceableTypeWriter : ITypeWriter
    {
        private ITypeWriter referenceWriter = new ObjectReferenceWriter();

        #region ITypeWriter Members

        public virtual ITypeWriter getReferenceWriter()
        {
            return referenceWriter;
        }

        public abstract void write( object obj, IProtocolFormatter writer );

        #endregion
    }
}
