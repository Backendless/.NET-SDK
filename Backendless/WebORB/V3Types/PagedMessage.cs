using System;
using System.Collections;

namespace Weborb.V3Types
{
	public class PagedMessage : SeqMessage
	{
		private int _pageIndex = 0;
		private int _numberPages;

		public PagedMessage( DataMessage dataMessage, IList data, int sequenceId, int totalRecords ) : base( dataMessage, data, sequenceId, totalRecords )
		{			
			int pageSize = (int) dataMessage.headers[ "pageSize" ];
			this._numberPages = (int) Math.Ceiling( totalRecords * 1.0f / pageSize );

			if( dataMessage.headers.Contains( "pageIndex" ) )
				this._pageIndex = (int) dataMessage.headers[ "pageIndex" ];
		}

		public int numberPages
		{
			get
			{
				return this._numberPages;
			}

			set
			{
				this._numberPages = value;
			}
		}

		public int pageIndex
		{
			get
			{
				return this._pageIndex;
			}

			set
			{
				this._pageIndex = value;
			}
		}
	}
}
