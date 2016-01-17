using System;
using System.Xml;
using System.Configuration;
using Weborb.Activation;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
	/// <summary>
	/// 
	/// </summary>
	public class ActivatorsConfigHandler : ORBConfigHandler
	{
		public override object Configure( object parent, object configContext, XmlNode section )
		{
			XmlNodeList activators = section.ChildNodes;

			foreach( XmlNode node in activators )
				if( node.Name.Equals( "activator" ) )
					setupActivator( node );

			return this;
		}

		private void setupActivator( XmlNode activatorNode )
		{
			XmlNodeList activatorInfo = activatorNode.ChildNodes;
			string activationModeName = null;
			string activatorClassName = null;
            
			foreach( XmlNode info in activatorInfo )
			{
				if( info.Name.Equals( "activationModeName" ) )
					activationModeName = info.InnerText.Trim();
				else if( info.Name.Equals( "className" ) )
					activatorClassName = info.InnerText.Trim();
			}

			object activator = null;

			try
			{
				activator = getORBConfig().getObjectFactories()._CreateServiceObject( activatorClassName );
			}
			catch( Exception exception )
			{
				if( Log.isLogging( LoggingConstants.ERROR ) )
					Log.log( LoggingConstants.ERROR, "Unable to instantiate activator with class name " + activatorClassName, exception );

				return;
			}
			if( activator is IActivator )
                getORBConfig().getActivators()._AddActivator( activationModeName, (IActivator) activator );
			else if( Log.isLogging( LoggingConstants.ERROR ) )
				Log.log( LoggingConstants.ERROR, "Unable to add activator. Activator with class name " + activatorClassName + " does not implement the Weborb.Activation.IActivator interface");
		}
	}
}
