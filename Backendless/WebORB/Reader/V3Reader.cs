using System;
using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Reader
{
	public class V3Reader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			return RequestParser.readData( reader, parseContext.getVersion() == 3 ? parseContext : parseContext.getCachedContext( 3 ) );
		}

		#endregion
	}
}
