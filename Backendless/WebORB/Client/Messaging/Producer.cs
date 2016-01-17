#if (SILVERLIGHT && !WINDOWS_PHONE)
using System.Windows.Browser;
#endif

#if (SILVERLIGHT)
using System;
using Weborb.V3Types;
using System.Collections.Generic;
using Weborb.Util.Logging;

namespace Weborb.Client.Messaging
{
    public class Producer : MessageAgent
    {
        protected ErrorHandler errorHandler;

        protected Responder<Object> sendResponder;

        public string SubTopic;

        public Producer(string destination)
            : this(destination, "weborb.aspx")
        { }
        public Producer(string destination, string gatewayUrl)
            : base(null, gatewayUrl)
        {
            Destination = destination;
            _agentType = "producer";
            errorHandler = new ErrorHandler(onFault);
            sendResponder = new Responder<Object>(sendResponse, errorHandler);
        }

        public void Send(object message)
        {
            Send(message, null);
        }
        public void Send(object message, string clientId)
        {
            AsyncMessage msg = new AsyncMessage();
            msg.destination = base.Destination;
            msg.body = new BodyHolder();
            msg.body.body = message;

            if (clientId != null)
            {
                msg.headers = new Dictionary<String, Object>();
                msg.headers["WebORBClientId"] = clientId;
            }

            sendMessage(msg, sendResponder);
        }

        protected void sendResponse(Object response)
        {
            if (Log.isLogging(LoggingConstants.DEBUG))
                Log.log(LoggingConstants.DEBUG, response);
        }

        protected void onFault(Fault fault)
        {
#if( !WINDOWS_PHONE )
            if (uiControl != null)
                uiControl.Dispatcher.BeginInvoke(delegate()
                {
                    HtmlPage.Window.Alert("Fault during message send: " + fault.Detail + "\n" + fault.Message);
                });
#endif
        }
    }
}
#endif