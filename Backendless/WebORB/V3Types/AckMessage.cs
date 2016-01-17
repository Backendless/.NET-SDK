using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;
using Weborb.Util;
using Weborb.Message;

namespace Weborb.V3Types
{
	public class AckMessage : V3Message
	{
        // must be here. used by server-to-server AMF client during deserialization
        public AckMessage()
        {
        }

		public AckMessage( string correlationId, object clientId, object obj ) : this( correlationId, clientId, obj, null )
		{
		}

        public AckMessage(string correlationId, object clientId, object obj, IDictionary headers)
        {
            this.correlationId = correlationId;
            this.clientId = clientId == null ? Guid.NewGuid().ToString().ToUpper() : clientId;
            this.messageId = Guid.NewGuid().ToString().ToUpper(); // "12DB8FBF-1700-DC12-C88A-B0BA6C88C7ED";

            if( headers != null )
            {
                this.headers = headers;
            }
            else
            {
#if (FULL_BUILD)
                Hashtable responseMetadata = null;

                if( ThreadContext.getProperties().Contains( ORBConstants.RESPONSE_METADATA ) )
                    responseMetadata = (Hashtable) ThreadContext.getProperties()[ ORBConstants.RESPONSE_METADATA ];

                if( responseMetadata != null )
                    this.headers = responseMetadata;
                else
                    this.headers = new Hashtable();
#else
                this.headers = new Dictionary<object, object>();
#endif
            }

            DateTime startTime = new DateTime( 1970, 1, 1 );
            long t = DateTime.Now.Ticks - startTime.Ticks;
            this.timestamp = (long) TimeSpan.FromTicks( t ).TotalMilliseconds;
            this.body = new BodyHolder();
            this.body.body = obj;
            this.timeToLive = 0;

        }

        public override V3Message execute( Request message, RequestContext context )
		{
			//throw new Exception( "AckMessage should never be execution target" );
            return this;
		}
	}
}
