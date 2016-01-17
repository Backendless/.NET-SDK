using System;
using System.IO;
using System.Xml;
using Weborb;

namespace Weborb.Writer
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write( object obj, IProtocolFormatter writer )
		{
			XmlNode document = (XmlNode ) obj;
            writer.WriteXML( document );
		}

		private byte[] getEncoded( string str )
		{
			int strlen = str.Length;
			uint utflen = 0;
			char[] charr = str.ToCharArray();
			int count = 0;
			
			for( int i = 0; i < strlen; i++ )
			{
				int c = charr[ i ];
				if( c >= 1 && c <= 127 )
					utflen++;
				else if( c > 2047 )
					utflen += 3;
				else
					utflen += 2;
			}

			byte[] bytearr = new byte[ utflen + 4 ];
			bytearr[ count++ ] = (byte) ( utflen >> 24 & 0xff );
			bytearr[ count++ ] = (byte) ( utflen >> 16 & 0xff );
			bytearr[ count++ ] = (byte) ( utflen >> 8 & 0xff );
			bytearr[ count++ ] = (byte) ( utflen >> 0 & 0xff );

			for( int i = 0; i < strlen; i++ )
			{
				int c = charr[ i ];
				if( c >= 1 && c <= 127 )
				{
					bytearr[ count++ ] = (byte) c;
				}
				else if( c > 2047 )
				{
					bytearr[ count++ ] = (byte) ( 0xe0 | c >> 12 & 0xf );
					bytearr[ count++ ] = (byte) ( 0x80 | c >> 6 & 0x3f );
					bytearr[ count++ ] = (byte) ( 0x80 | c >> 0 & 0x3f );
				}
				else
				{
					bytearr[ count++ ] = (byte) ( 0xc0 | c >> 6 & 0x1f );
					bytearr[ count++ ] = (byte) ( 0x80 | c >> 0 & 0x3f );
				}
			}

			return bytearr;
		}

		#endregion
	}
}
