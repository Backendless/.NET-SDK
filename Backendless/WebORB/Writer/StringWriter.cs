using System;
using System.IO;
using System.Text;
using Weborb;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
    public class StringWriter : ITypeWriter
    {
        private bool isReferenceable = false;
        private ITypeWriter referenceWriter = new StringReferenceWriter();

        public StringWriter( bool isReferenceable )
        {
            this.isReferenceable = isReferenceable;
        }

        #region ITypeWriter Members

        public void write( object obj, IProtocolFormatter writer )
        {
            int refId = -1;
            ReferenceCache referenceCache = null;

            if( isReferenceable )
            {
                referenceCache = writer.GetReferenceCache();
                refId = referenceCache.GetStringId( obj );
            }

            if( refId != -1 )
            {
                writer.WriteStringReference( refId );
            }
            else
            {
                if( isReferenceable )
                    referenceCache.AddString( obj );

                //formatter.getContextWriter().write( obj, formatter );
                if( obj is string )
                    writer.WriteString( (string) obj );
                else if( obj is StringBuilder )
                    writer.WriteString( ((StringBuilder) obj).ToString() );
                else if( obj is Char )
                    writer.WriteString( obj.ToString() );
                else if( obj is char[] )
                    writer.WriteString( new String( (char[]) obj ) );
            }
        }

        public ITypeWriter getReferenceWriter()
        {
            //return isReferenceable ? referenceWriter : null;
            return null;
        }

        #endregion
    }
}
