using System;
using System.Collections.Generic;

namespace Weborb.Util.Logging.Cloud
{
    public interface IDataStoreLog
    {
        /// <summary>
        /// Use this method to insert a new log entry into the data store.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="category">This is the category that the log entry belongs to.</param>
        /// <param name="logMessage">This is the message that represents the log entry and will be returned 
        /// when the log is queried</param>
        void LogEntry(string instanceId, long category, string logMessage);

        /// <summary>
        /// This will retrieve the log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <returns>A collection of log messages for the specified application instance.</returns>
        ICollection<ILogEntryDataModel> RetrieveLog(string instanceId);

        /// <summary>
        /// This will retrieve the log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="category">This is the category that the log entry belongs to.</param>
        /// <returns>A collection of log messages for the specified application instance.</returns>
        ICollection<ILogEntryDataModel> RetrieveLog(string instanceId, long category);

        /// <summary>
        /// This method is used to search log entries and find all matches for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="searchText">The text that will be used for searching in the data store.</param>
        /// <returns>A collection of log messages for the specified application instance matching the search 
        /// text.</returns>
        ICollection<ILogEntryDataModel> SearchLog(string instanceId, string searchText);

        /// <summary>
        /// This will clear the entire log for the specified application instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        void ClearLog(string instanceId);

        /// <summary>
        /// This will clear the log entries that were inserted prior to the clearItemsBeforeDate for a 
        /// specified applicaiton instance.
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="clearItemsBeforeDate">This is the date that will be used to clear the log.</param>
        void ClearLog(string instanceId, DateTime clearItemsBeforeDate);

        /// <summary>
        /// This will clear all of the log entries except for the first x number of logs (where x is the value 
        /// specified for logEntriesToKeep) for a specified application instance.  
        /// </summary>
        /// <param name="instanceId">This is the ID of the application that owns the log.</param>
        /// <param name="logEntriesToKeep">The number of log entries that will be kept.</param>
        void ClearLog(string instanceId, int logEntriesToKeep);
    }
}
