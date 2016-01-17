using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    class DateReferenceWriter : ITypeWriter
    {
        public virtual void write( object obj, IProtocolFormatter formatter )
        {
            ReferenceCache referenceCache = formatter.GetReferenceCache();
            DateTime date = (DateTime) obj;
            int refId = referenceCache.GetObjectId( date.ToUniversalTime() );

            if( refId != -1 )
            {
                formatter.WriteDateReference( refId );
            }
            else
            {
                referenceCache.AddObject( date.ToUniversalTime() );
                formatter.getContextWriter().write( obj, formatter );
            }
        }

        public ITypeWriter getReferenceWriter()
        {
            return null;
        }
    }
}
