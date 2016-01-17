#if( CLOUD )
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Xml;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;

using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

using Weborb.Util;
using Weborb.Cloud;
using Weborb.Util.Logging;

namespace Weborb.Config
{
  public class AzureExternalConfig : IExternalConfig
  {
    private static string configFileName = "weborb.config";

    private static CloudBlobContainer storageFacility = null;

    public static CloudBlobContainer StorageFacility
    {
      get { return storageFacility; }
    }

    static AzureExternalConfig()
    {
      initializeStorageFacility();
    }

    public static void startRTMPServer()
    {
      try
      {
        AzureUtil.LocalResourceRoot = RoleEnvironment.GetLocalResource( ORBConstants.LOCAL_STORAGE ).RootPath;

        AzureUtil.CopyDirectory( Path.Combine( Paths.GetWebORBPath(), "Applications" ),
                                 Path.Combine( AzureUtil.LocalResourceRoot, "Applications" ) );

        int port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[ "RTMPIn" ].IPEndpoint.Port;

        // Create Messaging server. 'port' is the port number, 500 is connection backlog
        Messaging.RTMPServer server = new Messaging.RTMPServer( "default", port, 500, ORBConfig.GetInstance() );

        // Start the messaging server
        server.start();

        if( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "RTMP server was started on the port " + port );

        // Stote the server instance in the Application context, so it can be cleared out when application stops     
        //HttpContext.Current.ApplicationInstance.Application[ "weborbMessagingServer" ] = server;
      }
      catch( Exception e )
      {
        if( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, "Cann't start RTMP Server", e );
      }
    }

    public static CloudBlob ConfigBlob
    {
      get
      {
        return storageFacility.GetBlobReference( configFileName );
      }
    }

    #region IExternalConfig Members

    public Hashtable Configure( ORBConfig config, ArrayList sectionsToProcess )
    {
      if( ConfigBlob != null )
      {
        Hashtable configObjects = new Hashtable();

        using( XmlTextReader configReader = new XmlTextReader( GetConfigStream() ) )
        {
          XmlDocument configDoc = new XmlDocument();
          configDoc.PreserveWhitespace = true;
          configDoc.Load( configReader );
          XmlElement root = configDoc.DocumentElement;
          XmlNode configSections = root.GetElementsByTagName( "configSections" )[ 0 ];

          foreach( XmlNode section in configSections.ChildNodes )
          {
            if( section is XmlWhitespace )
              continue;

            string sectionName = section.Attributes[ "name" ].Value.Trim();

            Hashtable handlers = ParseSection( config, section.ChildNodes, sectionsToProcess );
            XmlNodeList configSection = root.GetElementsByTagName( sectionName );

            if( configSection.Count > 1 )
              throw new ConfigurationException(
                "invalid configuration in weborb.config. Found more than one section with name " + sectionName );

            ProcessSection( config, sectionName, configSection[ 0 ].ChildNodes, handlers, configObjects,
                            sectionsToProcess );
          }
        }

        return configObjects;
      }
      else
      {
        return new Hashtable();
      }
    }

    public Stream GetConfigStream()
    {
      if( ConfigBlob != null )
      {
        return new MemoryStream( ConfigBlob.DownloadByteArray() );
      }
      else
        return new MemoryStream();
    }

    public void SaveConfig( XmlTextWriter writer )
    {
      writer.Flush();
      writer.BaseStream.Position = 0;
      ConfigBlob.UploadFromStream( writer.BaseStream );
      ConfigBlob.Metadata[ "timestamp" ] = ORBUtil.currentTimeMillis().ToString();
      ConfigBlob.SetMetadata();
      ORBConfig.reset();
    }

    public void FlushConfig()
    {
      ConfigBlob.Delete();
    }

    #endregion

    private static Hashtable ParseSection( ORBConfig config, XmlNodeList sections, ArrayList sectionsToProcess )
    {
      Hashtable sectionHandlers = new Hashtable();

      foreach( XmlNode node in sections )
      {
        if( !( node is XmlElement ) )
          continue;

        string name = node.Attributes[ "name" ].Value.Trim();

        if( sectionsToProcess != null && !sectionsToProcess.Contains( name ) )
          continue;

        string type = node.Attributes[ "type" ].Value.Trim();
        Type configHandlerType = TypeLoader.LoadType( type );
        sectionHandlers[ name ] = config.getObjectFactories()._CreateServiceObject( configHandlerType );
      }

      return sectionHandlers;
    }

    private static void ProcessSection( ORBConfig config, string configSectionName, XmlNodeList sectionNodes, Hashtable sectionHandlers, Hashtable configObjects, ArrayList sectionsToProcess )
    {
      foreach( XmlNode node in sectionNodes )
      {
        if( !( node is XmlElement ) )
          continue;

        string sectionName = node.Name;

        if( sectionsToProcess != null && !sectionsToProcess.Contains( sectionName ) )
          continue;

        if( !sectionHandlers.ContainsKey( sectionName ) )
        {
          if( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR,
                   "Cannot find a configuration handler for element " + sectionName + " in section " +
                   node.ParentNode.Name );
          continue;
        }

        IConfigurationSectionHandler handler = (IConfigurationSectionHandler) sectionHandlers[ sectionName ];
        object configObject = handler.Create( null, handler is ORBConfigHandler ? config : null, node );

        if( configObject != null )
          configObjects[ configSectionName + "/" + sectionName ] = configObject;
      }
    }

    private static void initializeStorageFacility()
    {
      CloudBlobClient storage = AzureUtil.StorageAccount.CreateCloudBlobClient();
      storageFacility = storage.GetContainerReference( ORBConstants.WEBORB_AZURE_CONTAINER );

      var permissions = storageFacility.GetPermissions();
      permissions.PublicAccess = BlobContainerPublicAccessType.Container;
      storageFacility.SetPermissions( permissions );

      var blob = storageFacility.GetBlobReference( configFileName );
      blob.UploadFile( Path.Combine( Paths.GetWebORBPath(), configFileName ) );
    }
  }
}
#endif