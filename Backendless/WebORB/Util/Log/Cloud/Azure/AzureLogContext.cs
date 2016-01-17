#if (CLOUD)
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Weborb.Util.Cloud;

namespace Weborb.Util.Logging.Cloud.Azure
{
    public class AzureLogContext : TableServiceContext
    {
        public AzureLogContext()
            : base(AzureUtil.StorageAccount.TableEndpoint.AbsoluteUri, AzureUtil.StorageAccount.Credentials)
        { }

        #region Properties
        public IQueryable<AzureLogEntry> AzureLogTable
        {
            get { return this.CreateQuery<AzureLogEntry>(AZURE_LOG_TABLE_NAME); }
        }
        #endregion

        #region Constants
        public const string AZURE_LOG_TABLE_NAME = "AzureLogTable";
        #endregion

    }
}
#endif