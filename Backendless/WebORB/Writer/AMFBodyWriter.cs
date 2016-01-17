using System;
using System.IO;
using System.Text;
using System.Collections;

using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Message;

namespace Weborb.Writer
{
	public class AMFBodyWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write( object obj, IProtocolFormatter writer )
		{
			//Log.log( ORBConstants.INFO, "AMFBodyWriter.write.begin: " + writer.BaseStream.Length );
			Body body = (Body) obj;

			if( Log.isLogging( LoggingConstants.DEBUG ) )
				Log.log( LoggingConstants.DEBUG, "AMFBodyWriter:write body.responseURI: " + body.responseURI + " body.serviceURI: " + body.serviceURI );

            writer.DirectWriteString( body.ResponseUri == null ? "null" : body.ResponseUri );
            writer.DirectWriteString( body.ServiceUri == null ? "null" : body.ServiceUri );
            writer.DirectWriteInt( -1 );
			//((FlashorbBinaryWriter)writer).WriteInt( i );
			//Log.log( ORBConstants.INFO, "AMFBodyWriter.write.before writing response object: " + writer.BaseStream.Length );
			writer.ResetReferenceCache();
			writer.BeginWriteBodyContent();
			MessageWriter.writeObject( body.responseDataObject, writer );
			writer.EndWriteBodyContent();
			//Log.log( ORBConstants.INFO, "AMFBodyWriter.write.end: " + writer.BaseStream.Length );
		}

		#endregion
	}
}
