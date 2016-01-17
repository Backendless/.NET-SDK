using System;
using Weborb.Message;

namespace Weborb.Exceptions
{
	public class LicenseException : Exception
	{
		public string description = "error message not available";
		public string details = "details not available";
#if (FULL_BUILD)
        [NonSerialized()] 
#endif
        private Request requestObject;

		public LicenseException( string message ) : base( message )
		{
			this.description = message;
			this.details = message;
		}

        public LicenseException( string message, Request requestObject ) : base( message )
        {
            this.requestObject = requestObject;
        }

        public Request getRequestObject()
        {
            return requestObject;
        }
	}
}
