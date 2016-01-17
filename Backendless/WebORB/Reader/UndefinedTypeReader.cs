using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class UndefinedTypeReader : ITypeReader
	{
		private static IAdaptingType undefinedType = new UndefinedType();

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return undefinedType;
		}
	}
}
