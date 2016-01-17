using System;
using System.IO;
using System.Text;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class UTFStringReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return new StringType( reader.ReadUTF() );
		}
	}
}
