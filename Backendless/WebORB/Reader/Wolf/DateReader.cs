using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class DateReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read(XmlElement element, ParseContext parseContext)
        {
            double ticks = double.Parse( element.InnerText.Trim() );
            DateTime oldDate = new DateTime( 1970, 1, 1 );
            DateTime correctDate = oldDate.AddMilliseconds( ticks );
            return new DateType( correctDate );
        }

        #endregion
    }
}
