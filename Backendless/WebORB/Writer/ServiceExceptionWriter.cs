using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Exceptions;

namespace Weborb.Writer
{
    public class ServiceExceptionWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            ServiceException exception = (ServiceException)obj;
            Dictionary<String, Object> objectFields = new Dictionary<String, Object>();
            objectFields[ "description" ] = exception.description;
            objectFields[ "details" ] = exception.details;
            objectFields[ "type" ] = exception.type;
            objectFields[ "code" ] = exception.code;
            String className = exception.exceptionName;
            formatter.GetObjectSerializer().WriteObject( className, objectFields, formatter );
        }

        #endregion
    }
}
