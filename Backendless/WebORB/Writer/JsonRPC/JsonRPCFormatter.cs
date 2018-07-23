using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Weborb.Exceptions;
using Weborb.Message;
using Weborb.Protocols.JsonRPC;

namespace Weborb.Writer.JsonRPC
{
  class JsonRPCFormatter : IProtocolFormatter
  {
    private ObjectSerializer objectSerializer;
    private ReferenceCache referenceCache;
    private bool fault;
    private JsonTextWriter writer;
    private int beginSelectBytesIndex;
    private Dictionary<Type, ITypeWriter> customWriters = new Dictionary<Type, ITypeWriter>();

    public JsonRPCFormatter()
    {
      objectSerializer = new ObjectSerializer();
      referenceCache = new ReferenceCache();
      writer = new JsonTextWriter();

      JsonNumberWriter numberWriter = new JsonNumberWriter();
      customWriters.Add( typeof( int ), numberWriter );
      customWriters.Add( typeof( long ), numberWriter );
      customWriters.Add( typeof( float ), numberWriter );
      customWriters.Add( typeof( double ), numberWriter );
      customWriters.Add( typeof( short ), numberWriter );
      customWriters.Add( typeof( byte ), numberWriter );
      customWriters.Add( typeof( Decimal ), numberWriter );
    }

    internal override void BeginSelectCacheObject()
    {
      beginSelectBytesIndex = writer.ToString().Length;
    }

    internal override object EndSelectCacheObject()
    {
      int length = writer.ToString().Length - beginSelectBytesIndex;
      if( length <= 0 )
        return "";

      string substring = writer.ToString().Substring( beginSelectBytesIndex, length );

      return substring;
    }

    internal override void WriteCachedObject( object cached )
    {
      writer.WriteCachedJSON( (string) cached );
    }

    public override ITypeWriter getWriter( Type type )
    {
      if( customWriters.ContainsKey( type ) )
        return customWriters[ type ];

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

    public override void BeginWriteMessage( Request message )
    {
      writer = new JsonTextWriter();
      Body[] body = message.getResponseBodies();
      fault = (body.Length > 0 && body[ 0 ].ResponseDataObject is Exception);
      writer.WriteStartObject();

      // write out version
      string version = (string) message.Headers[ 1 ].headerValue.defaultAdapt();
      if( version == "1.1" )
      {
        writer.WriteMember( "version" );
        writer.WriteString( "1.1" );
      }
      else if( version == "2.0" )
      {
        writer.WriteMember( "jsonrpc" );
        writer.WriteString( "2.0" );
      }

      // if fault - modify error message to better comply JSON-RPC specification
      if( fault )
        body[ 0 ].responseDataObject = ProcessExceptionStack( version, (Exception) body[ 0 ].responseDataObject );

      // hack: modify body if 'inspection' is ON to avoid cyclic references in ServiceDescriptor
      //if( message.Headers.Length > 3 )
      //  body[ 0 ].responseDataObject = ((ServiceDescriptor) body[ 0 ].responseDataObject).functions;

      // write out id if was present          
      if( message.getHeader( "id" ).headerValue != null )
      {
        writer.WriteMember( "id" );
        object id = message.getHeader( "id" ).headerValue.defaultAdapt();
        MessageWriter.writeObject( id, message.GetFormatter() );
      }

      // only in version 1.0 was allowed to print out result:null on failure and error:null on successfull invocation
      if( version == "1.0" )
      {
        writer.WriteMember( fault ? "result" : "error" );
        writer.WriteNull();
      }

      writer.WriteMember( !fault ? "result" : "error" );
    }

    private object ProcessExceptionStack( string version, Exception error )
    {
      if( version == "1.0" )
        return error;

      Dictionary<string, object> newError = new Dictionary<string, object>();

      // set error message
      newError[ "message" ] = error.Message;

      // find out error code
      int code = -32603;
      if( error is ServiceException )
      {
        if( ((ServiceException) error).description.StartsWith( "unable to find method" ) )
          code = -32601;
        if( ((ServiceException) error).description.StartsWith( "None of the handlers were able to invoke" ) )
          code = -32600;

        if( code == -32603 )
          newError[ "message" ] = ((ServiceException) error).description;
      }

      newError[ "code" ] = code;

      if( version == "1.1" )
      {
        newError[ "name" ] = "JSONRPCError";
        newError[ "error" ] = (error.InnerException != null) ? ProcessExceptionStack( version, error.InnerException ) :
                                                                  error.StackTrace;
        return newError;
      }
      else
      {
        newError[ "data" ] = (error.InnerException != null) ? ProcessExceptionStack( version, error.InnerException ) :
                                                                  error.StackTrace;
        return newError;
      }
    }

    public override void EndWriteMessage()
    {
      writer.WriteEndObject();
    }

    public override void WriteMessageVersion( float version )
    {

    }

    public override void BeginWriteArray( int length )
    {
      writer.WriteStartArray();
    }

    public override void EndWriteArray()
    {
      writer.WriteEndArray();
    }

    public override void WriteBoolean( bool b )
    {
      writer.WriteBoolean( b );
    }

    public override void WriteDate( DateTime datetime )
    {
      string date = datetime.ToString( "MMMM dd, yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo );
      writer.WriteString( date );
    }

    public override void BeginWriteObjectMap( int size )
    {
      BeginWriteObject( size );
    }

    public override void EndWriteObjectMap()
    {
      EndWriteObject();
    }

    public override void WriteFieldName( String s )
    {
      writer.WriteMember( s );
    }

    public override void BeginWriteFieldValue()
    {
    }

    public override void EndWriteFieldValue()
    {
    }

    public override void WriteNull()
    {
      writer.WriteNull();
    }

    public override void WriteInteger( int number )
    {
      writer.WriteNumber( number );
    }

    public void WriteLong( long number )
    {
      writer.WriteNumber( number );
    }

    public override void WriteDouble( double number )
    {
      writer.WriteNumber( number );
    }

    public override void BeginWriteNamedObject( string objectName, int fieldCount )
    {
      BeginWriteObject( fieldCount );
    }

    public override void EndWriteNamedObject()
    {
      EndWriteObject();
    }

    public override void BeginWriteObject( int fieldCount )
    {
      writer.WriteStartObject();
    }

    public override void EndWriteObject()
    {
      writer.WriteEndObject();
    }

    public override void WriteArrayReference( int refID )
    {
      WriteReference( refID );
    }

    public override void WriteDateReference( int refID )
    {
      WriteReference( refID );
    }

    public override void WriteObjectReference( int refID )
    {
      WriteReference( refID );
    }

    public override void WriteStringReference( int refID )
    {
      WriteReference( refID );
    }

    private void WriteReference( int refID )
    {
      throw new Exception( "Writing references by JsonRpc isn't supported" );
    }

    public override void WriteString( string s )
    {
      writer.WriteString( s );
    }

    public override void WriteByteArray( byte[] array )
    {
      throw new NotImplementedException();
    }

    /*
    public override void WriteXML( XmlNode document )
    {
      // TODO:  Test this
      writer.WriteString( document.OuterXml );
    }
    */

    public override ProtocolBytes GetBytes()
    {
      byte[] bytes = Encoding.UTF8.GetBytes( writer.ToString() );
      ProtocolBytes protocolBytes = new ProtocolBytes();
      protocolBytes.length = (int) bytes.Length;
      protocolBytes.bytes = bytes;
      return protocolBytes;
    }

    public override void Cleanup()
    {
      writer.Close();
    }

    public override string GetContentType()
    {
      return "application/json";
    }

    public override IObjectSerializer GetObjectSerializer()
    {
      return objectSerializer;
    }

  }
}
