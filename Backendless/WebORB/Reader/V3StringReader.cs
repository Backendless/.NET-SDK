using System;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class V3StringReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return new StringType( ReaderUtils.readString( reader, parseContext ) );
		}

		#endregion
	}
}
