using System;
using System.Xml;
using System.Globalization;
using System.Collections;
using System.Web;
using System.IO;

using Weborb.Reader.Wolf;
using Weborb.Writer.Wolf;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Protocols;
using Weborb.Message;
using Weborb.Util.Logging;

namespace Weborb.Protocols.Wolf
{
  public class RequestParser : IMessageFactory
  {
    private static RequestParser instance;
    private static Hashtable readers;

    public RequestParser()
    {
      readers = new Hashtable();
      readers[ "Array" ] = new Weborb.Reader.Wolf.ArrayReader();
      readers[ "b" ] = readers[ "Array" ];

      readers[ "Boolean" ] = new Weborb.Reader.Wolf.BooleanReader();
      readers[ "c" ] = readers[ "Boolean" ];

      readers[ "Date" ] = new Weborb.Reader.Wolf.DateReader();
      readers[ "d" ] = readers[ "Date" ];

      readers[ "Undefined" ] = new Weborb.Reader.Wolf.NullReader();
      readers[ "h" ] = readers[ "Undefined" ];

      readers[ "Number" ] = new Weborb.Reader.Wolf.NumberReader();
      readers[ "i" ] = readers[ "Number" ];

      readers[ "Object" ] = new Weborb.Reader.Wolf.ObjectReader();
      readers[ "j" ] = readers[ "Object" ];

      readers[ "Reference" ] = new Weborb.Reader.Wolf.ReferenceReader();
      readers[ "k" ] = readers[ "Reference" ];

      readers[ "String" ] = new Weborb.Reader.Wolf.StringReader();
      readers[ "l" ] = readers[ "String" ];
      readers[ "XML" ] = new Weborb.Reader.Wolf.XmlReader();
      
      readers[ "Ref" ] = new Weborb.Reader.Wolf.RefReader();
      
      instance = this;
    }


    public string GetProtocolName( Request input )
    {
      return "wolf";
    }

    public string[] GetProtocolNames()
    {
      return new string[] { "wolf" };
    }

    public static RequestParser GetInstance()
    {
      if( instance == null )
        instance = new RequestParser();

      return instance;
    }

    public IAdaptingType ParseElement( XmlNode xmlNode, ParseContext parseContext )
    {
      IXMLTypeReader reader = (IXMLTypeReader) readers[ xmlNode.Name ];
      return reader.read( (XmlElement) xmlNode, parseContext );
    }
    #region IMessagetFactory Members

    public bool CanParse( string contentType )
    {
      return contentType.ToLower().Contains( "wolf/xml" );
    }

    public Request Parse( Stream requestStream )
      {
          System.Xml.XmlReader reader = XmlTextReader.Create(requestStream);
        
          XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
         
          xmlReaderSettings.IgnoreProcessingInstructions = true;
          
          #if NET_20
          xmlReaderSettings.ProhibitDtd = true;
          #endif

          #if NET_40
          xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
          #endif

          System.Xml.XmlDocument document = new System.Xml.XmlDocument();
          //document.Load( requestStream );
        
          document.Load(reader);

      if( Log.isLogging( LoggingConstants.DEBUG ) )
        Log.log( LoggingConstants.DEBUG, document.InnerXml );

      XmlElement requestRoot = document.DocumentElement;
      String version = requestRoot.GetAttribute( "version" );
      XmlElement requestElement = (XmlElement) requestRoot.GetElementsByTagName( "Request" )[ 0 ];
      XmlNode headersElement = requestElement.GetElementsByTagName( "Headers" )[ 0 ];
      ArrayList headers = new ArrayList();

      int requestID = int.Parse( requestElement.GetAttribute( "id" ) );
      headers.Add( new Header( "requestid", false, 0, new NumberObject( requestID ) ) );

      if( headersElement != null )
        foreach( XmlNode headerElement in headersElement )
        {
          if( !( headerElement is XmlElement ) )
            continue;

          IAdaptingType headerValue = null;

          if( headerElement.FirstChild != null )
            headerValue = ParseElement( headerElement.FirstChild, new ParseContext() );
          else
            headerValue = new StringType( headerElement.InnerText.Trim() );

          headers.Add( new Header( headerElement.Name, false, 0, headerValue ) );
        }

      String target = requestElement.GetElementsByTagName( "Target" )[ 0 ].InnerText.Trim();
      String methodName = requestElement.GetElementsByTagName( "Method" )[ 0 ].InnerText.Trim();
      string serviceURL = "";

      // when dealing with WOLF-PubSub functionality set serviceURL to "null"
      if( methodName == "sendWOLFPubSubMessage" )
        serviceURL = "null";
      else
        serviceURL = target + "." + methodName;

      Body bodyPart = new Body( serviceURL, null, 0, parseArguments( requestElement.GetElementsByTagName( "Arguments" )[ 0 ] ) );
      NumberFormatInfo formatInfo = new CultureInfo( 0x0409 ).NumberFormat;
      Request msg = new Request( float.Parse( version, formatInfo ), (Header[]) headers.ToArray( typeof( Header ) ), new Body[] { bodyPart } );
      msg.SetFormatter( new WolfFormatter() );
      return msg;
    }

    private object[] parseArguments( XmlNode arguments )
    {
      ParseContext parseContext = new ParseContext();
      ArrayList argsList = new ArrayList();

      foreach( XmlNode xmlNode in arguments )
      {
        if( !( xmlNode is XmlElement ) )
          continue;

        argsList.Add( ParseElement( xmlNode, parseContext ) );
      }

      parseContext.setRefObjects();

      return argsList.ToArray();
    }

    #endregion
  }
}