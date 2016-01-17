using System;
using System.IO;
using System.Text;

using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	/// <summary>
	/// 
	/// </summary>
	public class RemoteReferenceReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			string objectName = reader.ReadUTF();
			object reference = RequestParser.readData( reader, parseContext );

			if( reference == null )
				return null;

			string hostingEnvironmentID = reader.ReadUTF();
			return new RemoteReferenceObject( reference );
		}
	}
}
