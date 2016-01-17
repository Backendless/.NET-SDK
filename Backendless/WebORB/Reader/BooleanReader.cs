using System;
using System.IO;
using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class BooleanReader : ITypeReader
	{
		private bool initialized = false;
		private bool val;

		public BooleanReader()
		{
		}

		public BooleanReader( bool initvalue )
		{
			initialized = true;
			val = initvalue;
		}

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return new BooleanType( initialized ? val : reader.ReadBoolean() );
		}
	}
}
