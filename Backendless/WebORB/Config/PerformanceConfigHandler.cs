using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Weborb.Util.License;
using Weborb.Util.Logging;

namespace Weborb.Config
{
    public class PerformanceConfigHandler : ORBConfigHandler
    {
        internal bool enableResponseBuffer = true;

        public override object Configure( object parent, object configContext, XmlNode section )
        {
            /*if( LicenseManager.GetInstance().IsStandardLicense() )
            {
                if( Log.isLogging( LoggingConstants.INFO ) )
                    Log.log( LoggingConstants.INFO, "WebORB performance control is available only in the Professional and Enterprise Editions of the product" );

                return this;
            }*/

            XmlNode responseBufferNode = ((XmlElement) section).GetElementsByTagName( "responseBuffer" )[ 0 ];
            XmlAttribute enabledAttribute = responseBufferNode.Attributes[ "enable" ];

            if( enabledAttribute != null )
            {
                string enabled = enabledAttribute.Value.ToLower().Trim();
                enableResponseBuffer = enabled.Equals( "yes" ) || enabled.Equals( "true" ) || enabled.Equals( "1" );
            }

            return this;
        }
    }
}

