using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections;
using System.IO;

using Weborb.Security;
using Weborb.Util.License;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.V3Types.Core;
#if CLOUD
using Weborb.Cloud;
#endif

namespace Weborb.Config
{
	public class FlexRemotingServiceConfig : BaseFlexConfig
	{
        protected const string REMOTINGSERVICE_FILE = "remoting-config.xml";
        public static List<string> destinationsIds = new List<string>();
    		//private static FileSystemWatcher watcher;
	      protected static XmlDocument configDoc;

        public override void PreConfig()
        {
        destinationsIds.Clear();
        }

        public override string GetConfigFileName()
        {
            return REMOTINGSERVICE_FILE;
        }

        public override string  GetDefaultServiceHandlerName()
        {
            return "Weborb.V3Types.Core.RemotingHandler";
        }

        public override string GetXPath()
        {
            return "/service";
        }

        public override IDestination ProcessDestination( ORBConfig orbConfig, string destinationId, XmlElement xmlElement )
        {            
            XmlElement props = (XmlElement)xmlElement.GetElementsByTagName( "properties" )[ 0 ];
            string source = XmlUtil.GetElementText( props, "source" );
            string scope = XmlUtil.GetElementText( props, "scope" );
            Hashtable context = null;

            if( scope != null && scope.Trim().Length > 0 )
            {
                context = new Hashtable();
                context[ ORBConstants.ACTIVATION ] = scope;
            }

            if ( Log.isLogging( LoggingConstants.INFO ) )
              Log.log( LoggingConstants.INFO, "Registered Flex Remoting destination - " + destinationId );
            orbConfig.GetServiceRegistry()._AddMapping( destinationId, source, context );

            XmlNodeList securityNodes = xmlElement.GetElementsByTagName( "security" );

            if( securityNodes != null && securityNodes.Count > 0 )
            {
                XmlElement securityElement = (XmlElement)securityNodes[ 0 ];
                XmlElement securityConstraintNode = (XmlElement)securityElement.GetElementsByTagName( "security-constraint" )[ 0 ];
                XmlElement rolesNode = (XmlElement)securityConstraintNode.GetElementsByTagName( "roles" )[ 0 ];
                XmlNodeList rolesNodeList = rolesNode.GetElementsByTagName( "role" );
                AccessConstraint constraint = new AccessConstraint( source + "_constraint", "grant" );

                foreach( XmlNode roleNode in rolesNodeList )
                    constraint.AddRole( roleNode.InnerText );

                ORBSecurity security = orbConfig.getSecurity();
                security.getConstraintsList()[ constraint.name ] = constraint;
                security.SecureResource( source, new string[] { constraint.name }, null );
            }

            destinationsIds.Add( destinationId );
            return new RemotingDestination( destinationId, source );            
        }

        public override void PostConfig()
        {
            /*
            if( watcher == null )
            {
                watcher = new FileSystemWatcher( basePath, REMOTINGSERVICE_FILE );
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += new FileSystemEventHandler( OnChanged );
                watcher.EnableRaisingEvents = true;
            }
             */ 
        }

        public static XmlDocument GetConfigDoc()
        {
          if( configDoc != null )
            return configDoc;

          configDoc = new XmlDocument( );
#if CLOUD
          configDoc.Load( AzureUtil.BlobToStream( "WEB-INF/flex/" + REMOTINGSERVICE_FILE ) );
#else
          configDoc.Load( Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), REMOTINGSERVICE_FILE ) );
#endif
          return configDoc;
        }

        public static void SaveConfig()
        {	  
#if CLOUD
          MemoryStream stream = new MemoryStream();
          configDoc.Save( stream );
          stream.Position = 0;
          AzureUtil.GetBlob( "WEB-INF/flex/" + REMOTINGSERVICE_FILE ).UploadFromStream( stream );
#else  		    
          configDoc.Save( Path.Combine( ORBConfig.GetInstance().GetFlexConfigPath(), REMOTINGSERVICE_FILE ) );
#endif
        }

    		private void OnChanged( object source, FileSystemEventArgs evt )
    		{
    			Configure( evt.FullPath, orbConfig );
    		}
	}
}
