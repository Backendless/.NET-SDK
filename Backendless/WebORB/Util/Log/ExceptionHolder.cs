using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util.Logging
{
    public class ExceptionHolder
    {
        private String message;
        private Exception exception;

        public String Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }

        public Exception ExceptionObject
        {
            get
            {
                return exception;
            }

            set
            {
                exception = value;
            }
        }

        public override String ToString()
        {
            if( message != null )
                return message + " " + exception.Message + ":" + exception.StackTrace;
            else
                return exception.Message + ":" + exception.StackTrace;
        }
    }
}
