using System;
using System.Collections;
using System.IO;
using System.Xml;

using Weborb.Data;
using Weborb.Util;
using Weborb.Util.License;
using Weborb.Util.Logging;
using Weborb.V3Types.Core;

namespace Weborb.Config
{
	public class FlexDataServiceConfig : BaseFlexConfig
	{
        public override string GetConfigFileName()
        {
            return "data-management-config.xml";
        }

        public override string GetDefaultServiceHandlerName()
        {
            return "Weborb.Data.DataServiceHandler";
        }

        public override string GetXPath()
        {
            return "/service";
        }

        public override IDestination ProcessDestination( ORBConfig orbConfig, string destinationId, XmlElement destinationNode )
        {
            XmlElement propertiesNode = (XmlElement)destinationNode.GetElementsByTagName( "properties" )[ 0 ];

            string adapter = XmlUtil.GetAttributeText( destinationNode, "adapter", "ref" );
            IDataServiceAdapter dataServiceAdapter = (IDataServiceAdapter) orbConfig.GetDataServices()._GetAdapter( adapter );
            Destination destination = dataServiceAdapter.GetConfigurator().Configure( destinationId, propertiesNode );
            destination.Id = destinationId;
            destination.Adapter = dataServiceAdapter;
            return destination;
        }

        public override void PreConfig()
        {
        }

        public override void PostConfig()
        {
        }
	}
}
