using System;

#if( !WIN8 )
using System.Configuration;
#endif
using System.Xml;
using System.Collections;
using Weborb.Registry;

namespace Weborb.Config
{
	/// <summary>
	/// Summary description for ServicesConfigHandler.
	/// </summary>
	public class ServicesConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
			foreach( XmlNode node in section.ChildNodes )
			{
                if( !(node is XmlElement) )
					continue;

				string serviceName = ((XmlElement) node).GetElementsByTagName( ORBConstants.NAME )[ 0 ].InnerText.Trim();
				string serviceId = ((XmlElement) node).GetElementsByTagName( ORBConstants.SERVICEID )[ 0 ].InnerText.Trim();
				getORBConfig().GetServiceRegistry()._AddMapping( serviceName, serviceId, new Hashtable() );
			}

			return this;
		}

	}
}
