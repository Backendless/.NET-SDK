using System;
using System.Xml;

namespace Weborb.Util
{
	public class XmlUtil
	{
		public static string GetElementText( XmlElement element, string elementName )
		{
			XmlNodeList nodeList = element.GetElementsByTagName( elementName );

			if( nodeList == null || nodeList.Count == 0 )
				return null;

			XmlElement node = (XmlElement) nodeList[ 0 ];

			if( node == null )
				return null;

			return node.InnerText;
		}

		public static string GetAttributeText( XmlElement element, string attrName )
		{
			XmlAttribute attr = element.Attributes[ attrName ];

			if( attr == null )
				return null;

			return attr.Value;
		}

		public static string GetAttributeText( XmlElement element, string elementName, string attrName )
		{
			XmlNodeList nodeList = element.GetElementsByTagName( elementName );

			if( nodeList == null || nodeList.Count == 0 )
				return null;

			XmlElement node = (XmlElement) nodeList[ 0 ];

			if( node == null )
				return null;

			XmlAttribute attr = node.Attributes[ attrName ];

			if( attr == null )
				return null;

			return attr.Value;
		}
	}
}
