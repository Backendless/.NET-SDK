#if (CLOUD)
using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Text;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Specialized;
using System.IO;
using Weborb.Writer;
using Weborb.Writer.Amf;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Messaging.Server.Scheduling;
using Weborb.Messaging.Api.Scheduling;
using System.Reflection;
using System.Collections.Generic;
using Weborb.Util.Cloud;

namespace Weborb.Util.Logging.Cloud.Azure
  {
  public class AzureTraceLogger : AbstractLogger
    {
    public override void fireEvent( String category, Object eventObject, DateTime timestamp )
      {
      if( eventObject is ExceptionHolder )
        eventObject = ( (ExceptionHolder) eventObject ).ExceptionObject.ToString();

      string logEntry = "[Thread-" + Thread.CurrentThread.ManagedThreadId + "] " + category + ":" + timestamp +
                        ":" + eventObject;

      Trace.WriteLine( logEntry, category );
      }
    }
  }
#endif