using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	/// <summary>
	/// 
	/// </summary>
	public class SpecificFilePolicy : ILoggingPolicy
	{
		private static readonly string policyName = "Specific File";
		private ILogger logger;
		private Hashtable policyParameters;

		public SpecificFilePolicy( Hashtable policyParameters )
		{
			if( Log.isLogging( LoggingConstants.DEBUG ) )
				Log.log( LoggingConstants.DEBUG, "creating SpecificFilePolicy" );

			string fileName = (string)policyParameters[ ORBConstants.FILE_NAME ];
			this.logger = new TraceLogger( fileName );
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
