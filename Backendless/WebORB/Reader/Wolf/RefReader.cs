using System;
using System.Xml;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
  public class RefReader : IXMLTypeReader
  {
    #region IXMLTypeReader Members

    public IAdaptingType read( XmlElement element, ParseContext parseContext )
    {
      int refID = int.Parse( element.InnerText.Trim() );
      RefObject refObject = new RefObject( refID, parseContext.getParsedObject( refID - 1 ) );
      parseContext.RefObjects.Add( refObject );

      return refObject;
    }

    #endregion
  }
}
