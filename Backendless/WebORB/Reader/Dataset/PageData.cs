using System;
using System.Collections;
using System.Data;

namespace Weborb.Reader.Dataset
{	
	/// <summary>
	/// Summary description for PageData.
	/// </summary>
	public class PageData : Hashtable
	{
		private ArrayList recordsInPage;
		private int cursor = 0;

		public void addRow( DataRow dataRow )
		{
			if( this.recordsInPage == null )
			{
				this.recordsInPage = new ArrayList();
				Add( "Page", this.recordsInPage );
			}
			
			this.recordsInPage.Add( dataRow.ItemArray );
		}		

		public void setFirstRowIndex( int index )
		{
			cursor = index;
			Add( "Cursor", index );
		}

		public ArrayList getPageRecords()
		{
			Add( "Count", this.recordsInPage == null ? 0 : this.recordsInPage.Count );
			return this.recordsInPage;
		}
	}
}
