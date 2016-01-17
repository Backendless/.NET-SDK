using System;
using System.IO;
using System.Text;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class NamedObjectReader : ITypeReader
	{
		private ITypeReader objectReader = new AnonymousObjectReader();

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			string objectName = reader.ReadUTF();
			return new NamedObject( objectName, objectReader.read( reader, parseContext ) );
		}
	}
}
