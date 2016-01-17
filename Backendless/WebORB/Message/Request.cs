using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Util.Logging;
using Weborb.Exceptions;
using Weborb.Writer;
using Weborb.V3Types;

namespace Weborb.Message
{
	public class Request
	{
		private float version;
		private Header[] headers;
		private Body[] bodyParts;
		private IList<Body> responseBodies = new List<Body>();
		private int currentBody = 0; // for the multipart requests
		private static long instancesCreated = 0;
		//private Call debugCall;
    private IProtocolFormatter formatter;

		public Request( float version, Header[] headers, Body[] bodyParts )
		{
			instancesCreated++;
			this.version = version;
			this.headers = headers;
			this.bodyParts = bodyParts;
			//this.debugCall = new Call( bodyParts.Length );
		}

	  public Header[] Headers
	  {
	    set
	    {
	      headers = value;
	    }
      get
      {
        return headers;
      }
	  }

	  public Body[] BodyParts
	  {
	    get
	    {
	      return bodyParts;
	    }
	    set
	    {
	      bodyParts = value;
	    }
	  }

	  public float getVersion()
		{
			return version;
		}

		public int getBodyCount()
		{
			return bodyParts.Length;
		}

		public int CurrentBody
		{
			set
			{
				if( value >= getBodyCount() )
					throw new IndexOutOfRangeException( "invalid body part index. this message has " + getBodyCount() + "body parts" );

				this.currentBody = value;
			}
		}

		public string getRequestURI()
		{
			return bodyParts[ currentBody ].serviceURI;
		}

		public object getRequestBodyData()
		{
			return bodyParts[ currentBody ].dataObject;
		}

    public void setRequestBodyData( object obj )
        {
            bodyParts[ currentBody ].dataObject = obj;
        }

		public void setResponseBodyData( object obj )
		{
			//if( Log.isLogging( LoggingConstants.DEBUG ) && ( obj != null ) )
			//	Log.log( LoggingConstants.DEBUG, "AMFMessage:setResponseBodyData type: " + obj.GetType() + " object: " + obj );

			if( ( obj is Exception ) && !(obj is ServiceException) )
			{
				obj = new ServiceException( obj.ToString(), (Exception) obj );
				//debugCall.setError( currentBody );
			}

            if( (obj is Exception) && isV3Request() )
                obj = new ErrMessage( null, (Exception)obj );

			//debugCall.setReturnValue( currentBody, obj );
			bodyParts[ currentBody ].responseDataObject = obj;
			responseBodies.Add( bodyParts[ currentBody ] );
		}

		public void setResponseURI( String responseURI )
		{
			bodyParts[ currentBody ].serviceURI = "";
			bodyParts[ currentBody ].responseURI += responseURI;
		}

		public Header getHeader( String headerName )
		{
			for( int i = 0; i < headers.Length; i++ )
				if( headers[ i ].headerName.Equals( headerName ) )
					return headers[ i ];

			return null;
		}

		public Header[] getResponseHeaders()
		{
			return new Header[ 0 ]; // TODO: Fix this
		}

		public Body[] getResponseBodies()
		{
            Body[] bodyArray = new Body[ responseBodies.Count ];
            responseBodies.CopyTo( bodyArray, 0 );
            return bodyArray;
		}

        public bool isV3Request()
        {
            if( getRequestURI().IndexOf( "." ) != -1 )
                return false;

            if( !(getRequestBodyData() is Array) )
                return false;

            return true;
        }

		// ***************** INSTANCE COUNT *************************************************

		public static long getInstanceCount()
		{
			return instancesCreated;
		}

		// ********************* CALL TRACING *************************************************

        /*
		public Call getCall()
		{
			return debugCall;
		}

        public void setCallTraceStatus( String status )
		{
			debugCall.setStatus( currentBody, status );
		}
         
		public void setCallTraceMethodInfo( string serviceId, string function, object[] arguments )
		{
			debugCall.setMethodInfo( currentBody, serviceId, function, arguments );
		}

        public void setCallTraceDuration( long duration )
		{
			debugCall.setInvocationDuration( currentBody, duration );
		}

		public void setCallTraceInvocationHandler( string author )
		{
			debugCall.setHandledBy( currentBody, author );
		}
         * */

        // ********************* PROTOOCOL FORMATTER *******************************************

        public IProtocolFormatter GetFormatter()
        {
            return formatter;
        }

        public void SetFormatter( IProtocolFormatter formatter )
        {
            this.formatter = formatter;
        }
	}
}
