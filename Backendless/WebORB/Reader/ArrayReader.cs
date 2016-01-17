using System;
using System.IO;

using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class ArrayReader : ITypeReader
	{
		public ArrayReader()
		{
		}

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int length = reader.ReadInteger();
			IAdaptingType[] array = new IAdaptingType[ length ];
			ArrayType arrayType = new ArrayType( array );
			parseContext.addReference( arrayType );

			for( int i = 0; i < length; i++ )
				array[ i ] = RequestParser.readData( reader, parseContext );

			return arrayType;
		}
	}
}
