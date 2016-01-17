using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class XmlReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read( XmlElement element, ParseContext parseContext )
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml( element.InnerXml );
            return new XmlType( xmlDoc );
        }

        #endregion
    }
}
