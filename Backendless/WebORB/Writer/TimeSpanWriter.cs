using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class TimeSpanWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            TimeSpan ts = (TimeSpan) obj;
            MessageWriter.writeObject( ts.TotalMilliseconds, formatter );
        }

        #endregion
    }
}
