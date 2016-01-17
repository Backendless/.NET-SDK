using System;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class ReaderUtils
	{
		public static string readString( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int len = reader.ReadVarInteger();

			if( (len & 0x1) == 0 )
				return (string) parseContext.getStringReference( len >> 1 );

			string str = reader.ReadUTF( len >> 1 );

			if( str.Length == 0 )
				return str;

			parseContext.addStringReference( str );
			return str;
		}
	}
}
