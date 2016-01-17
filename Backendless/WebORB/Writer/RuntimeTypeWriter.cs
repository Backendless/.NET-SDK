using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class RuntimeTypeWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            Type type = (Type) obj;
            MessageWriter.writeObject( type.FullName, formatter );
        }

        #endregion
    }
}
