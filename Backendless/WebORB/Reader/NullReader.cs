using System;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class NullReader : ITypeReader
	{
		#region ITypeReader Members

		public Weborb.Types.IAdaptingType read(FlashorbBinaryReader reader, ParseContext parseContext)
		{
			return new NullType();
		}

		#endregion
	}
}
