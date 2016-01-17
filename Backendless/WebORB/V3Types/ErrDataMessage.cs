using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.V3Types
{
    public class ErrDataMessage : ErrMessage
    {
        private DataMessage _cause;
        private Object _serverObject;
        private string[] _propertyNames;

        public ErrDataMessage( DataMessage cause, string correlationId, Exception exception )
            : base( correlationId, exception )
        {
            _cause = cause;
            faultCode = null;
        }

        public DataMessage cause
        {
            get
            {
                return _cause;
            }

            set
            {
                _cause = value;
            }
        }

        public Object serverObject
        {
            get
            {
                return _serverObject;
            }

            set
            {
                _serverObject = value;
            }
        }

        public string[] propertyNames
        {
            get
            {
                return _propertyNames;
            }

            set
            {
                _propertyNames = value;
            }
        }
    }
}
