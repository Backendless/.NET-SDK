using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
#if CLOUD
using Microsoft.WindowsAzure.ServiceRuntime;
using Weborb.Cloud;
#endif
using System.Xml;
using Weborb.Config.Configurators;
using Weborb.RDS.Internal;
using Weborb.Util.Logging;
using Weborb.V3Types.Core;

namespace Weborb.Config
  {
  class FlexDynamicServiceConfig : FlexRemotingServiceConfig
    {
    private const string DYNAMIC_SERVICES_FILE = "dynamic-remoting-config.xml";    
    public static List<string> dynamicDestinationsIds = new List<string>();

    // construct and return dynamic configDoc
    public new static XmlDocument GetConfigDoc()
      {      
      XmlDocument configDoc = new XmlDocument();
#if CLOUD
          configDoc.Load( AzureUtil.BlobToStream( "WEB-INF/flex/" + REMOTINGSERVICE_FILE ) );
#else
      String filePath = Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), DYNAMIC_SERVICES_FILE );
      using( var fileStream = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read ) )
      {
        configDoc.Load( fileStream );
      }

      //configDoc.Load( Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), DYNAMIC_SERVICES_FILE ) );
#endif
      return configDoc;
      }

    public override void PreConfig()
      {
      RegenerateServices();
      }

    public static void RegenerateServices()
    {
#if !CLOUD
      if( !Directory.Exists( ORBConfig.GetInstance().GetFlexConfigPath() ) )
        return;
#endif

      RemotingCodeGenConfig config = (RemotingCodeGenConfig) ORBConfig.GetInstance().GetConfig( "weborb/codeGenerator" );

      if( !config.InspectAllClassesForFlashBuilder )
        return;

      dynamicDestinationsIds = new List<string>();
      StringBuilder dynamicServicesConfig = new StringBuilder();

      dynamicServicesConfig.Append( "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                    "<service id=\"dynamic-service\" class=\"Weborb.V3Types.Core.RemotingHandler\"" +
                                    " messageTypes=\"flex.messaging.messages.RemotingMessage\">\n" +
                                    "\t<adapters />\n " +
                                    "\t<default-channels>\n" +
                                    "\t\t<channel ref=\"my-amf\"/>\n" +
                                    "\t\t<channel ref=\"my-secure-amf\"/>\n" +
                                    "\t</default-channels>\n\n" );

      List<ServiceNamePosition> services = RDSServiceScanner.getServices();

      foreach( ServiceNamePosition serviceInfo in services )
      {
        String destinationId = serviceInfo.shortName + "Service";

        if( dynamicDestinationsIds.Contains( destinationId ) )
        {
          // try to construct unique destinationId
          int suffix = 2;
          while( dynamicDestinationsIds.Contains( destinationId + suffix.ToString() ) )
            suffix++;

          destinationId += suffix.ToString();
        }

        dynamicDestinationsIds.Add( destinationId );

        AddDestinationToFile( dynamicServicesConfig, serviceInfo.serviceName, destinationId );
      }

      dynamicServicesConfig.Append( "</service>\n" );

#if CLOUD
        var blob = AzureUtil.GetBlob( GetFullPath( DYNAMIC_SERVICES_FILE ) );
        blob.UploadText( dynamicServicesConfig.ToString() );
#else
      String filePath = Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), DYNAMIC_SERVICES_FILE );

      try
      {
        LocalConfigurator.DisableFlexWatcher();

        using( StreamWriter file = new StreamWriter( filePath ) )
        {
          file.Write( dynamicServicesConfig.ToString() );
        }
      }
      catch( Exception exception )
      {
        String error =
          "Unable to save dynamic destinations. Did you grant Write permission to the WEB-INF/flex folder for the NETWORK SERVICE/ASPNET user?";

        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, error );

        if( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, error, exception );
      }
      finally
      {
        LocalConfigurator.EnableFlexWatcher();
      }
#endif
    }

    public static void SaveConfig()
      {
      RegenerateServices();
      }

    private static void AddDestinationToFile( StringBuilder dynamicServicesFile, string serviceName, string destinationId )
      {
      dynamicServicesFile.Append( "\t<destination id=\"" + destinationId + "\">\n" );
      dynamicServicesFile.Append( "\t\t<properties>\n" );
      dynamicServicesFile.Append( "\t\t<source>" ).Append( serviceName ).Append( "</source>\n" );
      dynamicServicesFile.Append( "\t\t</properties>\n" );
      dynamicServicesFile.Append( "\t</destination>\n\n" );
      }

    public override string GetConfigFileName()
      {
      return DYNAMIC_SERVICES_FILE;
      }
    }
  }
