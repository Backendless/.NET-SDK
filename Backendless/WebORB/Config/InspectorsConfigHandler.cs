using System;
using System.Xml;
using System.Configuration;
using Weborb.Util;
using Weborb.Handler;
using Weborb.Util.Logging;

namespace Weborb.Config
{
	public class InspectorsConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
            /*if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            {
                Handlers handlers = getORBConfig().getHandlers();
                handlers.AddInspectionHandler( new ObjectHandler() );
                handlers.AddInspectionHandler( new WebServiceHandler() );
                return null;
            }*/

			foreach( XmlNode node in section.ChildNodes )
			{
				if( !node.Name.Equals( "serviceInspector" ) )
					continue;

				string inspectorTypeName = node.InnerText.Trim();

				if( inspectorTypeName.Length > 0 )
					ConfigureInspector( inspectorTypeName );
			}

			return this;
		}

		public void ConfigureInspector( string inspectorTypeName )
		{
			object inspectorObject;

            try
            {
                inspectorObject = getORBConfig().getObjectFactories()._CreateServiceObject( inspectorTypeName );
            }
            catch( Exception exception )
            {
                if( Log.isLogging( LoggingConstants.ERROR ) )
                    Log.log( LoggingConstants.ERROR, String.Format( "Unable to create inspection handler {0} due to exception {1}", inspectorTypeName, exception.Message ) );

                return;
            }

            if( !(inspectorObject is IInspectionHandler) )
            {
                if( Log.isLogging( LoggingConstants.ERROR ) )
                    Log.log( LoggingConstants.ERROR, "invalid inspection handler type - " + inspectorTypeName + ". Invocation handler must implement Weborb.Handler.IInspectorHandler" );

                return;
            }

			getORBConfig().getHandlers().AddInspectionHandler( (IInspectionHandler) inspectorObject );
		}

		public void AddInspector( string inspectorTypeName )
		{
			XmlNode configNode = GetConfigNode();
			XmlElement inspectorElement = configNode.OwnerDocument.CreateElement( "serviceInspector" );
			inspectorElement.InnerText = inspectorTypeName;
			configNode.AppendChild( inspectorElement );
			SaveConfig();
			ConfigureInspector( inspectorTypeName );
		}

		public void RemoveInspector( string inspectorTypeName )
		{
			XmlNode configNode = GetConfigNode();

			foreach( XmlNode node in configNode.ChildNodes )
			{
				if( !(node.Name.Equals( "serviceInspector" ) && node.InnerText.Trim().Equals( inspectorTypeName ) ) )
					continue;

				configNode.RemoveChild( node );
				break;
			}

			SaveConfig();
			getORBConfig().getHandlers().RemoveInspectionHandler( inspectorTypeName );
		}
	}
}
