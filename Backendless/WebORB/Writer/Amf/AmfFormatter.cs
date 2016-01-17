using System;
using System.IO;
using System.Xml;
using System.Collections;
using Weborb;
using Weborb.Writer;
using Weborb.Util;
using Weborb.Message;

namespace Weborb.Writer.Amf
  {
  public class AmfFormatter : IProtocolFormatter
    {
    private FlashorbBinaryWriter writer;
    private ObjectSerializer objectSerializer;
    private ReferenceCache referenceCache;

    protected MemoryStream stream;

    // this is used for extraction of the body for caching purpuses
    protected long beginSelectBytesIndex;

    public AmfFormatter()
      {
      stream = new MemoryStream();
      writer = new FlashorbBinaryWriter( stream );
      objectSerializer = new ObjectSerializer();
      referenceCache = new ReferenceCache();
      }

    #region IProtocolFormatter Members

    internal override void BeginSelectCacheObject()
      {
      beginSelectBytesIndex = stream.Length;
      }

    internal override object EndSelectCacheObject()
      {
      if ( stream.Length < beginSelectBytesIndex )
        return new byte[ 0 ];

      byte[] res = new byte[ stream.Length - beginSelectBytesIndex ];
      stream.Position = (int)beginSelectBytesIndex;
      stream.Read( res, 0, res.Length );
      return res;
      }

    internal override void WriteCachedObject( object cached )
      {
      writer.Write( (byte[])cached );
      }

    public override ITypeWriter getWriter( Type type )
      {
      return null;
      }

    public override ReferenceCache GetReferenceCache()
      {
      return referenceCache;
      }

    public override void ResetReferenceCache()
      {
      referenceCache.Reset();
      }

    public override void DirectWriteBytes( byte[] b )
      {
      writer.Write( b );
      }

    public override void DirectWriteString( string str )
      {
      UTF8Util.writeUTF( writer, str );
      }

    public override void DirectWriteInt( int i )
      {
      writer.Write( i );
      }

    public override void DirectWriteBoolean( bool b )
      {
      writer.Write( b );
      }

    public override void DirectWriteShort( int s )
      {
      writer.WriteShort( s );
      }

    public override void WriteMessageVersion( float version )
      {
      writer.WriteShort( (int)version );
      }

    public override void BeginWriteArray( int length )
      {
      writer.Write( (byte)Datatypes.ARRAY_DATATYPE_V1 );
      writer.WriteInt( length );
      }

    public override void EndWriteArray()
      {
      }

    public override void WriteBoolean( bool b )
      {
      writer.Write( (byte)Datatypes.BOOLEAN_DATATYPE_V1 );
      writer.Write( b );
      }

    public override void WriteDate( DateTime datetime )
      {
      writer.Write( (byte)Datatypes.DATE_DATATYPE_V1 );
      DateTime olddate = new DateTime( 1970, 1, 1 );

#if( FULL_BUILD || PURE_CLIENT_LIB)
      // get the offset of the time zone the server is in
      double localTimezoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset( olddate ).TotalMilliseconds;

      // convert 1/1/1970 12AM to UTC
      olddate = TimeZone.CurrentTimeZone.ToUniversalTime( olddate );
#else
            // get the offset of the time zone the server is in
            double localTimezoneOffset = TimeZoneInfo.Local.GetUtcOffset( olddate ).TotalMilliseconds;

            // convert 1/1/1970 12AM to UTC
            olddate = TimeZoneInfo.ConvertTime( olddate, TimeZoneInfo.Utc );
#endif
      // bring it back to 12AM
      olddate = olddate.AddMilliseconds( localTimezoneOffset );

      datetime = datetime.ToUniversalTime();
      TimeSpan span = datetime.Subtract( olddate );
      long totalMs = ( (long)span.TotalMilliseconds );
      writer.WriteDouble( totalMs );

      writer.WriteShort( (short)0 );
      }

    public override void BeginWriteObjectMap( int size )
      {
      writer.Write( (byte)Datatypes.OBJECTARRAY_DATATYPE_V1 );
      writer.WriteInt( size );
      }

    public override void EndWriteObjectMap()
      {
      writer.Write( (byte)0 );
      writer.Write( (byte)0 );
      writer.Write( (byte)Datatypes.ENDOFOBJECT_DATATYPE_V1 );
      }

    public override void WriteFieldName( string s )
      {
      UTF8Util.writeUTF( writer, s );
      }

    public override void WriteNull()
      {
      writer.Write( (byte)Datatypes.NULL_DATATYPE_V1 );
      }

    public override void WriteInteger( int number )
      {
      writer.Write( (byte)Datatypes.NUMBER_DATATYPE_V1 );
      writer.WriteDouble( number );
      }

    public override void WriteDouble( double number )
      {
      writer.Write( (byte)Datatypes.NUMBER_DATATYPE_V1 );
      writer.WriteDouble( number );
      }

    public override void BeginWriteNamedObject( string objectName, int fieldCound )
      {
      writer.Write( (byte)Datatypes.NAMEDOBJECT_DATATYPE_V1 );
      UTF8Util.writeUTF( writer, objectName );
      }

    public override void EndWriteNamedObject()
      {
      writer.Write( (byte)0 );
      writer.Write( (byte)0 );
      writer.Write( (byte)Datatypes.ENDOFOBJECT_DATATYPE_V1 );
      }

    public override void BeginWriteObject( int fieldCound )
      {
      writer.Write( (byte)Datatypes.OBJECT_DATATYPE_V1 );
      }

    public override void EndWriteObject()
      {
      writer.Write( (byte)0 );
      writer.Write( (byte)0 );
      writer.Write( (byte)Datatypes.ENDOFOBJECT_DATATYPE_V1 );
      }

    public override void WriteDateReference( int refID )
      {
      writer.Write( (byte)Datatypes.POINTER_DATATYPE_V1 );
      writer.WriteShort( (ushort)refID );
      }

    public override void WriteArrayReference( int refID )
      {
      writer.Write( (byte)Datatypes.POINTER_DATATYPE_V1 );
      writer.WriteShort( (ushort)refID );
      }

    public override void WriteByteArray( byte[] array )
    {
      writer.Write( (byte)Datatypes.V3_DATATYPE );
      writer.Write( (byte)Datatypes.BYTEARRAY_DATATYPE_V3 );
      writer.WriteVarInt( array.Length << 1 | 0x1 );
      writer.Write( array );
    }

    public override void WriteObjectReference( int refID )
      {
      writer.Write( (byte)Datatypes.POINTER_DATATYPE_V1 );
      writer.WriteShort( (ushort)refID );
      }

    public override void WriteStringReference( int refID )
      {
      writer.Write( (byte)Datatypes.POINTER_DATATYPE_V1 );
      writer.WriteShort( (ushort)refID );
      }

    public override void WriteString( string s )
      {
      writer.Write( getStringBytes( s ) );
      }

#if (FULL_BUILD)
    public override void WriteXML( XmlNode document )
      {
      writer.Write( (byte)Datatypes.PARSEDXML_DATATYPE_V1 );
      writer.Write( getEncoded( document.OuterXml ) );
      }
#endif

    public override ProtocolBytes GetBytes()
      {
      ProtocolBytes protocolBytes = new ProtocolBytes();
      protocolBytes.length = (int)stream.Length;
      protocolBytes.bytes = stream.GetBuffer();
      stream.Close();
      writer.Close();
      return protocolBytes;
      }

    public override void Cleanup()
      {
      writer.Close();
      }

    public override string GetContentType()
      {
      return "application/x-amf";
      }

    public override IObjectSerializer GetObjectSerializer()
      {
      return objectSerializer;
      }

    #endregion

    private byte[] getEncoded( string str )
      {
      int strlen = str.Length;
      uint utflen = 0;
      char[] charr = str.ToCharArray();
      int count = 0;

      for ( int i = 0; i < strlen; i++ )
        {
        int c = charr[ i ];
        if ( c >= 1 && c <= 127 )
          utflen++;
        else if ( c > 2047 )
          utflen += 3;
        else
          utflen += 2;
        }

      byte[] bytearr = new byte[ utflen + 4 ];
      bytearr[ count++ ] = (byte)( utflen >> 24 & 0xff );
      bytearr[ count++ ] = (byte)( utflen >> 16 & 0xff );
      bytearr[ count++ ] = (byte)( utflen >> 8 & 0xff );
      bytearr[ count++ ] = (byte)( utflen >> 0 & 0xff );

      for ( int i = 0; i < strlen; i++ )
        {
        int c = charr[ i ];
        if ( c >= 1 && c <= 127 )
          {
          bytearr[ count++ ] = (byte)c;
          }
        else if ( c > 2047 )
          {
          bytearr[ count++ ] = (byte)( 0xe0 | c >> 12 & 0xf );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 6 & 0x3f );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 0 & 0x3f );
          }
        else
          {
          bytearr[ count++ ] = (byte)( 0xc0 | c >> 6 & 0x1f );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 0 & 0x3f );
          }
        }

      return bytearr;
      }

    public byte[] getStringBytes( String str )
      {
      //TODO: this needs to be fixed and cleaned up
      if ( str.Equals( "System.Int32" ) )
        str = "int";

      int strlen = str.Length;
      uint utflen = 0;
      char[] charr = str.ToCharArray();
      int count = 0;

      for ( int i = 0; i < strlen; i++ )
        {
        int c = charr[ i ];

        if ( c >= 1 && c <= 127 )
          utflen++;
        else if ( c > 2047 )
          utflen += 3;
        else
          utflen += 2;
        }

      byte[] bytearr = new byte[ utflen + ( utflen <= 65535 ? 3 : 5 ) ];
      bytearr[ count++ ] = utflen <= 65535 ? (byte)2 : (byte)12;

      if ( utflen > 65535 )
        {
        bytearr[ count++ ] = (byte)( utflen >> 24 & 0xff );
        bytearr[ count++ ] = (byte)( utflen >> 16 & 0xff );
        }

      bytearr[ count++ ] = (byte)( utflen >> 8 & 0xff );
      bytearr[ count++ ] = (byte)( utflen >> 0 & 0xff );

      for ( int i = 0; i < strlen; i++ )
        {
        int c = charr[ i ];

        if ( c >= 1 && c <= 127 )
          {
          bytearr[ count++ ] = (byte)c;
          }
        else if ( c > 2047 )
          {
          bytearr[ count++ ] = (byte)( 0xe0 | c >> 12 & 0xf );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 6 & 0x3f );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 0 & 0x3f );
          }
        else
          {
          bytearr[ count++ ] = (byte)( 0xc0 | c >> 6 & 0x1f );
          bytearr[ count++ ] = (byte)( 0x80 | c >> 0 & 0x3f );
          }
        }

      return bytearr;
      }
    }
  }
