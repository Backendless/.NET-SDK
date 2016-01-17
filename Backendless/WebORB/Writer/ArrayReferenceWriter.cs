using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class ArrayReferenceWriter : ITypeWriter
    {
        public void write( object obj, IProtocolFormatter formatter )
        {
            ReferenceCache referenceCache = formatter.GetReferenceCache();
            int refId = referenceCache.GetObjectId( obj );

            if( refId != -1 )
            {
                formatter.WriteArrayReference( refId );
            }
            else
            {
                referenceCache.AddObject( obj );
                formatter.getContextWriter().write( obj, formatter );
            }
        }

        public ITypeWriter getReferenceWriter()
        {
            return null;
        }
    }
}
