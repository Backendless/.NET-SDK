#if (SILVERLIGHT && !WINDOWS_PHONE)
using System.Windows.Browser;
#endif

#if (SILVERLIGHT)
using System;
using System.Net;
using System.Windows;
using Weborb.V3Types;
using System.Windows.Threading;
using Weborb.Util.Logging;
using System.Windows.Controls;
using System.Collections;

namespace Weborb.Client.Messaging
{
    public class Consumer : MessageAgent
    {
        #region Fields
        /// <summary>
        /// The timer used to try resubscription
        /// </summary>
        protected DispatcherTimer _pollingTimer;

        /// <summary>
        /// The time to wait between resubscribe attempts
        /// </summary>
        protected int _pollingInterval;

        /// <summary>
        /// Current subscribe mesage - Used for resubscribe attempts
        /// </summary>
        protected CommandMessage _subscribeMessage;

        /// <summary>
        /// Flag indicating whether the Consumer is currently connected
        /// 
        /// </summary>
        protected bool _subscribed;

        /// <summary>
        /// The timestamp of the last received message
        /// </summary>
        protected long _timestamp;

        /// <summary>
        /// Used for handling message errors
        /// </summary>
        protected ErrorHandler errorHandler;

        protected Responder<Object[]> responder;

        protected Responder<Object> subscribeResponder;
        #endregion

        #region Constructors
        public Consumer(string destination)
            : this(destination, null)
        { }
        public Consumer(string destination, UserControl uiControl)
            : this(destination, uiControl, 1000)
        { }
        public Consumer(string destination, UserControl uiControl, int pollingInterval)
            : this(destination, uiControl, pollingInterval, "weborb.aspx")
        { }
        public Consumer(string destination, UserControl uiControl, int pollingInterval, string gatewayUrl)
            : base(uiControl, gatewayUrl)
        {
            _pollingInterval = pollingInterval;
            Destination = destination;
            errorHandler = new ErrorHandler(onFault);
            responder = new Responder<Object[]>(pollResponse, errorHandler);
            subscribeResponder = new Responder<object>(subscribeResponse, errorHandler);
            _agentType = "consumer";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Updates the destination for the consumer and resubscribes if currently subscribed
        /// </summary>
        public override String Destination
        {
            get { return base.Destination; }
            set
            {
                if (_destination != value)
                {
                    bool resetSubscription = false;

                    if (Subscribed)
                    {
                        Unsubscribe();
                        resetSubscription = true;
                    }

                    base.Destination = value;

                    if (resetSubscription)
                        Subscribe();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean Subscribed
        {
            get { return _subscribed; }
            set
            {
                if (_subscribed != value)
                {
                    _subscribed = value;

                    if (_subscribed)
                    {
                        //  Turn on polling 
                    }
                    else
                    {
                        //  Turn off polling
                    }
                }
            }
        }

        /// <summary>
        /// Contains the timestamp of the most recent message this Consumer has received.
        /// This value is passed to the destination in a <code>receive()</code> call to 
        /// request that it deliver messages for the Consumer from the timestamp forward.
        /// All messages with a timestamp value greater than the <code>timestamp</code> 
        /// value will be returned during a poll operation.  Setting this value to -1 will 
        /// retrieve all cached messages from the destination.
        /// </summary>
        public long Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        #endregion

        #region Events
        public event MessagingEventDelegate MessageRecieved;
        protected void onMessageReceived(MessagingEventArgs e)
        {
            MessageRecieved(this, e);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Subscribe()
        {
            if (Subscribed)
                Unsubscribe();

            if (Log.isLogging(LoggingConstants.INFO))
                Log.log(LoggingConstants.INFO, "Subscribing consumer");

            _subscribeMessage = buildSubscribeMessage();

            sendMessage(_subscribeMessage, subscribeResponder);
        }

        protected void pollingTimer_Tick(object sender, EventArgs e)
        {
            CommandMessage msg = new CommandMessage();
            msg.operation = CommandMessage.POLL_OPERATION.ToString();
            msg.destination = Destination;

            sendMessage(msg, responder);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unsubscribe()
        {
            sendMessage(buildUnsubscribeMessage(), subscribeResponder);

            _pollingTimer.Stop();
        }

        protected CommandMessage buildSubscribeMessage()
        {
            CommandMessage msg = new CommandMessage();
            msg.operation = CommandMessage.SUBSCRIBE_OPERATION.ToString();
            msg.destination = Destination;

            return msg;
        }

        protected CommandMessage buildUnsubscribeMessage()
        {
            CommandMessage msg = new CommandMessage();
            msg.operation = CommandMessage.UNSUBSCRIBE_OPERATION.ToString();
            msg.destination = Destination;

            return msg;
        }

        protected void pollResponse(Object[] response)
        {
            if (response != null && response.Length > 0)
            {
                foreach (IDictionary dict in response)
                {
                    onMessageReceived(new MessagingEventArgs(dict["body"], dict["headers"]));
                }
            }
        }

        protected void subscribeResponse(Object response)
        {
            _pollingTimer = new DispatcherTimer();
            _pollingTimer.Interval = new TimeSpan(0, 0, 0, 0, _pollingInterval);
            _pollingTimer.Tick += new EventHandler(pollingTimer_Tick);
            _pollingTimer.Start();
        }

        protected void onFault(Fault fault)
        {
#if( !WINDOWS_PHONE )
            if (uiControl != null)
                uiControl.Dispatcher.BeginInvoke(delegate()
                {
                    HtmlPage.Window.Alert("Fault in consumer: " + fault.Detail + "\n" + fault.Message);
                });
#endif
        }
    }

    public delegate void MessagingEventDelegate(object sender, MessagingEventArgs args);
    public class MessagingEventArgs : EventArgs
    {
        public MessagingEventArgs()
            : this(null)
        { }
        public MessagingEventArgs(object body)
            : this(body, null)
        { }
        public MessagingEventArgs(object body, object headers)
        {
            Body = body;
            Headers = headers;
        }

        public Object Body;
        public Object Headers;
    }

}
#endif