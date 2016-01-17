using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Text;
using Weborb.Writer;
using Weborb.Message;

namespace Weborb.Writer.Wolf
  {
  public class WolfFormatter : IProtocolFormatter
    {
    private Stack stack;
    private XmlDocument doc;
    private bool serializeAsFault;
    private IObjectSerializer objectSerializer;
    private ReferenceCache referenceCache;

    private string WOLF = "WOLF";
    private string ARRAY = "Array";
    private string BOOLEAN = "Boolean";
    private string DATE = "Date";
    private string FIELD = "Field";
    private string NAME = "Name";
    private string VALUE = "Value";
    private string UNDEFINED = "Undefined";
    private string NUMBER = "Number";
    private string OBJECT = "Object";
    private string REFERENCE = "Reference";
    private string STRING = "String";

    public WolfFormatter()
      : this( false )
      {
      }

    public WolfFormatter( bool optimize )
      {
      if ( optimize )
        {
        WOLF = "a";
        ARRAY = "b";
        BOOLEAN = "c";
        DATE = "d";
        FIELD = "e";
        NAME = "f";
        VALUE = "g";
        UNDEFINED = "h";
        NUMBER = "i";
        OBJECT = "j";
        REFERENCE = "k";
        STRING = "l";
        }

      doc = new XmlDocument();
      stack = new Stack();
      XmlElement element = doc.CreateElement( WOLF );
      doc.AppendChild( element );
      stack.Push( element );
      objectSerializer = new ObjectSerializer();
      referenceCache = new ReferenceCache();
      }
    #region IProtocolFormatter Members

    internal override void BeginSelectCacheObject()
      {
      }

    internal override object EndSelectCacheObject()
      {
      XmlElement topElement = (XmlElement)stack.Peek();
      return topElement.LastChild.OuterXml;
      }

    internal override void WriteCachedObject( object cached )
      {
      ( (XmlElement)stack.Peek() ).InnerXml = (string)cached;
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

    public override void BeginWriteMessage( Request message )
      {
      Body[] body = message.getResponseBodies();
      serializeAsFault = ( body.Length > 0 && body[ 0 ].ResponseDataObject is Exception );
      }

    public override void EndWriteMessage()
      {
      }

    public override void WriteMessageVersion( float version )
      {
      ( (XmlElement)stack.Peek() ).SetAttribute( "version", version.ToString() );
      stack.Push( doc.CreateElement( serializeAsFault ? "Fault" : "Response" ) );
      }

    public override void BeginWriteArray( int length )
      {
      stack.Push( doc.CreateElement( ARRAY ) );
      }

    public override void EndWriteArray()
      {
      XmlElement e = (XmlElement)stack.Pop();
      ( (XmlElement)stack.Peek() ).AppendChild( e );
      }

    public override void WriteBoolean( bool b )
      {
      XmlElement boolElement = doc.CreateElement( BOOLEAN );
      boolElement.InnerText = b ? "true" : "false";
      ( (XmlElement)stack.Peek() ).AppendChild( boolElement );
      }

    public override void WriteDate( DateTime datetime )
      {
      XmlElement dateElement = doc.CreateElement( DATE );
      dateElement.InnerText = datetime.ToString( "MMMM dd, yyyy HH:mm:ss", DateTimeFormatInfo.InvariantInfo );
      ( (XmlElement)stack.Peek() ).AppendChild( dateElement );
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
      XmlElement fieldElement = doc.CreateElement( FIELD );
      XmlElement nameElement = doc.CreateElement( NAME );
      nameElement.InnerText = s;
      fieldElement.AppendChild( nameElement );
      stack.Push( fieldElement );
      }

    public override void BeginWriteFieldValue()
      {
      stack.Push( doc.CreateElement( VALUE ) );
      }

    public override void EndWriteFieldValue()
      {
      // pop Value
      XmlElement e = (XmlElement)stack.Pop();
      // add it to Field
      ( (XmlElement)stack.Peek() ).AppendChild( e );

      // pop Field
      e = (XmlElement)stack.Pop();
      // add it to Object
      ( (XmlElement)stack.Peek() ).AppendChild( e );
      }

    public override void WriteNull()
      {
      ( (XmlElement)stack.Peek() ).AppendChild( doc.CreateElement( UNDEFINED ) );
      }

    public override void WriteInteger( int number )
      {
      XmlElement numberElement = doc.CreateElement( NUMBER );
      numberElement.InnerText = number.ToString();
      ( (XmlElement)stack.Peek() ).AppendChild( numberElement );
      }

    public override void WriteDouble( double number )
      {
      XmlElement numberElement = doc.CreateElement( NUMBER );
      numberElement.InnerText = number.ToString();
      ( (XmlElement)stack.Peek() ).AppendChild( numberElement );
      }

    public override void BeginWriteNamedObject( string objectName, int fieldCount )
      {
      XmlElement objectElement = doc.CreateElement( OBJECT );
      objectElement.SetAttribute( "objectName", objectName );
      stack.Push( objectElement );
      }

    public override void EndWriteNamedObject()
      {
      EndWriteObject();
      }

    public override void BeginWriteObject( int fieldCount )
      {
      stack.Push( doc.CreateElement( OBJECT ) );
      }

    public override void EndWriteObject()
      {
      XmlElement e = (XmlElement)stack.Pop();
      ( (XmlElement)stack.Peek() ).AppendChild( e );
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
      XmlElement refElement = doc.CreateElement( REFERENCE );
      refElement.InnerText = refID.ToString();
      ( (XmlElement)stack.Peek() ).AppendChild( refElement );
      }

    public override void WriteString( string s )
      {
      XmlElement strElement = doc.CreateElement( STRING );
      strElement.InnerText = s;
      ( (XmlElement)stack.Peek() ).AppendChild( strElement );
      }

    public override void WriteByteArray(byte[] array)
    {
      throw new NotImplementedException();
    }

    public override void WriteXML( XmlNode document )
      {
        WriteString( document.InnerXml );
      // TODO:  Add WolfFormatter.WriteXML implementation
      }

    public void WriteXmlNode( XmlNode element )
      {
      element = doc.ImportNode( element, true );
      ( (XmlElement)stack.Peek() ).AppendChild( element );
      }

    public override ProtocolBytes GetBytes()
      {
      resolveStack();
      MemoryStream stream = new MemoryStream();
      XmlTextWriter writer = new XmlTextWriter( stream, new UTF8Encoding( false ) );
      doc.WriteTo( writer );
      writer.Flush();
      ProtocolBytes protocolBytes = new ProtocolBytes();
      protocolBytes.length = (int)stream.Length;
      protocolBytes.bytes = stream.GetBuffer();
      writer.Close();
      stream.Close();
      return protocolBytes;
      }

    private XmlElement resolveStack()
      {
      XmlElement element = (XmlElement)stack.Pop();

      while ( stack.Count != 0 )
        {
        ( (XmlElement)stack.Peek() ).AppendChild( element );
        element = (XmlElement)stack.Pop();
        }

      return element;
      }

    public override void Cleanup()
      {
      // TODO:  Add WolfFormatter.Cleanup implementation
      }

    public override string GetContentType()
      {
      return "text/xml";
      }

    public override IObjectSerializer GetObjectSerializer()
      {
      return objectSerializer;
      }

    #endregion

    public XmlDocument getDocument()
      {
      resolveStack();
      return doc;
      }
    }
  }
