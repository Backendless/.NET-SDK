using System;
using System.IO;
using Weborb.Util.Logging;
using Weborb.Exceptions;

namespace Weborb.Writer
{
	public sealed class FlashorbBinaryWriter : BinaryWriter
	{
		public  FlashorbBinaryWriter( Stream stream ) : base( stream )
		{
		}

		public override void Write( short uv )
		{
            //byte[] bytes = BitConverter.GetBytes( uv );
            this.BaseStream.WriteByte( (byte) ((uv >> 8) & 0xFF) );
            this.BaseStream.WriteByte( (byte) ((uv >> 0) & 0xFF) );

            //byte[] bytes = new byte[ 2 ];
            //bytes[ 0 ] = (byte) ((uv >> 8) & 0xFF);
            //bytes[ 1 ] = (byte) ((uv >> 0) & 0xFF);
            //this.BaseStream.Write( bytes, 0, 2 );			
		}

		public void WriteShort( int v )
		{
            //byte[] bytes = new byte[ 2 ];
            //bytes[ 0 ] = (byte) ((v >> 8) & 0xFF);
            //bytes[ 1 ] = (byte) ((v >> 0) & 0xFF);
            //this.BaseStream.Write( bytes, 0, 2 );		
            //byte[] bytes = BitConverter.GetBytes( v );
            //this.BaseStream.WriteByte( bytes[ 1 ] );
            //this.BaseStream.WriteByte( bytes[ 0 ] );
            this.BaseStream.WriteByte( (byte) ((v >> 8) & 0xFF ));
            this.BaseStream.WriteByte( (byte) ((v >> 0) & 0xFF) );
		}

    private void WriteBytes( byte[] bytes )
      {
      byte[] revBytes = new byte[ 4 ];

      for ( int i = 0; i < 4; i++ )
        revBytes[ 3 - i ] = bytes[ i ];

      BaseStream.Write( revBytes, 0, 4 );      
      }

    public void WriteUInt( uint uv )
      {
      byte[] bytes = BitConverter.GetBytes( uv );

      WriteBytes( bytes );

      /*byte[] bytes = new byte[ 4 ];
      bytes[ 0 ] = (byte) ((uv >> 24) & 0xFF);
      bytes[ 1 ] = (byte) ((uv >> 16) & 0xFF);
      bytes[ 2 ] = (byte) ((uv >> 8) & 0xFF);
      bytes[ 3 ] = (byte) ((uv >> 0) & 0xFF);
      this.BaseStream.Write( bytes, 0, 4 );
       */
      }

		public void WriteInt( int uv )
		{
            byte[] bytes = BitConverter.GetBytes( uv );

            WriteBytes( bytes );

            /*byte[] bytes = new byte[ 4 ];
            bytes[ 0 ] = (byte) ((uv >> 24) & 0xFF);
            bytes[ 1 ] = (byte) ((uv >> 16) & 0xFF);
            bytes[ 2 ] = (byte) ((uv >> 8) & 0xFF);
            bytes[ 3 ] = (byte) ((uv >> 0) & 0xFF);
            this.BaseStream.Write( bytes, 0, 4 );
             */ 
		}

        public static byte[] GetVarIntBytes( int v )
        {
            byte[] bytes = null;
            if( v < 128 )
            {
                bytes = new byte[ 1 ];
                bytes[ 0 ] = (byte) v;
            }
            else if( v < 16384 )
            {
                bytes = new byte[ 2 ];
                bytes[ 0 ] = (byte) (v >> 7 & 0x7F | 0x80);
                bytes[ 1 ] = (byte) (v & 0x7F);
            }
            else if( v < 2097152 )
            {
                bytes = new byte[ 3 ];
                bytes[ 0 ] = (byte) (v >> 14 & 0x7F | 0x80);
                bytes[ 1 ] = (byte) (v >> 7 & 0x7F | 0x80);
                bytes[ 2 ] = (byte) (v & 0x7F);
            }
            else if( v < 1073741824 )
            {
                bytes = new byte[ 4 ];
                bytes[ 0 ] = (byte) (v >> 22 & 0x7F | 0x80);
                bytes[ 1 ] = (byte) (v >> 15 & 0x7F | 0x80);
                bytes[ 2 ] = (byte) (v >> 8 & 0x7F | 0x80);
                bytes[ 3 ] = (byte) (v & 0xFF);
            }
            else
            {
                throw new ServiceException( "value out of range - " + v );
            }

            return bytes;
        }

		public void WriteVarInt( int v )
		{
            if( v < 128 )
                BaseStream.WriteByte( (byte) v );
            else if( v < 16384 )
            {
                BaseStream.WriteByte( (byte) (v >> 7 & 0x7F | 0x80) );
                BaseStream.WriteByte( (byte) (v & 0x7F) );
            }
            else
            {
                byte[] bytes = GetVarIntBytes( v );
                BaseStream.Write( bytes, 0, bytes.Length );
            }
		}

		public void WriteDouble( double v )
		{
#if (FULL_BUILD)            
			WriteLong( BitConverter.DoubleToInt64Bits( v ) );
#else
            WriteLong( BitConverter.ToInt64( BitConverter.GetBytes( v ), 0 ) );
#endif
		}

        public void WriteLong( long uv )
        {
            byte[] bytes = BitConverter.GetBytes( uv );
            byte[] revBytes = new byte[ 8 ];

            for( int i = 0; i < 8; i++ )
                revBytes[ 7 - i ] = bytes[ i ];

            BaseStream.Write( revBytes, 0, 8 );


            /*
            byte[] bytes = new byte[ 8 ];
            bytes[ 0 ] = (byte) (uv >> 56);
            bytes[ 1 ] = (byte) (uv >> 48);
            bytes[ 2 ] = (byte) (uv >> 40);
            bytes[ 3 ] = (byte) (uv >> 32);
            bytes[ 4 ] = (byte) (uv >> 24);
            bytes[ 5 ] = (byte) (uv >> 16);
            bytes[ 6 ] = (byte) (uv >> 8);
            bytes[ 7 ] = (byte) (uv >> 0);
            this.BaseStream.Write( bytes, 0, 8 );*/
        }
	}
}
