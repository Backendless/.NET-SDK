#if (CLOUD)
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Text;
using Weborb.Messaging.Api.Scheduling;

namespace Weborb.Util.Logging.Cloud.Azure
{
    public class AzureLogJob : IScheduledJob
    {
        static AzureLog azureLog = new AzureLog();

        ICollection<String> logEntries;
        String blobLogName;

        public AzureLogJob(String blobLogName, ICollection<String> logEntries)
        {
            this.logEntries = logEntries;
            this.blobLogName = blobLogName;
        }

        #region Log file Retrieval and Saving
        public static ICollection<String> GetLog(String blobLogName)
        {
            IEnumerator<ILogEntryDataModel> iter = azureLog.RetrieveLog(blobLogName).GetEnumerator();

            ICollection<String> log = new List<String>();

            while (iter.MoveNext())
                log.Add(iter.Current.LogMessage);

            return log;
        }
        public static void ClearLog(String blobLogName)
        {
            azureLog.ClearLog(blobLogName);
        }

        private void saveLogEntries(ICollection<String> log)
        {
            IEnumerator<String> iter = log.GetEnumerator();

            while (iter.MoveNext())
                azureLog.LogEntry(blobLogName, LoggingConstants.INFO, iter.Current);

            try
              {
              AzureLog.AzureLogContext.SaveChanges( SaveChangesOptions.None );
              }
            catch( InvalidOperationException ex )
              {
              //AzureLog.AzureLogContext = new AzureLogContext();
              }
            catch( Exception ex )
              {

              }
        }
        #endregion

        #region IScheduledJob Members

        public void execute(ISchedulingService service)
        {
            saveLogEntries(logEntries);
        }

        #endregion
    }
}
#endif