using System;
using System.Threading;

using BackendlessAPI.Async;
using BackendlessAPI.Utils;
using BackendlessAPI.Engine;
using Weborb.Util.Logging;

namespace BackendlessAPI.RT
{
  internal class RTLookupService
  {
    private readonly ResultHandler<ReconnectAttempt> reconnectAttemptListener;
    private readonly ITimeoutManager timeOutManager;

    internal RTLookupService(ResultHandler<ReconnectAttempt> reconnectAttemptListener, ITimeoutManager timeOutManager)
    {
      this.reconnectAttemptListener = reconnectAttemptListener;
      this.timeOutManager = timeOutManager;
    }

    internal String Lookup()
    {
      try
      {
        String rtServer = Invoker.InvokeSync<String>("com.backendless.rt.RTService", "lookup", new Object[] { });
        return rtServer;
      }
      catch (System.Exception e)
      {
        Log.log(Backendless.BACKENDLESSLOG, String.Format("Lookup failed {0}", e));
        int retryTimeout = timeOutManager.NextTimeout();
        Log.log(Backendless.BACKENDLESSLOG, String.Format("Wait before lookup for {0}", retryTimeout));
        Thread.Sleep(retryTimeout);

        reconnectAttemptListener?.Invoke(new ReconnectAttempt(timeOutManager.RepeatedTimes(), retryTimeout, e.ToString()));

        return Lookup();
      }
    }
  }
}
