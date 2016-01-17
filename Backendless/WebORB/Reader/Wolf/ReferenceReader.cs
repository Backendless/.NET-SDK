using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class ReferenceReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read(XmlElement element, ParseContext parseContext)
        {
            int refID = int.Parse( element.InnerText.Trim() );
            return parseContext.getReference( refID );
        }

        #endregion
    }
}
