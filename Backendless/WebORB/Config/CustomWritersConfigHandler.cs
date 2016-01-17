using System;
using System.Configuration;
using System.Xml;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Writer;

namespace Weborb.Config
{
	public class CustomWritersConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
            /*if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            {
                if( Log.isLogging( LoggingConstants.INFO ) )
                    Log.log( LoggingConstants.INFO, "skipping custom serializer initialization. The feature is not available in WebORB Standard Edition" );

                return this;
            }*/

			foreach( XmlNode node in section.ChildNodes )
				if( node.Name.Equals( "customWriter" ) )
					ParseWriter( (XmlElement) node );

			return this;
		}

		private void ParseWriter( XmlElement element )
		{
			string writerTypeName = element.GetElementsByTagName( "writerClassName" )[ 0 ].InnerText.Trim();
			string className = element.GetElementsByTagName( "className" )[ 0 ].InnerText.Trim();

			if( writerTypeName.Length > 0 )
				InitializeWriter( writerTypeName, className );
		}

		public void InitializeWriter( string writerTypeName, string className )
		{
			object writerObject = getORBConfig().getObjectFactories()._CreateServiceObject( writerTypeName );
			Type mappedType = TypeLoader.LoadType( className );

			if( writerObject is ITypeWriter )
				MessageWriter.AddAdditionalTypeWriter( mappedType, (ITypeWriter) writerObject );
			else
				throw new ConfigurationException( "invalid configuration. custom writers must implement the Weborb.Writer.ITypeWriter interface" );
		}
	}
}
