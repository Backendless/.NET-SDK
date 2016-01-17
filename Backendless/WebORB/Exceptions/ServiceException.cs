using System;
using System.Runtime.Serialization;

namespace Weborb.Exceptions
{
#if (FULL_BUILD)    
	[Serializable()]
#endif
	public class ServiceException : Exception 
#if (FULL_BUILD)
        , ISerializable 
#endif
	{
		public static int instancesCreated = 0;
		public string description = "error message not available";
		public string details = "details not available";
        public string type;
		public int code = -1;
        public string exceptionName = "Weborb.Exceptions.ServiceException";

		public ServiceException( string errorMessage ) : base( errorMessage )
		{
			instancesCreated++;
			description = errorMessage;
		}

		public ServiceException( string errorMessage, int errorCode ) : base( errorMessage )
		{
			instancesCreated++;
			description = errorMessage;
			this.code = errorCode;
		}

		public ServiceException( string errorMessage, Exception exception ) : this( errorMessage, exception, -1 )
		{
		}

		public ServiceException( string errorMessage, Exception exception, int errorCode )
		{
            if( exception is ServiceException )
            {
                this.description = ((ServiceException) exception).description;
                this.code = ((ServiceException) exception).code;
            }
            else
            {
                this.exceptionName = exception.GetType().FullName;
                this.description = errorMessage;
                this.code = errorCode;
            }

			this.details = exception.Message + " : " + exception.StackTrace;
            this.type = exception.GetType().ToString();
		}

#if (FULL_BUILD)
		public ServiceException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{
			instancesCreated = info.GetInt32( "instancesCreated" );
			description = info.GetString( "description" );
			details = info.GetString( "details" );
			code = info.GetInt32( "code" );
		}

		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );
			info.AddValue( "instancesCreated", instancesCreated );
			info.AddValue( "description", instancesCreated );
			info.AddValue( "details", instancesCreated );
			info.AddValue( "code", code );
		}
#endif
	}
}
