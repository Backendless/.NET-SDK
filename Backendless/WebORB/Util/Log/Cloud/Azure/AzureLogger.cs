#if (CLOUD)
using System;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
namespace Weborb.Util.Logging
  {
  public class AzureLogger : AbstractLogger
    {
    public AzureLogger()
      {
      }

    public override void fireEvent( String category, Object eventObject, DateTime timestamp )
      {
      String eventLogName;

      switch( category )
        {
        default:
          eventLogName = "Information";
          break;
        }

      //RoleManager.WriteToLog( eventLogName, "[Thread-" + Thread.CurrentThread.ManagedThreadId + "] " +  category + ":" + timestamp + ":" + eventObject );
      //System.Console.WriteLine( category + ":" + timestamp + ":" + eventObject );
      }
    }
  }
#endif