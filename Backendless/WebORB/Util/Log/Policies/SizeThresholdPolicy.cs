using System;
using System.Collections;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	/// <summary>
	/// 
	/// </summary>
	public class SizeThresholdPolicy : ILoggingPolicy
	{
		private static readonly string policyName = "File Size Threshold";
		private ILogger logger;
		private Hashtable policyParameters;

		public SizeThresholdPolicy( Hashtable policyParameters )
		{	
			if( Log.isLogging( LoggingConstants.DEBUG ) )
				Log.log( LoggingConstants.DEBUG, "creating SizeThresholdPolicy" );

			this.logger = new SizeThresholdLogger( Convert.ToInt32( (string)policyParameters[ "fileSize" ] ), (string)policyParameters[ "fileName" ] );
   			this.policyParameters = policyParameters;
		}

		#region ILoggingPolicy Members

		public ILogger getLogger()
		{
			return logger;
		}

		public string getPolicyName()
		{
			return policyName;
		}

		public Hashtable getPolicyParameters()
		{
			return policyParameters;
		}

		#endregion
	}
}
