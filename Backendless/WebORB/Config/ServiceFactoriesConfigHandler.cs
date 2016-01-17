using System;
using System.Configuration;
using System.Xml;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
	public class ServiceFactoriesConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
			foreach( XmlNode node in section.ChildNodes )
				if( node.Name.Equals( "serviceFactory" ) )
					ParseServiceFactory( (XmlElement) node );

			return this;
		}

		private void ParseServiceFactory( XmlElement node )
		{
			string factoryClassName = node.GetElementsByTagName( "serviceFactoryClassName" )[ 0 ].InnerText.Trim();
			string className = node.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim();

			if( factoryClassName.Length > 0 )
				InstallFactory( factoryClassName, className );
		}

		public void AddServiceFactory( string serviceFactoryTypeName, string serviceTypeName )
		{
			InstallFactory( serviceFactoryTypeName, serviceTypeName );

			XmlNode configNode = GetConfigNode();
			XmlElement serviceFactoryElement = configNode.OwnerDocument.CreateElement( "serviceFactory" );
			XmlElement factoryTypeNameElement = configNode.OwnerDocument.CreateElement( "serviceFactoryClassName" );
			XmlElement typeNameElement = configNode.OwnerDocument.CreateElement( "className" );

			factoryTypeNameElement.InnerText = serviceFactoryTypeName;
			typeNameElement.InnerText = serviceTypeName;

			serviceFactoryElement.AppendChild( factoryTypeNameElement );
			serviceFactoryElement.AppendChild( typeNameElement );

			configNode.AppendChild( serviceFactoryElement );
			SaveConfig();
		}

		public void InstallFactory( string serviceFactoryTypeName, string serviceTypeName )
		{
      object factory;

      try
      {
        factory = getORBConfig().getObjectFactories()._CreateServiceObject( serviceFactoryTypeName );
      }
      catch( Exception )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, String.Format( "Unable to create service factory class - {0}. Factory mapping will be ignored", serviceFactoryTypeName ) );
        
        return;
      }

			if( factory is IServiceObjectFactory )
				getORBConfig().getObjectFactories().AddServiceObjectFactory( serviceTypeName, (IServiceObjectFactory) factory );
			else
				throw new ConfigurationException( "invalid configurarion. service factory class must implement the Weborb.Util.IServiceObjectFactory interface" );
		}

		public void RemoveServiceFactoryFor( string typeName )
		{
			getORBConfig().getObjectFactories().RemoveServiceFactoryFor( typeName );
			XmlNode configNode = GetConfigNode();

			foreach( XmlNode node in configNode.ChildNodes )
			{
				if( !node.Name.Equals( "serviceFactory" ) )
					continue;

				XmlElement element = (XmlElement) node;

				if( element.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim().Equals( typeName ) )
				{
					configNode.RemoveChild( node );
					break;					
				}
			}
		}			
	}
}
