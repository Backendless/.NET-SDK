using System;
using System.IO;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public interface ITypeReader
	{
		IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext );
	}
}
