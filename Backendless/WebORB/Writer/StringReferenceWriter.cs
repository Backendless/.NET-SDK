using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class StringReferenceWriter : ITypeWriter
    {
        public void write( object obj, IProtocolFormatter formatter )
        {
            ReferenceCache referenceCache = formatter.GetReferenceCache();
            int refId = referenceCache.GetStringId( obj );

            if( refId != -1 )
            {
                formatter.WriteStringReference( refId );
            }
            else
            {
                referenceCache.AddString( obj );
                formatter.getContextWriter().write( obj, formatter );
            }
        }

        public ITypeWriter getReferenceWriter()
        {
            return null;
        }
    }
}
