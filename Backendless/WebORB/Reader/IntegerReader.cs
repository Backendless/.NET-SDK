using System;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class IntegerReader : ITypeReader
	{
		#region ITypeReader Members

		public Weborb.Types.IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return new NumberObject( (reader.ReadVarInteger() << 3) >> 3 );
		}

		#endregion
	}
}
