#if (CLOUD)
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Weborb.Util.Logging.Cloud.Azure
  {
    public class AzureLogEntry : TableServiceEntity, ILogEntryDataModel
      {
      #region Fields
      protected Object _instanceId;
      protected DateTime _timeStamp;
      protected String _logMessage;
      protected long _category;
      #endregion

      #region Constructors

      public AzureLogEntry()
        : this( String.Empty, LoggingConstants.INFO, String.Empty, DateTime.UtcNow.ToString( "MMddyyyy" ),
                string.Format( "{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid() ) )
        {
        }

      public AzureLogEntry(string instanceId, long category, string logMessage)
          : this( instanceId, category, logMessage, DateTime.UtcNow.ToString( "MMddyyyy" ), 
                  string.Format( "{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid() ) )
        {
        }

      public AzureLogEntry(string instanceId, long category, string logMessage, string partitionKey, string rowKey)
          : base(partitionKey, rowKey)
        {
        InstanceID = instanceId;
        LogMessage = logMessage;
        Category = category;
        }
      #endregion

      #region ILogEntryDataModel Members
      /// <summary>
      /// This is the ID of the Azure Application that is logging the entry.
      /// </summary>
      public string InstanceID { get; set; }

      /// <summary>
      /// This is the message which represents the actual contents of the log.
      /// </summary>
      public string LogMessage { get; set; }

      /// <summary>
      /// This is the code for the category and should be retrieved via a call to Log.getCode(...).
      /// </summary>
      public long Category { get; set; }
      #endregion
      }
  }
#endif