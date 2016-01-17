using System;
using System.Xml;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
	public class ClassMappingsHandler : ORBConfigHandler
	{
        public override object Configure( object parent, object configContext, XmlNode section )
        {
            foreach( XmlNode node in section.ChildNodes )
                if( node.Name.Equals( "classMapping" ) )
                    ParseMapping( (XmlElement) node );

            return this;
        }

        private void ParseMapping( XmlElement element )
        {
            string clientClassName = element.GetElementsByTagName( "clientClass" )[ 0 ].InnerText.Trim();
            string serverClassName = element.GetElementsByTagName( "serverClass" )[ 0 ].InnerText.Trim();

            if( clientClassName.Length != 0 && serverClassName.Length != 0 )
            {
                Type serverType = null;

                try
                {
                    serverType = TypeLoader.LoadType( serverClassName );
                }
                catch( Exception )
                {
                    if( Log.isLogging( LoggingConstants.ERROR ) )
                        Log.log( LoggingConstants.ERROR, "server type for a class mapping cannot be found " + serverClassName );

                    return;
                }

                if( serverType != null )
                    getORBConfig().getTypeMapper()._AddClientClassMapping( clientClassName, serverType );
                else
                    if( Log.isLogging( LoggingConstants.ERROR ) )
                        Log.log( LoggingConstants.ERROR, "server type for a class mapping cannot be found " + serverClassName );
            }
        }
	}
}
