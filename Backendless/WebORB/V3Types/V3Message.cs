using System;
using System.Collections;
using Weborb.Types;
using Weborb.Message;
using Weborb.Util;

namespace Weborb.V3Types
{
#if (FULL_BUILD)
    [Serializable()]
#endif
	public abstract class V3Message
	{
		internal long _timestamp;
        internal BodyHolder _body;
        internal int _timeToLive;
        internal string _destination;
        internal string _messageId;
        internal Object _clientId;
        internal IDictionary _headers;
        internal string _correlationId;
#if (FULL_BUILD)
        [NonSerialized]
#endif
        internal bool isError = false;

		public long timestamp
		{
			get
			{
				return _timestamp;
			}

			set
			{
				_timestamp = value;
			}
		}

		public BodyHolder body
		{
			get
			{
				return _body;
			}

			set
			{
				_body = value;
			}
		}

		public int timeToLive
		{
			get
			{
				return _timeToLive;
			}
			set
			{
				_timeToLive = value;
			}
		}

		public string destination
		{
			get
			{
				return _destination;
			}
			set
			{
				_destination = value;
			}
		}


		public string messageId
		{
			get
			{
				return _messageId;
			}
			set
			{
				_messageId = value;
			}
		}



		public object clientId
		{
			get
			{
				return _clientId;
			}
			set
			{
				_clientId = value;
			}
		}

		public IDictionary headers
		{
			get
			{
				return _headers;
			}
			set
			{
				_headers = value;
			}
		}

		public string correlationId
		{
			get
			{
				return _correlationId;
			}
			set
			{
				_correlationId = value;
			}
		}

		public abstract V3Message execute( Request message, RequestContext context );
	}
}
