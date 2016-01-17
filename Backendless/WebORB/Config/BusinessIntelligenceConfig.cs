using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Weborb.Registry;
using Weborb.Management.RBIManagement;
using Weborb.Util;
using System.Collections;
using Weborb.Management.ServiceBrowser;
using Weborb.Util.Logging;
using System.IO;

namespace Weborb.Config
{
  public class BusinessIntelligenceConfig
  {

    private XmlDocument configDoc = new XmlDocument();
    private XmlElement configElement;
    private ORBConfig orbConfig;
    private MonitoredClassRegistry registry = new MonitoredClassRegistry();
    private ServerConfiguration configuration = new ServerConfiguration();
    private string configPath;

    public string GetConfigFileName()
    {
      return "weborb.config";
    }

    public void Configure( string configPath, ORBConfig orbConfig )
    {
      this.configPath = configPath;
      this.orbConfig = orbConfig;

      if( !File.Exists( configPath ) )
      {
        if( Log.isLogging( LoggingConstants.INFO ) )
          Log.log( LoggingConstants.INFO, "File not found " + configPath );
        return;
      }

      XmlTextReader configReader = new XmlTextReader( configPath );

      try
      {
        configDoc.Load( configReader );
      }
      catch( Exception exception )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Unable to parse " + GetConfigFileName(), exception );

        return;
      }

      configReader.Close();

      XmlElement root = configDoc.DocumentElement;
      configElement = (XmlElement) root.GetElementsByTagName( ConfigConstants.BUSINESSINTELLIGENCE )[ 0 ];

      if( configElement != null )
        validate();

    }

    public void validate()
    {
      if( configElement == null )
        return;

      XmlElement monitoredServicesElement = (XmlElement) configElement.GetElementsByTagName( ConfigConstants.MONITOREDSERVICES )[ 0 ];

      if( monitoredServicesElement != null )
      {
        IList monitoredServices = new ArrayList();
        foreach( XmlNode node in monitoredServicesElement.ChildNodes )
        {
          if( node.Name == ConfigConstants.MONITOREDSERVICE )
            monitoredServices.Add( node );
        }
        IEnumerator enumerator = monitoredServices.GetEnumerator();
        registry.clear();

        while( enumerator.MoveNext() )
        {
          XmlElement monitoredService = (XmlElement) enumerator.Current;
          ServiceNode node = parseMonitoredService( monitoredService, null );

          if( node != null )
            registry.addSelectedNode( node );
        }
      }
      else if( Log.isLogging( LoggingConstants.ERROR ) )
        Log.log( LoggingConstants.ERROR, "Business intelligence settings are not properly configured. Can't find " + ConfigConstants.MONITOREDSERVICES + " tag." );

      XmlElement rbiServerConfiguration = (XmlElement) configElement.GetElementsByTagName( ConfigConstants.RBISERVERCONFIGURATION )[ 0 ];

      if( rbiServerConfiguration != null )
      {
        XmlElement serverAddressElement = (XmlElement) rbiServerConfiguration.GetElementsByTagName( "serverAddress" )[ 0 ];
        XmlElement reconnectionTimeoutElement = (XmlElement) rbiServerConfiguration.GetElementsByTagName( "reconnectionTimeout" )[ 0 ];
        XmlElement pollingTimeoutElement = (XmlElement) rbiServerConfiguration.GetElementsByTagName( "pollingTimeout" )[ 0 ];

        if( serverAddressElement != null )
          configuration.serverAddress = serverAddressElement.InnerText.Trim();

        if( reconnectionTimeoutElement != null )
        {
          String timeout = reconnectionTimeoutElement.InnerText.Trim();

          try
          {
            configuration.reconnectionTimeout = Int32.Parse( timeout );
          }
          catch( Exception )
          {
            if( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Reconnection timeout " + timeout + " can't be parsed to integer value." );
          }
        }

        if( pollingTimeoutElement != null )
        {
          String timeout = pollingTimeoutElement.InnerText.Trim();

          try
          {
            configuration.pollingTimeout = Int32.Parse( timeout );
          }
          catch( Exception )
          {
            if( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Polling timeout " + timeout + " can't be parsed to integer value." );
          }
        }
      }
      else if( Log.isLogging( LoggingConstants.ERROR ) )
        Log.log( LoggingConstants.ERROR, "Business intelligence settings are not properly configured. Can't find " + ConfigConstants.RBISERVERCONFIGURATION + " tag." );
    }

    private ServiceNode parseMonitoredService( XmlElement monitoredService, ServiceNode parent )
    {
      try
      {
        ServiceNode node = new ServiceNode();

        XmlNodeList namesList = monitoredService.GetElementsByTagName( "name" );
        XmlNode name = namesList != null && namesList.Count > 0 ? namesList[ 0 ] : null;

        node.Name = name == null ? "" : name.InnerText.Trim();
        node.Parent = parent;

        XmlNodeList items = null;
        foreach( XmlNode child in monitoredService.ChildNodes )
        {
          if( child.Name == "items" )
          {
            items = child.ChildNodes;
            break;
          }
        }

        IList children = new ArrayList();
        if( items != null )
        {
          for( int i = 0; i < items.Count; i++ )
            children.Add( items[ i ] );
        }

        for( int i = 0; i < children.Count; i++ )
        {
          ServiceNode child = parseMonitoredService( (XmlElement) children[ i ], node );

          if( child != null )
            node.AddItem( child );
        }

        XmlNode selection = null;
        foreach( XmlNode child in monitoredService.ChildNodes )
        {
          if( child.Name == "selection" )
          {
            selection = child;
            break;
          }
        }

        if( selection != null && selection.InnerText.Trim().ToLower() == "full" || items == null )
          node.Selected = ServiceNode.FULLY_SELECTED;
        else
          node.Selected = ServiceNode.PARTLY_SELECTED;

        return node;
      }
      catch( Exception )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "Can't parse monitored services for " + parent.getFullName() + " package." );

        return null;
      }
    }

    public void saveMonitoredClassRegistry()
    {
      ArrayList selectedNodes = registry.getSelectedNodes();

      XmlNodeList list = ( (XmlElement) configElement ).GetElementsByTagName( ConfigConstants.MONITOREDSERVICES );
      XmlNode node = list != null && list.Count > 0 ? list[ 0 ] : null;
      if( node != null )
        configElement.RemoveChild( node );

      XmlNode monitoredServicesElement = configDoc.CreateNode( XmlNodeType.Element, ConfigConstants.MONITOREDSERVICES, null );

      for( int i = 0; i < selectedNodes.Count; i++ )
        saveService( (ServiceNode) selectedNodes[ i ], monitoredServicesElement );

      configElement.AppendChild( monitoredServicesElement );

      saveConfig();
    }

    public void saveServerConfiguration( ServerConfiguration configuration )
    {
      this.configuration = configuration;

      XmlElement serverConfigurationElement = (XmlElement) configElement.GetElementsByTagName( ConfigConstants.RBISERVERCONFIGURATION )[ 0 ];
      XmlElement serverAddressElement = (XmlElement) serverConfigurationElement.GetElementsByTagName( "serverAddress" )[ 0 ];
      XmlElement reconnectionTimeoutElement = (XmlElement) serverConfigurationElement.GetElementsByTagName( "reconnectionTimeout" )[ 0 ];
      XmlElement pollingTimeoutElement = (XmlElement) serverConfigurationElement.GetElementsByTagName( "pollingTimeout" )[ 0 ];

      if( serverAddressElement == null )
      {
        serverAddressElement = (XmlElement) configDoc.CreateNode( XmlNodeType.Element, "serverAddress", null );
        serverConfigurationElement.AppendChild( serverAddressElement );
      }

      if( reconnectionTimeoutElement == null )
      {
        reconnectionTimeoutElement = (XmlElement) configDoc.CreateNode( XmlNodeType.Element, "reconnectionTimeout", null );
        serverConfigurationElement.AppendChild( reconnectionTimeoutElement );
      }

      if( pollingTimeoutElement == null )
      {
        pollingTimeoutElement = (XmlElement) configDoc.CreateNode( XmlNodeType.Element, "pollingTimeout", null );
        serverConfigurationElement.AppendChild( pollingTimeoutElement );
      }

      serverAddressElement.InnerText = configuration.serverAddress;
      reconnectionTimeoutElement.InnerText = "" + configuration.reconnectionTimeout;
      pollingTimeoutElement.InnerText = "" + configuration.pollingTimeout;
      saveConfig();
    }

    public ServerConfiguration getServerConfiguration()
    {
      return configuration;
    }

    private void saveService( ServiceNode node, XmlNode parent )
    {
      XmlNode monitoredService = configDoc.CreateNode( XmlNodeType.Element, ConfigConstants.MONITOREDSERVICE, null );
      XmlNode name = configDoc.CreateNode( XmlNodeType.Element, "name", null );
      XmlNode items = configDoc.CreateNode( XmlNodeType.Element, "items", null );
      XmlNode selection = configDoc.CreateNode( XmlNodeType.Element, "selection", null );
      selection.InnerText = "full";
      name.InnerText = node.Name;
      monitoredService.AppendChild( name );

      if( node.Selected == ServiceNode.PARTLY_SELECTED )
      {
        for( int i = 0; i < node.Items.Length; i++ )
          saveService( (ServiceNode) node.Items[ i ], items );

        monitoredService.AppendChild( items );
      }
      else
        monitoredService.AppendChild( selection );

      parent.AppendChild( monitoredService );
    }

    public MonitoredClassRegistry getMonitoredClassRegistry()
    {
      return registry;
    }

    private void saveConfig()
    {
      using( FileStream stream = File.Open( configPath, FileMode.Create, FileAccess.Write ) )
      {
        using( StreamWriter xmlWriter = new StreamWriter( stream, Encoding.UTF8 ) )
        {
          configDoc.Normalize();
          try
          {
            xmlWriter.Write( configDoc.InnerXml );
            xmlWriter.Flush();
          }
          catch( Exception exception )
          {
            if( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "Unable to write " + GetConfigFileName(), exception );
          }
        }
      }
    }
  }
}
