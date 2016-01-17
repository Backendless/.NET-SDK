using System;
using System.Collections;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	public class ConsoleLoggingPolicy : ILoggingPolicy
	{
        private ILogger logger;
        private Hashtable policyParameters;

		public ConsoleLoggingPolicy( Hashtable policyParameters )
		{
            this.logger = new ConsoleLogger();
            this.policyParameters = policyParameters;
        }

        #region ILoggingPolicy Members

        public ILogger getLogger()
        {
            return this.logger;                           
        }

        public string getPolicyName()
        {
            return "Command Line";
        }

        public Hashtable getPolicyParameters()
        {
            return policyParameters;
        }

        #endregion
    }
}
