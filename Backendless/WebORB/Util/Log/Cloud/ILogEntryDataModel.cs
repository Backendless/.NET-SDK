using System;

namespace Weborb.Util.Logging.Cloud
{
    public interface ILogEntryDataModel
    {
        /// <summary>
        /// This is the ID of the Application that is logging the entry.
        /// </summary>
        string InstanceID { get; set; }

        /// <summary>
        /// This is the time stamp of when the log entry was created.
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// This is the message which represents the actual contents of the log.
        /// </summary>
        String LogMessage { get; set; }

        /// <summary>
        /// This is the code for the category and should be retrieved via a call to Log.getCode(...).
        /// </summary>
        long Category { get; set; }
    }
}
