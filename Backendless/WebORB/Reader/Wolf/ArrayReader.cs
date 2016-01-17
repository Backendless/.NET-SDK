using System;
using System.Collections;
using System.Xml;
using Weborb.Reader;
using Weborb.Protocols.Wolf;
using Weborb.Types;

namespace Weborb.Reader.Wolf
{
	public class ArrayReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read( XmlElement element, ParseContext parseContext )
        {
            int refID = -1;
            
            if( element.HasAttribute( "referenceID" ) )
                refID = int.Parse( element.GetAttribute( "referenceID" ) );

            ArrayList arrayElements = new ArrayList();
            RequestParser requestParser = RequestParser.GetInstance();

            foreach( XmlNode xmlNode in element.ChildNodes )
            {
                if( !(xmlNode is XmlElement) )
                    continue;

                arrayElements.Add( requestParser.ParseElement( (XmlElement) xmlNode, parseContext ) );
            }

            ArrayType array = new ArrayType( (IAdaptingType[]) arrayElements.ToArray( typeof( IAdaptingType ) ) );

            if( refID != -1 )
                parseContext.addReference( array, refID );

            return array;
        }

        #endregion
    }
}
