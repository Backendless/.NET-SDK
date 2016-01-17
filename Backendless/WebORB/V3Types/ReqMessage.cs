using System;
using System.Text;
using System.Collections;
using System.Security.Principal;
using System.Threading;

using Weborb.Config;
using Weborb.Message;
#if (FULL_BUILD)
using Weborb.Dispatch;
using Weborb.Security;
using Weborb.V3Types.Core;
#endif
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Exceptions;

namespace Weborb.V3Types
{
    public class ReqMessage : V3Message
    {
        private string _operation;
        private string _source;
        private string _messageRefType;

        public string operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }

        public string messageRefType
        {
            get
            {
                return _messageRefType;
            }
            set
            {
                _messageRefType = value;
            }
        }

        public string source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
            }
        }

        public override V3Message execute(Request message, RequestContext context)
        {
            Object returnValue = null;
#if(FULL_BUILD)
            if (body.body == null)
                body.body = new object[0];
            else if (!body.body.GetType().IsArray)
                body.body = new object[] { body.body };

            try
            {
                if (Registry.ServiceRegistry.GetMapping(destination).Equals("*"))
                    destination = source;

                ThreadContext.getProperties()[ORBConstants.REQUEST_HEADERS] = headers;
                ThreadContext.getProperties()[ORBConstants.CLIENT_ID] = clientId;

                returnValue = Invoker.handleInvoke( message, destination, operation, (Object[])( (Object[])body.body )[0], context );
            }
            catch (Exception exception)
            {
                return new ErrMessage(messageId, exception);
            }
#endif
            return new AckMessage(messageId, clientId, returnValue);
        }
    }
}
