using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
#if CLOUD
using Microsoft.WindowsAzure.ServiceRuntime;
using Weborb.Cloud;
#endif
using Weborb.Management.Configuration.Destinations;
using Weborb.Messaging.PubSub;
using Weborb.Util.License;
using Weborb.Util.Logging;
using Weborb.Util;
using Weborb.V3Types.Core;


namespace Weborb.Config
  {
  public abstract class BaseFlexConfig
    {
    internal ORBConfig orbConfig;
    internal String basePath;
    public abstract String GetConfigFileName();
    public abstract String GetDefaultServiceHandlerName();
    public abstract void PreConfig();
    public abstract void PostConfig();
    public abstract String GetXPath();
    public abstract IDestination ProcessDestination( ORBConfig config, string destinationId, XmlElement xmlElement );

    protected static IServiceHandler serviceHandler;

    public static string GetFullPath( string fileName )
      {
      return @"WEB-INF\flex\" + fileName;
      }

    public void Configure( string basePath, ORBConfig orbConfig )
      {
      this.orbConfig = orbConfig;
      this.basePath = basePath;
      PreConfig();

      String filePath = Path.Combine( basePath, GetConfigFileName() );

      if ( !File.Exists( filePath ) )
        {
        if ( Log.isLogging( LoggingConstants.INFO ) )
          Log.log( LoggingConstants.INFO, "File not found " + filePath );
        return;
        }

#if CLOUD
            var blob = AzureUtil.GetBlob( GetFullPath( GetConfigFileName() ) );
            // Encode the XML string in a UTF-8 byte array
            byte[] encodedString = Encoding.UTF8.GetBytes( blob.DownloadText() );
#else
      String fileContent;
      using (var fileStream = new FileStream( filePath, FileMode.Open, FileAccess.Read, FileShare.Read) )
      using (var textReader = new StreamReader(fileStream))
      {
          fileContent = textReader.ReadToEnd();
      }

      //byte[] encodedString = Encoding.UTF8.GetBytes( File.ReadAllText( filePath ) );
      System.Text.UTF8Encoding  encoding=new System.Text.UTF8Encoding();
      byte[] encodedString = encoding.GetBytes( fileContent );
#endif
      // Put the byte array into a stream and rewind it to the beginning
      MemoryStream content = new MemoryStream( encodedString );
      content.Flush();
      content.Position = 0;

      XmlDocument configDoc = new XmlDocument();
      configDoc.PreserveWhitespace = false;

      try
        {
        configDoc.Load( content );
        }
      catch ( Exception exception )
        {
        if ( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Unable to parse " + GetConfigFileName(), exception );

        return;
        }

      XmlNodeList servicesNodes = configDoc.SelectNodes( GetXPath() );

      foreach ( XmlNode serviceNode in servicesNodes )
        {
        //XmlElement root = (XmlElement) 
        //configDoc.DocumentElement;
        XmlElement root = (XmlElement)serviceNode;

        XmlNodeList destinationNodes = root.GetElementsByTagName( "destination" );

        if ( destinationNodes.Count == 0 )
          continue;

        String serviceHandlerClassName = root.GetAttribute( "class" );

        if ( serviceHandlerClassName == null || serviceHandlerClassName.Length == 0 )
          serviceHandlerClassName = GetDefaultServiceHandlerName();

        serviceHandler = (IServiceHandler)ObjectFactories.CreateServiceObject( serviceHandlerClassName );

        XmlElement adaptersNode = (XmlElement)root.GetElementsByTagName( "adapters" )[ 0 ];
        XmlNodeList adaptersDefNodes = adaptersNode.GetElementsByTagName( "adapter-definition" );
        IEnumerator en = adaptersDefNodes.GetEnumerator();
        DataServices dataServices = orbConfig.GetDataServices();

        while ( en.MoveNext() )
          {
          XmlElement adapterDefinition = (XmlElement)en.Current;
          string id = XmlUtil.GetAttributeText( adapterDefinition, "id" );
          string type = XmlUtil.GetAttributeText( adapterDefinition, "class" );

          if ( type == null )
            type = XmlUtil.GetAttributeText( adapterDefinition, "type" );

          if ( type == null || type.Trim().Length == 0 )
            continue;

          string defaultAdapterStr = XmlUtil.GetAttributeText( adapterDefinition, "default" );
          bool defaultAdapter = defaultAdapterStr != null && defaultAdapterStr.ToLower().Equals( "true" );

          try
            {
            IAdapter adapter = (IAdapter)orbConfig.getObjectFactories()._CreateServiceObject( type );
            dataServices._AddAdapter( id, adapter, defaultAdapter );
            }
          catch ( Exception )
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "unable to load service adapter " + type );
            }
          }

        foreach ( XmlElement destElement in destinationNodes )
          {
          string destinationId = destElement.GetAttribute( "id" );
          IDestination destination = ProcessDestination( orbConfig, destinationId, destElement );
          destination.SetName( destinationId );
          XmlElement props = (XmlElement)destElement.GetElementsByTagName( "properties" )[ 0 ];
          destination.SetProperties( parseProperties( props ) );

          if ( destination.SetConfigServiceHandler() )
            {
            if ( destination.GetServiceHandler() == null )
              destination.SetServiceHandler( serviceHandler );

            dataServices.GetDestinationManager().AddDestination( destinationId, destination );
            }
          else
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Destination is not properly configured. Destination '" + destinationId + "' cannot be registered" );
            }

          if ( this is FlexMessagingServiceConfig )
          {
            Dictionary<string, FlexMessagingDestination> flexMessagingDestinations =
              FlexMessagingServiceConfig.Instance.FlexMessagingDestinations;
            if (flexMessagingDestinations.ContainsKey(destinationId))
              flexMessagingDestinations[destinationId] =
                FlexMessagingDestination.getDestinationScope((MessagingDestination) destination);
            else
              flexMessagingDestinations.Add(destinationId,
                                            FlexMessagingDestination.getDestinationScope(
                                              (MessagingDestination) destination));
          }

            dataServices.GetDestinationManager().AddDestination( destinationId, destination );
          }
        }

      PostConfig();
      }

    private Hashtable parseProperties( XmlElement propertiesElement )
      {
      Hashtable props = new Hashtable();
      XmlNodeList propsNodes = propertiesElement.ChildNodes;

      foreach ( XmlNode xmlNode in propsNodes )
        {
        if ( !( xmlNode is XmlElement ) )
          continue;

        XmlElement xmlProperty = (XmlElement)xmlNode;

        if ( xmlProperty.ChildNodes.Count == 1 && xmlProperty.FirstChild is XmlText )
          props[ xmlProperty.Name.Trim() ] = xmlProperty.InnerText.Trim();
        else
          {
          Hashtable subProps = parseProperties( xmlProperty );

          if ( subProps.Count != 0 )
            props[ xmlProperty.Name ] = subProps;
          }
        }

      return props;
      }
    }
  }