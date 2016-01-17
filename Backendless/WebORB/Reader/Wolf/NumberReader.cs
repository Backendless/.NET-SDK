using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
  public class NumberReader : IXMLTypeReader
  {
    #region IXMLTypeReader Members

    public IAdaptingType read( XmlElement element, ParseContext parseContext )
    {
      string value = element.InnerText.Trim().ToLower();
      double number = value == "nan" ? 0 : double.Parse( value );
      return new NumberObject( number );
    }

    #endregion
  }
}
