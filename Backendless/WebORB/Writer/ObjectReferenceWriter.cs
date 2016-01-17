using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
    class ObjectReferenceWriter : ITypeWriter
    {
        public void write( object obj, IProtocolFormatter formatter )
        {
            ReferenceCache referenceCache = formatter.GetReferenceCache();
            int refId = referenceCache.GetObjectId( obj );

            if( refId != -1 )
            {
                formatter.WriteObjectReference( refId );
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
