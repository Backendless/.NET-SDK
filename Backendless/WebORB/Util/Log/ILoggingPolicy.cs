using System;
using System.Collections;

namespace Weborb.Util.Logging
{
	public interface ILoggingPolicy
	{
		ILogger getLogger();
		string getPolicyName();
		Hashtable getPolicyParameters();
	}
}
