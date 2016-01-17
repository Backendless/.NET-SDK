#if (CLOUD)
using System;
using System.Collections;
using Weborb.Util.Logging;
using Weborb.Util.Logging.Cloud.Azure;

namespace Weborb.Util.Logging.Policies
{
	public class AzureLoggingPolicy : ILoggingPolicy
	{
        private ILogger logger;
        private Hashtable policyParameters;

        public AzureLoggingPolicy(Hashtable policyParameters)
		{
            this.logger = new AzureLogger();
            this.policyParameters = policyParameters;
        }

        #region ILoggingPolicy Members

        public ILogger getLogger()
        {
            return this.logger;                           
        }

        public string getPolicyName()
        {
            return "Azure Logging";
        }

        public Hashtable getPolicyParameters()
        {
            return policyParameters;
        }

        #endregion
    }
}
#endif