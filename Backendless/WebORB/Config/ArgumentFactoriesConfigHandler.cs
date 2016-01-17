using System;
using System.Configuration;
using System.Xml;

using Weborb.Util;

namespace Weborb.Config
{
  public class ArgumentFactoriesConfigHandler : ORBConfigHandler
  {
    public override object Configure( object parent, object configContext, XmlNode section )
    {
      foreach( XmlNode node in section.ChildNodes )
        if( node.Name.Equals( "argumentFactory" ) )
          ParseArgumentFactory( (XmlElement) node );

      return this;
    }

    private void ParseArgumentFactory( XmlElement node )
    {
      string factoryClassName = node.GetElementsByTagName( "argumentFactoryClassName" )[ 0 ].InnerText.Trim();
      string className = node.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim();

      if( factoryClassName.Length > 0 )
        getORBConfig().getObjectFactories().AddArgumentObjectFactory( className, CreateArgumentFactory( factoryClassName ) );
    }

    public void AddArgumentFactory( string argumentFactoryTypeName, string argumentTypeName )
    {
      getORBConfig().getObjectFactories().AddArgumentObjectFactory( argumentTypeName, CreateArgumentFactory( argumentFactoryTypeName ) );
      XmlNode configNode = GetConfigNode();
      XmlElement argumentFactoryElement = configNode.OwnerDocument.CreateElement( "argumentFactory" );
      XmlElement factoryTypeNameElement = configNode.OwnerDocument.CreateElement( "argumentFactoryClassName" );
      XmlElement typeNameElement = configNode.OwnerDocument.CreateElement( "className" );

      factoryTypeNameElement.InnerText = argumentFactoryTypeName;
      typeNameElement.InnerText = argumentTypeName;

      argumentFactoryElement.AppendChild( factoryTypeNameElement );
      argumentFactoryElement.AppendChild( typeNameElement );

      configNode.AppendChild( argumentFactoryElement );
      SaveConfig();
    }

    public void RemoveServiceFactoryFor( string argumentTypeName )
    {
      getORBConfig().getObjectFactories().RemoveArgumentFactoryFor( argumentTypeName );
      XmlNode configNode = GetConfigNode();

      foreach( XmlNode node in configNode.ChildNodes )
      {
        if( !node.Name.Equals( "argumentFactory" ) )
          continue;

        XmlElement element = (XmlElement) node;

        if( element.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim().Equals( argumentTypeName ) )
        {
          configNode.RemoveChild( node );
          break;
        }
      }

      SaveConfig();
    }

    private IArgumentObjectFactory CreateArgumentFactory( string argumentFactoryTypeName )
    {
      object factory = getORBConfig().getObjectFactories()._CreateServiceObject( argumentFactoryTypeName );

      if( factory is IArgumentObjectFactory )
        return (IArgumentObjectFactory) factory;
      else
        throw new ConfigurationException( "invalid configuration. argument factory class must implement the Weborb.Util.IArgumentObjectFactory interface" );
    }
  }
}
