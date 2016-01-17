using System;
using System.Collections;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	/// <summary>
	/// 
	/// </summary>
	public class DatePolicy : ILoggingPolicy
	{
		private static readonly string policyName = "Date Based Logging";
		private ILogger logger;
		private Hashtable policyParameters;

		public DatePolicy( Hashtable policyParameters )
		{
			if( Log.isLogging( LoggingConstants.DEBUG ) )
				Log.log( LoggingConstants.DEBUG, "creating DatePolicy" );

			this.policyParameters = policyParameters;
			this.logger = new DateLogger();
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
