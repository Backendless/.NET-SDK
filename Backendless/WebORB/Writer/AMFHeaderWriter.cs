using System;
using System.IO;
using System.Text;
using System.Collections;

using Weborb.Util;
using Weborb.Message;

namespace Weborb.Writer
{
	public class AMFHeaderWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
			Header header = (Header) obj;
            writer.DirectWriteString( header.headerName );
            writer.DirectWriteBoolean( header.mustUnderstand );
            writer.DirectWriteInt( -1 );
			MessageWriter.writeObject( header.headerValue, writer );
		}

		#endregion
	}
}
