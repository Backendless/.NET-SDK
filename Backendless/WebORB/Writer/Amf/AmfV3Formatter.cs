using System;
using System.IO;
using System.Xml;
#if (FULL_BUILD)
using System.Data;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb;
using Weborb.Writer;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Message;
using Weborb.V3Types;

namespace Weborb.Writer.Amf
  {
  public sealed class AmfV3Formatter : IProtocolFormatter
    {
    private static Dictionary<Type, ITypeWriter> writers = new Dictionary<Type, ITypeWriter>();
    private FlashorbBinaryWriter writer;
    private IObjectSerializer objectSerializer;
    private V3ReferenceCache referenceCache;
    private Encoding utf8 = UTF8Encoding.UTF8;
    private MemoryStream stream;

    // this is used for extraction of the body for caching purpuses
    private long beginSelectBytesIndex;

    static AmfV3Formatter()
      {
#if (FULL_BUILD)
      writers[ typeof( DataSet ) ] = new DataSetAsArrayWriter();
      writers[ typeof( DataTable ) ] = new DataTableAsListWriter();
#endif
      writers[ typeof( DateTime ) ] = new DateWriter( true );
      writers[ typeof( DateTimeOffset ) ] = new DateTimeOffsetWriter( true );
      writers[ typeof( byte[] ) ] = new ByteArrayWriter();
      writers[ typeof( BodyHolder ) ] = new BodyHolderWriter();

      StringWriter stringWriter = new StringWriter( true );
      writers[ typeof( string ) ] = stringWriter;
      writers[ typeof( char[] ) ] = stringWriter;
      writers[ typeof( Char ) ] = stringWriter;
      writers[ typeof( StringBuilder ) ] = stringWriter;
      }

    public static void AddTypeWriter( Type mappedType, ITypeWriter writer )
      {
      //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
      //    throw new Exception( "cannot register custom serializers, this feature is available in WebORB Professional Edition" );

      writers[ mappedType ] = writer;
      }

    public AmfV3Formatter()
      {
      stream = new MemoryStream();
      writer = new FlashorbBinaryWriter( stream );
      objectSerializer = new V3ObjectSerializer();
      referenceCache = new V3ReferenceCache();
      }

    public override void WriteByteArray( byte[] array )
      {
      writer.Write( (byte)Datatypes.BYTEARRAY_DATATYPE_V3 );
      writer.WriteVarInt( array.Length << 1 | 0x1 );
      writer.Write( array );
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
      ITypeWriter typeWriter;
      writers.TryGetValue( type, out typeWriter );
      return typeWriter;
      }

    public override ReferenceCache GetReferenceCache()
      {
      return referenceCache;
      }

    public override void ResetReferenceCache()
      {
      referenceCache.Reset();
      }

    public override void DirectWriteString( string str )
      {
      UTF8Util.writeUTF( writer, str );
      }

    public override void DirectWriteBytes( byte[] b )
      {
      writer.Write( b );
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

    public override void BeginWriteBodyContent()
      {
      writer.Write( (byte)Datatypes.V3_DATATYPE );
      }

    public override void BeginWriteArray( int length )
      {
      writer.Write( (byte)Datatypes.ARRAY_DATATYPE_V3 );
      writer.WriteVarInt( length << 1 | 0x1 );
      writer.WriteVarInt( 0x1 );
      }

    public override void WriteBoolean( bool b )
      {
      if ( b )
        writer.Write( (byte)Datatypes.BOOLEAN_DATATYPE_TRUEV3 );
      else
        writer.Write( (byte)Datatypes.BOOLEAN_DATATYPE_FALSEV3 );
      }

    public override void WriteDate( DateTime datetime )
      {
      writer.Write( (byte)Datatypes.DATE_DATATYPE_V3 );
      writer.WriteVarInt( 0x1 );
#if( WINDOWS_PHONE8 )
       TimeSpan timeZoneOffset = TimeZoneInfo.Local.BaseUtcOffset;
#elif (FULL_BUILD || PURE_CLIENT_LIB)
      TimeSpan timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset( datetime );
#else
      TimeSpan timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset( datetime );
#endif
      DateTime olddate = new DateTime( 1970, 1, 1 );//.ToUniversalTime();
      TimeSpan span = datetime.Subtract( olddate );
      span = span.Subtract( timeZoneOffset );
      long totalMs = ( (long)span.TotalMilliseconds );
      writer.WriteDouble( totalMs );
      }

    public override void BeginWriteObjectMap( int size )
      {
      writer.Write( (byte)Datatypes.OBJECT_DATATYPE_V3 );
      writer.WriteVarInt( 0x3 | size << 4 ); // classInfo with size of the property count
      writer.WriteVarInt( 1 ); // no classname
      }

    public override void WriteFieldName( String s )
      {
      WriteStringOrReferenceId( s );
      }

    public override void WriteNull()
      {
      writer.Write( (byte)Datatypes.NULL_DATATYPE_V3 );
      }

    public override void WriteInteger( int number )
      {
      if ( number >= -268435456 && number <= 268435455 )
        {
        writer.Write( (byte)Datatypes.INTEGER_DATATYPE_V3 );
        writer.WriteVarInt( (int)( (int)number & 0x1fffffff ) );
        }
      else
        {
        writer.Write( (byte)Datatypes.DOUBLE_DATATYPE_V3 );
        writer.WriteDouble( number );
        }
      }

    public void WriteDouble( double number, bool writeMarker )
      {
      if ( writeMarker )
        writer.Write( (byte)Datatypes.DOUBLE_DATATYPE_V3 );

      writer.WriteDouble( number );
      }

    public void WriteUncompressedUInteger( uint number )
      {
      writer.WriteUInt( number );
      }

    public void WriteUncompressedInteger( int number )
      {
      writer.WriteInt( number );
      }

    public void WriteVarIntWithoutMarker( int number )
      {
      writer.WriteVarInt( number );
      }

    public override void WriteDouble( double number )
      {
      WriteDouble( number, true );
      }

    public override void BeginWriteNamedObject( string objectName, int fieldCount )
      {
      writer.Write( (byte)Datatypes.OBJECT_DATATYPE_V3 );
      writer.WriteVarInt( 0x3 | fieldCount << 4 );

      if ( objectName == null )
        writer.WriteVarInt( 1 );
      else
        {
        //UTF8Util.writeUTF( writer, objectName, true );
        WriteStringOrReferenceId( objectName );
        referenceCache.AddToTraitsCache( objectName );
        }
      }

    public override void BeginWriteObject( int fieldCount )
      {
      writer.Write( (byte)Datatypes.OBJECT_DATATYPE_V3 );
      writer.WriteVarInt( 0x3 | fieldCount << 4 );
      writer.WriteVarInt( 0x1 );
      }

    public override void EndWriteObject()
      {
      // TODO:  Add AmfV3Formatter.EndWriteObject implementation
      }

    public override void WriteArrayReference( int refID )
      {
      writer.Write( (byte)Datatypes.ARRAY_DATATYPE_V3 );
      writer.WriteVarInt( refID << 1 );
      }

    public override void WriteDateReference( int refID )
      {
      writer.Write( (byte)Datatypes.DATE_DATATYPE_V3 );
      writer.WriteVarInt( refID << 1 );
      }

    public override void WriteObjectReference( int refID )
      {
      writer.Write( (byte)Datatypes.OBJECT_DATATYPE_V3 );
      writer.WriteVarInt( refID << 1 );
      }

    public override void WriteStringReference( int refID )
      {
      writer.Write( (byte)Datatypes.UTFSTRING_DATATYPE_V3 );
      writer.WriteVarInt( refID << 1 );
      }

    public override void WriteString( string s )
      {
      WriteString( s, true );
      }

    public void WriteString( string s, bool writeMarker )
      {
      if ( writeMarker )
        writer.Write( (byte)Datatypes.UTFSTRING_DATATYPE_V3 );

      if ( s.Length == 0 )
        writer.Write( (byte)0x01 );
      else
        {
        //UTF8Util.writeUTF( writer, s, true );

        byte[] buffer = utf8.GetBytes( s );
        writer.WriteVarInt( (int)( buffer.Length << 1 | 0x1 ) );

        if ( buffer.Length > 0 )
          writer.Write( buffer );
        }
      }

#if (FULL_BUILD)
    public override void WriteXML( XmlNode document )
      {
      writer.Write( (byte)Datatypes.LONGXML_DATATYPE_V3 );
      UTF8Util.writeUTF( writer, document.OuterXml, true );
      }
#endif

    private void WriteStringOrReferenceId( String s )
      {
      int id = referenceCache.GetStringId( s );

      if ( id != -1 )
        {
        writer.WriteVarInt( id << 1 );
        }
      else
        {
        referenceCache.AddString( s );
        UTF8Util.writeUTF( writer, s, true );
        }
      }

    public override ProtocolBytes GetBytes()
      {
      ProtocolBytes protocolBytes = new ProtocolBytes();
      protocolBytes.length = (int)stream.Length;
      protocolBytes.bytes = stream.GetBuffer();
      stream.Close();
      writer.Close();
      return protocolBytes;
      //return stream.ToArray();

      //stream.Flush();
      //
      /*
      byte[] b = stream.ToArray();			
      Weborb.Util.Logging.Log.log( Weborb.Util.Logging.LoggingConstants.DEBUG, getByteArrayPrettyPrint( b ) );
      return b;
      */
      }

    private string getByteArrayPrettyPrint( byte[] b )
      {
      string str = "";

      for ( int i = 0; i < b.Length; i++ )
        {
        string ch = b[ i ].ToString( "X" );

        if ( ch.Length == 1 )
          ch = "0" + ch;

        str += ch;
        str += " ";
        }

      return str;
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
    }
  }
