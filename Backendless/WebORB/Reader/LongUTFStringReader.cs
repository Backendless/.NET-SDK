using System;
using System.IO;
using System.Text;

using Weborb.Types;
using Weborb.Util.IO;

namespace Weborb.Reader
{
  public class LongUTFStringReader : ITypeReader
  {
    public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
    {
      int dataLength = reader.ReadInteger();
      byte[] buffer = reader.ReadBytes( dataLength );
      return new StringType( Encoding.UTF8.GetString( buffer, 0, buffer.Length ) );
    }
  }
}
