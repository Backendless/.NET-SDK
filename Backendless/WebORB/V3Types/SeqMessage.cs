using System;
using System.Collections;

namespace Weborb.V3Types
{
	public class SeqMessage : AckMessage
	{
		private int _sequenceId = 0;
		private int _sequenceSize;
		private object[] _sequenceProxies = null;
		private DataMessage _dataMessage;

		public SeqMessage( DataMessage dataMessage, IList data, int sequenceId, int totalRecords ) : base( dataMessage.messageId, dataMessage.clientId, data )
		{			
			this._sequenceId = sequenceId;
			this._sequenceSize = totalRecords;
			this.destination = dataMessage.destination;
		}

		public SeqMessage( DataMessage dataMessage, object data, int sequenceId ) : base( null, dataMessage.clientId, data )
		{			
			this._messageId = dataMessage.messageId;
			this._clientId = null;
			this._sequenceId = sequenceId;
			this._sequenceSize = 1;
			this._destination = dataMessage.destination;
			this._dataMessage = dataMessage; 
			this._correlationId = null;
		}
		
		public int sequenceId
		{
			get
			{
				return this._sequenceId;
			}

			set
			{
				this._sequenceId = value;
			}
		}

		public int sequenceSize
		{
			get
			{
				return this._sequenceSize;
			}

			set
			{
				this._sequenceSize = value;
			}
		}

		public object[] sequenceProxies
		{
			get
			{
				return _sequenceProxies;
			}

			set
			{
				this._sequenceProxies = value;
			}
		}

		public DataMessage dataMessage
		{
			get
			{
				return _dataMessage;
			}

			set
			{
				_dataMessage = value;
			}
		}
	}
}
