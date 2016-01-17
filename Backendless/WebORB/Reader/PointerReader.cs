using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class PointerReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int pointer = reader.ReadUnsignedShort();
			return parseContext.getReference( pointer );
		}
	}
}
