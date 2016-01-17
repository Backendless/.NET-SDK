#if (SILVERLIGHT)
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Weborb.Util.Logging;
using Weborb.V3Types;
using Weborb.Message;
using Weborb.Util;
using System.IO;
using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Config;
using System.Collections;
using System.Collections.Generic;

namespace Weborb.Client.Messaging
{
    public class MessageAgent
    {
        #region Fields
        /// <summary>
        /// The current destination for the Consumer
        /// </summary>
        protected string _destination;

        protected string _agentType;

        protected UserControl uiControl;

        protected string id;

        protected WeborbClient client;
        #endregion

        #region Constructors
        public MessageAgent()
            : this(null)
        { }
        public MessageAgent(UserControl uiControl)
            : this(uiControl, "weborb.aspx")
        { }
        public MessageAgent(UserControl uiControl, string gatewayUrl)
        {
            this.uiControl = uiControl;

            this.id = Guid.NewGuid().ToString();

            client = new WeborbClient(gatewayUrl, "GenericDestination", uiControl);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Updates the destination for the consumer and resubscribes if currently subscribed
        /// </summary>
        public virtual String Destination
        {
            get { return _destination; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("The specified destination is invalid and cannot be null or empty");

                if (_destination != value)
                {
                    _destination = value;

                    if (Log.isLogging(LoggingConstants.INFO))
                        Log.log(LoggingConstants.INFO, "Destination set to " + _destination + " for consumer");
                }
            }
        }
        #endregion

        static MessageAgent()
        {
            Types.Types.AddClientClassMapping("flex.messaging.messages.AcknowledgeMessage", typeof(AckMessage));
            Types.Types.AddClientClassMapping("flex.messaging.messages.RemotingMessage", typeof(CommandMessage));
            Types.Types.AddClientClassMapping("flex.messaging.messages.CommandMessage", typeof(CommandMessage));
            Types.Types.AddClientClassMapping("flex.messaging.messages.ErrorMessage", typeof(ErrMessage));
            Types.Types.AddClientClassMapping( "flex.messaging.io.ArrayCollection", typeof( ObjectProxy ) );
            ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory("Weborb.V3Types.BodyHolder", new Weborb.V3Types.BodyHolderFactory());
            Types.Types.AddAbstractTypeMapping(typeof(IDictionary), typeof(Dictionary<object, object>));
        }

        protected void sendMessage<T>(V3Message msg, Responder<T> responder)
        {
            if (msg.headers == null)
                msg.headers = new Dictionary<String, Object>();

            msg.headers["DSId"] = id;

            client.SendMessage(msg, responder);
        }
    }
}
#endif