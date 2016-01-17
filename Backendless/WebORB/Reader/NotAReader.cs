using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class NotAReader : ITypeReader
	{
		public NotAReader()
		{
		}

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return null;
		}
	}
}
