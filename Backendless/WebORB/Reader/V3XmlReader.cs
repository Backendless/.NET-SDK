using System;
using System.IO;
using System.Xml;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class V3XmlReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int len = reader.ReadVarInteger();

			if( (len & 0x1) == 0 )
				return (XmlType) parseContext.getReference( len >> 1 );

			len = len >> 1;

            XmlType xmlType = null;

            if( len == 0 )
            {
                xmlType = ParseString( "" );
            }
            else
            {
                string xmlStr = reader.ReadUTF( len );
                xmlType = ParseString( xmlStr );
            }

			parseContext.addReference( xmlType );
			return xmlType;
		}

		#endregion

		private XmlType ParseString( string xmlStr )
		{
			System.Xml.XmlDocument document = new System.Xml.XmlDocument();

			try
			{
				document.LoadXml( xmlStr );
			}
			catch( Exception )
			{
			}

			return new XmlType( document );
		}
	}
}
