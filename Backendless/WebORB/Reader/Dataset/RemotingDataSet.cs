using System;
using System.Data;
using System.Web.SessionState;
using System.Diagnostics;
using Weborb.Reader;
using Weborb.Util;
using Weborb.Exceptions;


namespace Weborb.Reader.Dataset
{
	/// <summary>
	/// Summary description for DataSetWriter.
	/// </summary>
	public class RemotingDataSet : INamedType
	{
        public static string PAGESIZE = "PageSize";
		private static int DEFAULT_PAGE_SIZE = -1;
		private DataTable dataTable;

		public RemotingDataSet()
		{
		}

		public RemotingDataSet( DataTable dataTable )
		{
			//this.dataSet = dataSet;
			//this.dataTable = this.dataSet.Tables[ 0 ];
			this.dataTable = dataTable;
		}

		public static void SetDefaultPageSize( int pageSize )
		{
            //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            //    throw new Exception( "cannot change page size, this feature is available in WebORB Professional Edition" );

			DEFAULT_PAGE_SIZE = pageSize;
		}

		public DataSetInfo getDataSetInfo()
		{
			DataSetInfo dataSetInfo = new DataSetInfo( this );			
			dataSetInfo.setCurrentRowIndex( 1 );
            int recordsToGet = DEFAULT_PAGE_SIZE == -1 ? dataTable.Rows.Count : DEFAULT_PAGE_SIZE;

            if( ThreadContext.getRuntimeConfig().Contains( PAGESIZE ) )
                recordsToGet = (int) ThreadContext.getRuntimeConfig()[ PAGESIZE ];                
            
			dataSetInfo.setRecordsData( getRecords( 0, recordsToGet ).getPageRecords() );
			dataSetInfo.setNumberOfRows( dataTable.Rows.Count );
            dataSetInfo.setPagingSize( recordsToGet );

			string[] columnNames = new string[ dataTable.Columns.Count ];

			for( int i = 0; i < columnNames.Length; i++ )
				columnNames[ i ] = dataTable.Columns[ i ].ColumnName;

			dataSetInfo.setColumnNames( columnNames  );
			return dataSetInfo;
		}

		public PageData getRecords( string id, int firstRow, int rowsToGet )
		{
			HttpSessionState httpSession = ThreadContext.currentSession();			
			RemotingDataSet dataSet = (RemotingDataSet) httpSession[ id ];
			
			if( dataSet == null )
				throw new ServiceException( "unable to find DataSet mapped to " + id );

			try
			{
				return dataSet.getRecords( firstRow - 1, rowsToGet );
			}
			catch( Exception exception )
			{
				Trace.WriteLine( exception.StackTrace );
				throw new ServiceException( "unable to retrieve records due to exception", exception );
			}
		}

		private PageData getRecords( int rowStart, int rowsToGet )
		{
			PageData pageData = new PageData();
			pageData.setFirstRowIndex( rowStart + 1 );

			if( rowStart > dataTable.Rows.Count )
				throw new Exception( "invalid row index" );

			int endRow = Math.Min( rowStart + rowsToGet, dataTable.Rows.Count );

			for( int i = rowStart; i < endRow; i++ )
				pageData.addRow( dataTable.Rows[ i ] );

			return pageData;
		}

		#region INamedType Members

		public string getTypeName()
		{
			return "RecordSet";
		}

		#endregion
	}
}
