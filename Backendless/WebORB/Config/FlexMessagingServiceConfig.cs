using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Weborb.Management.Configuration.Destinations;
using Weborb.Util;
using Weborb.Cloud;
using Weborb.Util.License;
using Weborb.Util.Logging;
using Weborb.V3Types.Core;
using Weborb.Messaging.PubSub;

namespace Weborb.Config
{
  public class FlexMessagingServiceConfig : BaseFlexConfig
  {
    protected static XmlDocument configDoc;
    public const string MESSAGING_CONFIG_FILENAME = "messaging-config.xml";
    public static FlexMessagingServiceConfig Instance;
    private Dictionary<String, FlexMessagingDestination> flexMessagingDestinations = new Dictionary<String, FlexMessagingDestination>();

    public FlexMessagingServiceConfig()
    {
      Instance = this;
    }

    public Dictionary<string, FlexMessagingDestination> FlexMessagingDestinations
    {
      get { return flexMessagingDestinations; }
    }

    public override String GetConfigFileName()
    {
      return MESSAGING_CONFIG_FILENAME;
    }

    public override String GetDefaultServiceHandlerName()
    {
      return "Weborb.Messaging.PubSub.Memory.MessagingServiceHandler";
    }

    public ICollection<FlexMessagingDestination> getFlexMessagingDestinations()
    {
      return flexMessagingDestinations.Values;
    }

    public override string GetXPath()
    {
      return "/service";
    }

    public override void PreConfig()
    {
    }

    public override void PostConfig()
    {
    }

    public void clear()
    {
      DestinationManager destinationManager =
        orbConfig.GetDataServices().GetDestinationManager();

      foreach( String name in flexMessagingDestinations.Keys )
        destinationManager.RemoveDestination( name );

      flexMessagingDestinations.Clear();
    }

    public override IDestination ProcessDestination( ORBConfig orbConfig, string destinationId, XmlElement destinationNode )
    {
      if( Log.isLogging( LoggingConstants.INFO ) )
        Log.log( LoggingConstants.INFO, "Registered Flex Messaging destination - " + destinationId );
      MessagingDestination destination = new MessagingDestination( destinationId );

      XmlNode channels = (XmlNode) destinationNode.SelectSingleNode( "channels" );
      XmlNode channel = null;

      if( channels != null )
        channel = (XmlNode) channels.SelectSingleNode( "channel" );

      String channelName = null;
      
      if( channel != null )
        channelName = channel.Attributes[ "ref" ].Value;

      if( channelName != null )
        destination.ChannelName = channelName;
      else if( Log.isLogging( LoggingConstants.ERROR ) )
        Log.log( LoggingConstants.ERROR, "Flex Messaging destination [" + destination.name
                                          + "] does not have channel." );

      return destination;
    }

    public static XmlDocument GetConfigDoc()
    {
      if( configDoc != null )
        return configDoc;

      configDoc = new XmlDocument();
#if CLOUD
      configDoc.Load( AzureUtil.BlobToStream( "WEB-INF/flex/" + MESSAGING_CONFIG_FILENAME ) );
#else
      configDoc.Load( Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), MESSAGING_CONFIG_FILENAME ) );
#endif
      return configDoc;
    }

    public static void SaveConfig()
    {
#if CLOUD
      MemoryStream stream = new MemoryStream();
      configDoc.Save( stream );
      stream.Position = 0;
      AzureUtil.GetBlob( "WEB-INF/flex/" + MESSAGING_CONFIG_FILENAME ).UploadFromStream( stream );
#else
      configDoc.Save( Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), MESSAGING_CONFIG_FILENAME ) );
#endif
    }

    public static Boolean initDestinationServiceHandler( IDestination destination )
    {
      if( destination.SetConfigServiceHandler() )
      {
        if( destination.GetServiceHandler() == null )
          destination.SetServiceHandler( serviceHandler );
        return true;
      }
      else
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Destination is not properly configured. Destination '" + destination.GetName() + "' cannot be registered" );
      }
      return false;
    }
  }
}
