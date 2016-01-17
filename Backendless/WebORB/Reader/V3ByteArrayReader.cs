using System;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class V3ByteArrayReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int refId = reader.ReadVarInteger();

			if( (refId & 0x1) == 0 )
				return (ArrayType) parseContext.getReference( refId >> 1 );

			byte[] bytes = reader.ReadBytes( refId >> 1 );
            IAdaptingType[] objArray = new IAdaptingType[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
                objArray[i] = new NumberObject( bytes[i] );

			ArrayType arrayType = new ArrayType( objArray );
			parseContext.addReference( arrayType );
			return arrayType;
		}

		#endregion
	}
}
