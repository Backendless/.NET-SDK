using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;
using Weborb.Util.Logging;

namespace Weborb.Reader
{
	public class NumberReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			double d = reader.ReadDouble();
			return new NumberObject( d );
		}
	}
}
