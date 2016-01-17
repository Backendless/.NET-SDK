using System;
using System.Xml;
using System.Configuration;

namespace Weborb.Config
{
	public class AlternateConfigPathHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
			return section.InnerText.Trim();
		}
	}
}
