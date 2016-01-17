using System;
using System.Reflection;
using System.Xml;
using System.Configuration;
using System.Diagnostics;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Handler;

namespace Weborb.Config
{
	public class InvokersConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
            /*if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            {
                getORBConfig().getHandlers().AddInvocationHandler( new ObjectHandler() );
                getORBConfig().getHandlers().AddInvocationHandler( new WebServiceHandler() );
                return null;
            }*/

			foreach( XmlNode node in section.ChildNodes )
			{
      try
        {
        if( !node.Name.Equals( "serviceInvoker" ) )
          continue;

        string invokerTypeName = node.InnerText.Trim();

        if( invokerTypeName == "Weborb.Handler.WCFObjectHandler" && ORBUtil.GetMajorNETVersionOfWeborb() < 3 )
          continue;

        if( invokerTypeName == "Weborb.Handler.WCFRIAHandler" && ORBUtil.GetMajorNETVersionOfWeborb() < 4 )
          continue;

        if( invokerTypeName.Length > 0 )
          ConfigureInvoker( invokerTypeName );
        }
      catch ( Exception e )
        {
        if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log(LoggingConstants.EXCEPTION, "Cann't add invokation handler", e);
        }
			}

			return this;
		}

		public void ConfigureInvoker( string invokerTypeName )
		{
			if( Log.isLogging( LoggingConstants.DEBUG ) )
				Log.log( LoggingConstants.DEBUG, "adding invoker: " +  invokerTypeName );

            object invokerObject;

            try
            {
                invokerObject = getORBConfig().getObjectFactories()._CreateServiceObject( invokerTypeName );
            }
            catch( Exception exception )
            {
                if( Log.isLogging( LoggingConstants.ERROR ) )
                    Log.log( LoggingConstants.ERROR, String.Format( "Unable to create invocation handler - {0} due to exception {1}",  invokerTypeName, exception.Message ) );

                return;
            }

            if( !(invokerObject is IInvocationHandler) )
            {
                if( Log.isLogging( LoggingConstants.ERROR ) )
                    Log.log( LoggingConstants.ERROR, "invalid invocation handler type - " + invokerTypeName + ". Invocation handler must implement Weborb.Handler.IInvocationHandler" );

                return;
            }
			
			getORBConfig().getHandlers().AddInvocationHandler( (IInvocationHandler) invokerObject );
		}

		public void AddInvoker( string invokerTypeName )
		{
			XmlNode configNode = GetConfigNode();
			XmlElement invokerElement = configNode.OwnerDocument.CreateElement( "serviceInvoker" );
			invokerElement.InnerText = invokerTypeName;
			configNode.AppendChild( invokerElement );
			SaveConfig();
			ConfigureInvoker( invokerTypeName );
		}

		public void RemoveInvoker( string invokerTypeName )
		{
			XmlNode configNode = GetConfigNode();

			foreach( XmlNode node in configNode.ChildNodes )
			{
				if( !(node.Name.Equals( "serviceInvoker" ) && node.InnerText.Trim().Equals( invokerTypeName ) ) )
					continue;

				configNode.RemoveChild( node );
				break;
			}

			SaveConfig();
			getORBConfig().getHandlers().RemoveInvocationHandler( invokerTypeName );
		}

	}
}
