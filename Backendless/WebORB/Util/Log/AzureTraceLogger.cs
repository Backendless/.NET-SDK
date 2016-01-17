#if (CLOUD)
using System;
using System.Diagnostics;

namespace Weborb.Util.Logging
  {
  public class AzureTraceLogger : AbstractLogger
    {
    public override void fireEvent( String category, Object eventObject, DateTime timestamp )
      {
      Trace.WriteLine( format( category, eventObject, timestamp ) );
      Trace.Flush();
      }
    }
  }
#endif