using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections.Generic;

using Weborb.Writer;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
  public class SerializationConfigHandler : ORBConfigHandler
  {
    private string enumWriter;
#if NET_35 || NET_40
    private ICollection<String> keywords = new HashSet<String>();
#else
    private ICollection<String> keywords = new List<String>();
#endif
    public override object Configure( object parent, object configContext, XmlNode section )
    {
      foreach( XmlNode node in section.ChildNodes )
      {
        if( node.Name.Equals( "serializeGenericsAsVector" ) )
          SerializeGenericCollectionAsVector = node.InnerText.Trim().ToLower().Equals( "yes" );

        if( node.Name.Equals( "serializePrivateFields" ) )
          SerializePrivateFields = !node.InnerText.Trim().ToLower().Equals( "no" );

        if( node.Name.Equals( "legacyCollectionSerialization" ) )
          LegacyCollectionSerialization = node.InnerText.Trim().ToLower().Equals( "yes" );

        if( node.Name.Equals( "enumerationSerializer" ) )
        {
          enumWriter = node.InnerText.Trim();

          try
          {
            EnumerationWriter enumWriterObj = (EnumerationWriter) ObjectFactories.CreateServiceObject( enumWriter );
            MessageWriter.SetEnumerationWriter( enumWriterObj );
          }
          catch( Exception exception )
          {
            if( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Error configuring enumeration serializer", exception );
          }
        }
      }

      XmlNodeList keywordsList = ( (XmlElement) section ).SelectNodes( "keywordSubstitution/reservedKeywords/keyword" );

      foreach( XmlNode node in keywordsList )
        keywords.Add( node.InnerText.Trim() );

      PrefixForKeywords = ( (XmlElement) section ).SelectNodes( "keywordSubstitution/prefixKeywordsWith" )[ 0 ].InnerText.Trim();

      return this;
    }

    public bool LegacyCollectionSerialization { get; set; }

    public bool SerializeGenericCollectionAsVector { get; set; }

    public bool SerializePrivateFields { get; set; }

    public String PrefixForKeywords { get; set; }

    public ICollection<String> Keywords
    {
      get
      {
        return keywords;
      }
    }

    public string EnumerationSerializer
    {
      get { return enumWriter; }
    }
  }
}
