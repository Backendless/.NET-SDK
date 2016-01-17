using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int dataLength = reader.ReadInt32();
			byte[] buffer = reader.ReadBytes( dataLength );
			MemoryStream stream = new MemoryStream( buffer );
			System.Xml.XmlDocument document = new System.Xml.XmlDocument();
			document.Load( stream );
			return new XmlType( document );
		}
	}
}
