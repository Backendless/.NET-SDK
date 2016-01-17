using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public abstract class AbstractUnreferenceableTypeWriter : ITypeWriter
    {
        #region ITypeWriter Members

        public virtual ITypeWriter getReferenceWriter()
        {
            return null;
        }

        public abstract void write( object obj, IProtocolFormatter writer );

        #endregion
    }
}
