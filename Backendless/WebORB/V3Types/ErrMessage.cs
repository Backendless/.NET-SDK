using System;
using Weborb.Exceptions;
using Weborb.Util.Logging;

namespace Weborb.V3Types
{
	public class ErrMessage : AckMessage
	{
		private object _rootCause;
 	    private string _faultString;
		private string _faultCode = "Server.Processing";
		private object _extendedData;
		private string _faultDetail;
        //private string _code;

        // used by server-to-server AMF client. Required for deserialization
        public ErrMessage()
        {
            isError = true;
        }

		public ErrMessage( string correlationId, Exception exception ) : base( correlationId, null, null )
		{
            if( exception is ServiceException )
            {
                faultString = ((ServiceException) exception).description;
                faultDetail = ((ServiceException) exception).details;

                if( ((ServiceException) exception).code != -1 )
                    extendedData = ((ServiceException) exception).code;
                else
                    extendedData = exception.Data;

                faultCode = ((ServiceException)exception).code.ToString();
            }
            else
            {
                faultString = exception.Message;
                faultDetail = exception.StackTrace;
                extendedData = exception.Data;


                //faultCode = ((ServiceException)exception).code.ToString();
            }
			
            isError = true;
		}
        /*
        public string code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        } **/

		public object rootCause
		{
			get
			{
				return _rootCause;
			}

			set
			{
				_rootCause = value;
			}
		}

		public string faultString
		{
			get
			{
				return _faultString;
			}

			set
			{
				_faultString = value;
			}
		}

		public string faultCode
		{
			get
			{
				return _faultCode;
			}

			set
			{
				_faultCode = value;
			}
		}

		public object extendedData
		{
			get
			{
				return _extendedData;
			}

			set
			{
				_extendedData = value;
			}
		}

		public string faultDetail
		{
			get
			{
				return _faultDetail;
			}

			set
			{
				_faultDetail = value;
			}
		}
	}
}
