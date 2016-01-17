using System;
using System.IO;
using System.Text;
using Weborb.Util.Logging;

namespace Weborb.Util.IO
{
	public class FlashorbBinaryReader : BinaryReader
	{
		private UTF8Encoding encoding;

		public FlashorbBinaryReader( Stream input ) : base( input )
		{
			encoding = new UTF8Encoding();
		}

		public int ReadVarInteger()
		{
			int num = this.BaseStream.ReadByte() & 0xFF;

			if( num < 128 )
				return num;

			int val = (num & 0x7F) << 7;
			num = this.BaseStream.ReadByte() & 0xFF;

			if( num < 128 )
				return val | num;

			val = (val | num & 0x7F) << 7;
			num = this.BaseStream.ReadByte() & 0xFF;

			if( num < 128 )
				return val | num;

			val = (val | num & 0x7F) << 8;
			num = this.BaseStream.ReadByte() & 0xFF;
			return val | num;
		}

		public int ReadUnsignedShort()
		{
			int byte1 = this.BaseStream.ReadByte();
			int byte2 = this.BaseStream.ReadByte();
			return (byte1 << 8) + (byte2 << 0);
		}

    public uint ReadUInteger()
      {
      uint byte1 = (uint)this.BaseStream.ReadByte();
      uint byte2 = (uint)this.BaseStream.ReadByte();
      uint byte3 = (uint)this.BaseStream.ReadByte();
      uint byte4 = (uint)this.BaseStream.ReadByte();
      return ( ( byte1 << 24 ) + ( byte2 << 16 ) + ( byte3 << 8 ) + ( byte4 << 0 ) );
      }

		public int ReadInteger()
		{
			int byte1 = this.BaseStream.ReadByte();
			int byte2 = this.BaseStream.ReadByte();
			int byte3 = this.BaseStream.ReadByte();
			int byte4 = this.BaseStream.ReadByte();
			return ((byte1 << 24) + (byte2 << 16) + (byte3 << 8) + (byte4 << 0));
		}

		public string ReadUTF()
		{
			int length = ReadUnsignedShort();
            byte[] bytes = ReadBytes( length );
			return encoding.GetString( bytes, 0, bytes.Length  );
		}

	  public string ReadUTF( int len )
		{
            if( len == 0 )
                return string.Empty;

            UTF8Encoding utf8 = new UTF8Encoding( false, true );
            byte[] encodedBytes = this.ReadBytes( len );
            string decodedString = utf8.GetString( encodedBytes, 0, encodedBytes.Length );
            return decodedString;
		}

		public long ReadLong()
		{
			int byte1 = this.BaseStream.ReadByte();
			int byte2 = this.BaseStream.ReadByte();
			int byte3 = this.BaseStream.ReadByte();
			int byte4 = this.BaseStream.ReadByte();
			int byte5 = this.BaseStream.ReadByte();
			int byte6 = this.BaseStream.ReadByte();
			int byte7 = this.BaseStream.ReadByte();
			int byte8 = this.BaseStream.ReadByte();

			return ((long)byte1 << 56) +
                ((long)(byte2 & 255) << 48) +
                ((long)(byte3 & 255) << 40) +
                ((long)(byte4 & 255) << 32) +
                ((long)(byte5 & 255) << 24) +
                ((byte6 & 255) << 16) +
                ((byte7 & 255) <<  8) +
                ((byte8 & 255) <<  0);
		}

		public override double ReadDouble()
		{
#if (FULL_BUILD)
			return BitConverter.Int64BitsToDouble( ReadLong() );
#else
            return BitConverter.ToDouble( BitConverter.GetBytes( ReadLong() ), 0 );
#endif
		}

		public int ReadInt16( FlashorbBinaryReader reader )
		{
			int byte1 = this.BaseStream.ReadByte();
			int byte2 = this.BaseStream.ReadByte();
			return ((byte2 << 8) + byte2);
		}
	}
}
