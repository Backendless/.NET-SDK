using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Weborb.Util.Logging;
using Weborb.V3Types.Core;

namespace Weborb.Config
{
  public class ChannelRegistry
  {
    private List<Channel> serviceChannels;
    private static ChannelRegistry singleton;
    public const String CHANNELS = "channels";
    public const String CHANNEL_DEFINITION = "channel-definition";
    public const String ENDPOINT = "endpoint";
    public const String POLLING_ENABLED = "polling-enabled";
    public const String POLLING_INTERVAL_SECONDS = "polling-interval-seconds";

    public ChannelRegistry()
    {
      serviceChannels = new List<Channel>();
    }

    public static ChannelRegistry getInstance()
    {
      if( singleton == null )
      {
        singleton = new ChannelRegistry();
        singleton.configure( ORBConfig.GetInstance().GetFlexConfigPath() );
      }

      return singleton;
    }

    public String getConfigFileName()
    {
      return "weborb-services-config.xml";
    }

    public void configure( String basePath )
    {
      XmlDocument configDoc = new XmlDocument();

      try
      {
        configDoc.Load( Path.Combine( basePath, getConfigFileName() ) );
      }
      catch( Exception exception )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Unable to parse " + getConfigFileName(), exception );
        return;
      }

      XmlElement root = configDoc.DocumentElement;

      XmlNodeList channelDefinitions = root.SelectNodes( CHANNELS + "/" + CHANNEL_DEFINITION );

      for( int i = 0; i < channelDefinitions.Count; i++ )
      {
        XmlElement channelDefinition = (XmlElement) channelDefinitions[ i ];
        XmlElement endpointElement = (XmlElement) channelDefinition.SelectSingleNode( ENDPOINT );
        String id = channelDefinition.Attributes[ "id" ].Value;
        String endpoint = endpointElement.Attributes[ "uri" ].Value;
        Channel channel = new Channel( id, endpoint );
        channel.setChannelClass( channelDefinition.Attributes[ "class" ].Value );

        if( endpointElement.Attributes[ "class" ] != null )
          channel.setEndpointClass( endpointElement.Attributes[ "class" ].Value );
        
        channel.setProperties( parseProperties( (XmlElement) channelDefinition.SelectSingleNode( "properties" ) ) );
        serviceChannels.Add( channel );
      }
    }

    public List<Channel> getChannels()
    {
      return serviceChannels;
    }

    public Channel getChannel( String id )
    {
      foreach( Channel channel in serviceChannels )
        if( channel.getId().Equals( id ) )
          return channel;

      return null;
    }

    private Hashtable parseProperties( XmlElement propertiesElement )
    {
      Hashtable props = new Hashtable();

      if( propertiesElement == null )
        return props;

      XmlNodeList propsNodes = propertiesElement.ChildNodes;

      for( int i = 0; i < propsNodes.Count; i++ )
      {
        Object xmlNode = propsNodes[ i ];

        if( !( xmlNode is XmlElement ) )
          continue;

        XmlElement xmlProperty = (XmlElement) xmlNode;

        if( !xmlProperty.HasChildNodes )
          props.Add( xmlProperty.Name, xmlProperty.InnerText );
        else
          props.Add( xmlProperty.Name, parseProperties( xmlProperty ) );
      }

      return props;
    }
  }
}
