#if (CLOUD)
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Web;
using System.Data.Services.Client;
using System.Collections;
using System.Linq;
using Weborb.Util.Cloud;

namespace Weborb.Util.Logging.Cloud.Azure
{
    public class AzureLog : IDataStoreLog
    {
        #region Constructors
        public AzureLog()
        { }
        #endregion

        #region IDataStoreLog Members
        /// <summary>
        /// Use this method to insert a new log entry into the data store.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="category">This is the category that the log entry belongs to.</param>
        /// <param name="logMessage">This is the message that represents the log entry and will be returned 
        /// when the log is queried</param>
        public void LogEntry(string instanceId, long category, string logMessage)
        {
            AzureLogEntry logEntry = new AzureLogEntry(instanceId, category, logMessage);

            AzureLogContext.AddObject(AzureLogContext.AZURE_LOG_TABLE_NAME, logEntry);
        }

        /// <summary>
        /// This will retrieve the log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <returns>A collection of log messages for the specified application instance.</returns>
        public ICollection<ILogEntryDataModel> RetrieveLog(string instanceId)
        {
            var results = from c in AzureLogContext.AzureLogTable
                          where c.InstanceID == instanceId
                          //orderby c.Timestamp
                          select c;

            CloudTableQuery<AzureLogEntry> query =
                new CloudTableQuery<AzureLogEntry>( results as DataServiceQuery<AzureLogEntry> );
            IEnumerable<AzureLogEntry> queryResults = query.Execute();

            IEnumerator<AzureLogEntry> iter = queryResults.GetEnumerator();

            ICollection<ILogEntryDataModel> log = new List<ILogEntryDataModel>();

            while (iter.MoveNext())
                log.Add(iter.Current);

            return log;
        }

        /// <summary>
        /// This will retrieve the log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="category">This is the category that the log entry belongs to.</param>
        /// <returns>A collection of log messages for the specified application instance.</returns>
        public ICollection<ILogEntryDataModel> RetrieveLog(string instanceId, long category)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to search log entries and find all matches for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="searchText">The text that will be used for searching in the data store.</param>
        /// <returns>A collection of log messages for the specified application instance matching the search 
        /// text.</returns>
        public ICollection<ILogEntryDataModel> SearchLog(string instanceId, string searchText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will clear the entire log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        public void ClearLog(string instanceId)
        {
            ICollection<ILogEntryDataModel> log = RetrieveLog(instanceId);

            IEnumerator<ILogEntryDataModel> iter = log.GetEnumerator();

            while (iter.MoveNext())
            {
                //AzureLogContext.AttachTo(AzureLogContext.AZURE_LOG_TABLE_NAME, iter.Current);
                AzureLogContext.DeleteObject(iter.Current);
                AzureLogContext.SaveChangesWithRetries();
            }
        }

        /// <summary>
        /// This will clear the log entries that were inserted prior to the clearItemsBeforeDate for a 
        /// specified applicaiton instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="clearItemsBeforeDate">This is the date that will be used to clear the log.</param>
        public void ClearLog(string instanceId, DateTime clearItemsBeforeDate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will clear all of the log entries except for the first x number of logs (where x is the value 
        /// specified for logEntriesToKeep) for a specified application instance.  
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="logEntriesToKeep">The number of log entries that will be kept.</param>
        public void ClearLog(string instanceId, int logEntriesToKeep)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Initialization Functionality
        static bool _initialized = false;
        static object _lock = new Object();

        public static void Initialize(HttpContext context)
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                handleInitialization(context);
                _initialized = true;
            }
        }

        static void handleInitialization(HttpContext context)
        {
            AzureLogContext = new AzureLogContext();
            AzureLogContext.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));

            // This will create tables for all public properties that are IQueryable (collections)
            CloudTableClient.CreateTablesFromModel( typeof(AzureLogContext), 
                        AzureUtil.StorageAccount.TableEndpoint.AbsoluteUri, AzureUtil.StorageAccount.Credentials);
        }
        #endregion

        #region Service Context API
        public static AzureLogContext AzureLogContext;
        #endregion
    }
}
#endif
