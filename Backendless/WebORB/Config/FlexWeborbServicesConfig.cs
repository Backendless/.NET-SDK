using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Config
{
    class FlexWeborbServicesConfig : FlexRemotingServiceConfig
    {
        public override string GetConfigFileName()
        {
            return "weborb-services-config.xml";
        }

        public override string GetDefaultServiceHandlerName()
        {
            return null;
        }

        public override void PreConfig()
        {

        }

        public override void PostConfig()
        {

        }

        public override string GetXPath()
        {
            return "/services-config/services/service";
        }
    }
}
