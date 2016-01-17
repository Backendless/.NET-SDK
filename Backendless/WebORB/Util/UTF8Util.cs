using System;
using System.IO;
using System.Text;
using Weborb.Util.Logging;
using Weborb.Writer;

namespace Weborb.Util
{
	public class UTF8Util
	{

		public static void writeUTF( FlashorbBinaryWriter writer, string content )
		{
			writeUTF( writer, content, false );
		}

		public static void writeUTF( FlashorbBinaryWriter writer, string content, bool extendedutf )
		{
            //int utfLength = UTF8Encoding.UTF8.GetByteCount( content );
            byte[] buffer = UTF8Encoding.UTF8.GetBytes( content );

            if( extendedutf )
            {
                writer.WriteVarInt( (int) (buffer.Length << 1 | 0x1) );
            }
            else
            {
                writer.Write( (byte) (buffer.Length >> 8 & 0xFF) );
                writer.Write( (byte) (buffer.Length >> 0 & 0xFF) );
            }

            

            if( buffer.Length > 0 )
                writer.Write( buffer );
            /*
			//dervived from java class DataOutputStream.writeUTF
			int strLength = content.Length;
			uint utfLength = 0;
			char[] charr = content.ToCharArray();
			int c, count = 0;
 
			for (int i = 0; i < strLength; i++) 
			{
				c = charr[ i ];

				if( (c >= 0x0001) && (c <= 0x007F) )
					utfLength++;
				else if( c > 0x07FF )
					utfLength += 3;
				else
					utfLength += 2;	
			}

			if( !extendedutf && utfLength > 65535 )
				throw new ApplicationException( "utf data format exception" );

			byte[] bytearr = new byte[ extendedutf ? utfLength : utfLength + 2 ];

			if( extendedutf )
			{
				writer.WriteVarInt( (int) (utfLength << 1 | 0x1) );
			}
			else
			{
				bytearr[ count++ ] = (byte) ( utfLength >> 8 & 0xFF );
				bytearr[ count++ ] = (byte) ( utfLength >> 0 & 0xFF );
			}

			for (int i = 0; i < strLength; i++) 
			{
				c = charr[ i ];

				if ( (c >= 0x0001) && (c <= 0x007F) )
				{
					bytearr[ count++ ] = (byte) c;
				}
				else if (c > 0x07FF)
				{
					bytearr[ count++ ] = (byte) (0xE0 | c >> 12 & 0x0F);
					bytearr[ count++ ] = (byte) (0x80 | c >>  6 & 0x3F);
					bytearr[ count++ ] = (byte) (0x80 | c >>  0 & 0x3F);
				}
				else 
				{
					bytearr[ count++ ] = (byte) (0xC0 | c >>  6 & 0x1F);
					bytearr[ count++ ] = (byte) (0x80 | c >>  0 & 0x3F);
				}
			}
        
			writer.Write( bytearr );
             * */
		}
	}
}
