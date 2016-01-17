using System;
using System.Configuration;
using System.Xml;
using Weborb.Util.Logging;
using Weborb.Protocols;
using Weborb.Util;

namespace Weborb.Config
{
    public class ProtocolsConfigHandler : ORBConfigHandler
    {
        public override object Configure( object parent, object configContext, XmlNode section )
        {
            foreach( XmlNode node in section.ChildNodes )
            {
                if( !node.Name.Equals( "protocolHandler" ) )
                    continue;

                string handlerTypeName = node.InnerText.Trim();

                if( handlerTypeName.Length > 0 )
                    ConfigureHandler( handlerTypeName );
            }

            return this;
        }

        private void ConfigureHandler( String handlerTypeName )
        {
            if( Log.isLogging( LoggingConstants.INFO ) )
                Log.log( LoggingConstants.INFO, "registering protocol handler: " +  handlerTypeName );

            object handlerObject = getORBConfig().getObjectFactories()._CreateServiceObject( handlerTypeName );

            if( !(handlerObject is IMessageFactory) )
                throw new ConfigurationException( "invalid protocol handler type " + handlerTypeName + ". protocol handler classes must implement Weborb.Protocols.IMessageFactory" );
			
            getORBConfig().getProtocolRegistry().AddMessageFactory( (IMessageFactory) handlerObject );
        }
	}
}
