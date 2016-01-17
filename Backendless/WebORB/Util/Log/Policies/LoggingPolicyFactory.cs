using System;
using System.Collections;
using System.Reflection;
using Weborb.Util;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	public class LoggingPolicyFactory : IArgumentObjectFactory
	{
		private static Hashtable POLICY_TYPES = new Hashtable();
		private static Type[] METHOD_ARGS = new Type[] { typeof( Hashtable ) };

		static LoggingPolicyFactory()
		{
			POLICY_TYPES.Add( "weborb.util.log.policies.SpecificFilePolicy", "SpecificFile" );
			POLICY_TYPES.Add( "weborb.util.log.policies.DatePolicy", "Date" );
			POLICY_TYPES.Add( "weborb.util.log.policies.SizeThresholdPolicy", "SizeThreshold" );
            POLICY_TYPES.Add( "flashorb.util.log.policies.SpecificFilePolicy", "SpecificFile" );
            POLICY_TYPES.Add( "flashorb.util.log.policies.DatePolicy", "Date" );
            POLICY_TYPES.Add( "flashorb.util.log.policies.SizeThresholdPolicy", "SizeThreshold" );
        }
	
		#region IArgumentObjectFactory Members

		public object createObject( IAdaptingType argument )
		{
            Hashtable properties = (Hashtable) argument.defaultAdapt();
			string className = (string) properties[ "_orbclassname" ];
			string methodName = (string) POLICY_TYPES[ className ];            
			return getPolicyObject( methodName, properties );
		}

		#endregion

		private object getPolicyObject( string methodName, Hashtable restrictionData )
		{
			MethodInfo method = GetType().GetMethod( methodName, BindingFlags.Instance | BindingFlags.Public, null, METHOD_ARGS, null ); 
			return method.Invoke( this, new object[] { restrictionData } );
		}

		public ILoggingPolicy SpecificFile( Hashtable restrictionData )
		{
			Hashtable initParams = new Hashtable();
			initParams.Add( "fileName", restrictionData[ "fileName" ] );
			return new SpecificFilePolicy( initParams );
		}

        public ILoggingPolicy Date( Hashtable restrictionData )
		{
			return new DatePolicy( new Hashtable() );
		}

        public ILoggingPolicy SizeThreshold( Hashtable restrictionData )
		{
			Hashtable initParams = new Hashtable();
			initParams.Add( "fileSize", restrictionData[ "sizeThreshold" ].ToString() );
			initParams.Add( "fileName", restrictionData[ "fileName" ].ToString() );
			return new SizeThresholdPolicy( initParams );
		}
	}
}
