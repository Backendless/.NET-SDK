#if (CLOUD)
using System;
using System.Collections;
using Weborb.Util.Logging;

namespace Weborb.Util.Logging.Policies
{
	public class AzureTraceLoggingPolicy : ILoggingPolicy
	{
        private ILogger logger;
        private Hashtable policyParameters;

        public AzureTraceLoggingPolicy( Hashtable policyParameters )
		    {
            this.logger = new AzureTraceLogger();
            this.policyParameters = policyParameters;
        }

        #region ILoggingPolicy Members

        public ILogger getLogger()
        {
            return this.logger;                           
        }

        public string getPolicyName()
        {
            return "Azure Trace Logging";
        }

        public Hashtable getPolicyParameters()
        {
            return policyParameters;
        }

        #endregion
    }
}
#endif
