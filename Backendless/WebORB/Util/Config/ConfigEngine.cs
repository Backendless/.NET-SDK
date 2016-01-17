using System;
using System.Collections;
using System.Threading;
using System.Xml;
using System.IO;
using System.Configuration;
using Weborb.Config;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Util.Config
{
  public class ConfigEngine
  {
    public static Hashtable Configure( ORBConfig config, string filename )
    {
      return Configure( config, filename, null );
    }

    public static Hashtable Configure( ORBConfig config, string filename, ArrayList sectionsToProcess )
    {
      if( !File.Exists( filename ) )
      {
        System.Console.WriteLine( "Unable to find configuration file at " + filename );
        return new Hashtable();
      }

      Hashtable configObjects = new Hashtable();

      using( XmlTextReader configReader = new XmlTextReader( filename ) )
      {
        XmlDocument configDoc = new XmlDocument();
        configDoc.PreserveWhitespace = false;
        while ( true )
        {
          try
          {
            configDoc.Load(configReader);
            break;
          }
          catch(IOException e)
          {
            Thread.Sleep(4000);
          }
        }
        XmlElement root = configDoc.DocumentElement;
        
        if ( root == null )
          return Configure(config, filename, sectionsToProcess);

        XmlNode configSections = root.GetElementsByTagName( "configSections" )[ 0 ];

        foreach( XmlNode section in configSections.ChildNodes )
        {
          if( section is XmlWhitespace )
            continue;

          string sectionName = section.Attributes[ "name" ].Value.Trim();

          Hashtable handlers = ParseSection( config, section.ChildNodes, sectionsToProcess );
          XmlNodeList configSection = root.GetElementsByTagName( sectionName );

          if( configSection.Count > 1 )
            throw new ConfigurationException( "invalid configuration in weborb.config. Found more than one section with name " + sectionName );

          ProcessSection( config, sectionName, configSection[ 0 ].ChildNodes, handlers, configObjects, sectionsToProcess );
        }
      }

      return configObjects;
    }

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
            Log.log( LoggingConstants.ERROR, "Cannot find a configuration handler for element " + sectionName + " in section " + node.ParentNode.Name );

          continue;
        }

        IConfigurationSectionHandler handler = (IConfigurationSectionHandler) sectionHandlers[ sectionName ];
        object configObject = handler.Create( null, handler is ORBConfigHandler ? config : null, node );

        if( configObject != null )
          configObjects[ configSectionName + "/" + sectionName ] = configObject;
      }
    }
  }
}
