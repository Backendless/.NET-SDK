using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Weborb.Management.Monitoring.Performance;
using Weborb.Util.License;
using Weborb.Util.Logging;

namespace Weborb.Config
{
    public class InstrumentationConfigHandler : ORBConfigHandler
    {
        public override object Configure( object parent, object configContext, XmlNode section )
        {
            /*if( LicenseManager.GetInstance().IsStandardLicense() )
            {
                if( Log.isLogging( LoggingConstants.INFO ) )
                    Log.log( LoggingConstants.INFO, "WebORB Instrumentation is available only in the Professional and Enterprise Editions of the product" );

                return false;
            }*/

            XmlAttribute enabledAttribute = section.Attributes[ "enable" ];

            if( enabledAttribute != null )
            {
                string enabled = enabledAttribute.Value.ToLower().Trim();

                if( enabled.Equals( "yes" ) || enabled.Equals( "true" ) || enabled.Equals( "1" ) )
                {
                    InvocationPerformanceMonitor.enablePerformanceStats();
                    return this;
                }
            }

            return this;
        }
    }
}
