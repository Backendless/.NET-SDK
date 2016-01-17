using System;
using System.Xml;
using Weborb.Util;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Config
{
	public class AbstractMappingsConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
            //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            //    return this;

			foreach( XmlNode node in section.ChildNodes )
				if( node.Name.Equals( "abstractClassMapping" ) )
					ParseMapping( (XmlElement) node );

			return this;
		}

		private void ParseMapping( XmlElement element )
		{
			string className = element.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim();
			string mappedClassName = element.GetElementsByTagName( "mappedClassName" )[ 0 ].InnerText.Trim();

			if( className.Length != 0 && mappedClassName.Length != 0 )
			{
				Type abstractType = TypeLoader.LoadType( className );
				Type mappedType = TypeLoader.LoadType( mappedClassName );

                if( abstractType == null )
                {
                    if( Log.isLogging( LoggingConstants.ERROR ) )
                        Log.log( LoggingConstants.ERROR, "Unable to find abstract type - " + className + ". Abstract mapping cannot be registered. Make sure the class library containing the type is deployed in the bin folder" );

                    return;
                }

                if( mappedType == null )
                {
                    if( Log.isLogging( LoggingConstants.ERROR ) )
                        Log.log( LoggingConstants.ERROR, "Unable to find type mapped to an abstract type. Type name - " + mappedClassName + ". Abstract mapping cannot be registered. Make sure the class library containing the type is deployed in the bin folder" );

                    return;
                }

				getORBConfig().getTypeMapper()._AddAbstractTypeMapping( abstractType, mappedType );
			}
		}
	}
}
