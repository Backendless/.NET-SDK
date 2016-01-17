using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class BooleanReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read( XmlElement element, ParseContext parseContext )
        {
            string booleanValue = element.InnerText.Trim().ToLower();
            return new BooleanType( booleanValue.Equals( "true" ) );
        }

        #endregion
    }
}
