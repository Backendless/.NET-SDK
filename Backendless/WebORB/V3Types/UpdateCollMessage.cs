using System;
using Weborb.Message;
using Weborb.Types;
using Weborb.Util;

namespace Weborb.V3Types
{
	public class UpdateCollMessage : AckMessage
	{
		private object[] _collectionId;
		private bool _replace;
		private int _updateMode;
		private object _identity;
		private int _operation;

        public UpdateCollMessage()
            : base( null, null, null )
        {
        }

		public UpdateCollMessage( string destination, string correlationId, object clientId, object obj, object[] collId ) : base( correlationId, clientId, obj )
		{
			base._destination = destination;
			_operation = 17;
			_updateMode = 1;
			_replace = false;
			_messageId = "srv:" + _messageId + ":1";
			_collectionId = collId;
		}

		public object[] collectionId
		{
			get
			{
				return _collectionId;
			}

			set
			{
				this._collectionId = value;
			}
		}

		public bool replace
		{
			get
			{
				return _replace;
			}

			set
			{
				this._replace = value;
			}
		}

		public int updateMode
		{
			get
			{
				return _updateMode;
			}

			set
			{
				this._updateMode = value;
			}
		}

		public int operation
		{
			get
			{
				return _operation;
			}

			set
			{
				this._operation = value;
			}
		}

		public object identity
		{
			get
			{
				return _identity;
			}

			set
			{
				_identity = value;
			}
		}

        public override V3Message execute( Request message, RequestContext context )
        {
            object[] args = (object[]) body.body;
            body.body = new object[] { ((IAdaptingType) args[ 0 ]).defaultAdapt() };
            return this;
        }
	}
}
