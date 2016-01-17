using System;
using System.Xml;
using System.Collections;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Protocols.Wolf;

namespace Weborb.Reader.Wolf
{
	public class ObjectReader : IXMLTypeReader
	{
        #region IXMLTypeReader Members

        public IAdaptingType read( XmlElement element, ParseContext parseContext )
        {
            int refID = -1;
            
            if( element.HasAttribute( "referenceID" ) )
                refID = int.Parse( element.GetAttribute( "referenceID" ) );

            // reserve place for object in parseContext
            int parsedObjectsIndex = parseContext.addParsedObject( null );

            Hashtable properties = new Hashtable();
            RequestParser xmlRequestParser = RequestParser.GetInstance();

            XmlNodeList xmlNodes = element.SelectNodes( "Field" );

            foreach( XmlNode xmlNode in xmlNodes )
            {
                string fieldName = null;
                IAdaptingType fieldValue = null;

                foreach( XmlNode fieldNode in xmlNode.ChildNodes )
                {
                    switch( fieldNode.Name )
                    {
                        case "Name":
                            fieldName = fieldNode.InnerText.Trim();
                            break;

                        case "Value":
                            fieldValue = xmlRequestParser.ParseElement( fieldNode.FirstChild, parseContext );
                            break;
                    }
                }

                properties[ fieldName ] = fieldValue;
            }

            IAdaptingType obj = new AnonymousObject( properties );

            string objectName = element.GetAttribute( "objectName" );

            if( objectName != null && objectName.Trim().Length != 0 )
                obj = new NamedObject( objectName, obj );

            if( refID != -1 )
                parseContext.addReference( obj, refID );

            parseContext.setParsedObject( parsedObjectsIndex, obj );
            
            return obj;
        }

        #endregion
    }
}
