using System;
using System.Collections;

namespace Weborb.Reader.Dataset
{
	/// <summary>
	/// Summary description for DataSetInfo.
	/// </summary>
	[Serializable()]
	public class DataSetInfo : INamedType, IHttpSessionObject
	{
		[NonSerialized()] private RemotingDataSet dataSet;
		private Hashtable _serverInfo;

		public DataSetInfo( RemotingDataSet dataSet )
		{
			this.dataSet = dataSet;
			this._serverInfo = new Hashtable();
			this._serverInfo[ "version" ] = 1.0d;
			this._serverInfo[ "serviceName" ] = "Weborb.Reader.Dataset.RemotingDataSet";
			this._serverInfo[ "id" ] = Guid.NewGuid().ToString();
		}

		public void setNumberOfRows( int numberOfRows )
		{
			this._serverInfo[ "totalCount" ] = numberOfRows;
		}

		public void setCurrentRowIndex( int currentRowIndex )
		{
			this._serverInfo[ "cursor" ] = currentRowIndex;
		}

		public void setColumnNames( string[] columnNames )
		{
			this._serverInfo[ "columnNames" ] = columnNames;
		}

		public void setRecordsData( ArrayList records )
		{
			this._serverInfo[ "initialData" ] = records;
		}

        public void setPagingSize( int size )
        {
            this._serverInfo[ "pagingSize" ] = size;
        }

        public Hashtable serverInfo
        {
            get
            {
                return _serverInfo;
            }
        }

		#region INamedType Members

		public string getTypeName()
		{
			return "RecordSet";
		}

		#endregion

		#region IHttpSessionObject Members

		public string getID()
		{
			return (string) this._serverInfo[ "id" ];
		}

		public object getObject()
		{
			return dataSet;
		}

		#endregion
	}
}
