using System;
using System.IO;

using Weborb;
using Weborb.Reader;

namespace Weborb.Writer
{
    public class NumberObjectWriter : AbstractUnreferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
        {
            writer.WriteDouble( (double) ((NumberObject) obj).adapt( typeof( double ) ) );
        }

        #endregion
    }
}
