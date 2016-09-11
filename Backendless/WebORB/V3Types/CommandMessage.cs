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
#if (!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using Weborb.Messaging.PubSub;
using Weborb.Messaging.Net.RTMP;
using Weborb.Messaging.Server;
using System.Collections.Generic;
using Weborb.Types.Generic;
using Weborb.Messaging.WebSocket;
#endif

namespace Weborb.V3Types
{
  public class CommandMessage : V3Message
  {
    public static String DSSELECTOR = "DSSelector";
    public static String DSSUBTOPIC = "DSSubtopic";
    public static String DSID = "DSId";

    private string _operation;
    //private string _source;
    //private string _messageRefType;

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

    /*public string messageRefType
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
    }*/

    public const string SUBSCRIBE_OPERATION = "0";
    public const string UNSUBSCRIBE_OPERATION = "1";
    public const string POLL_OPERATION = "2";
    public const string CLIENT_SYNC_OPERATION = "4";
    public const string CLIENT_PING_OPERATION = "5";
    public const string CLIENT_REQUEST_OPERATION = "7";
    public const string LOGIN_OPERATION = "8";
    public const string LOGOUT_OPERATION = "9";
    public const string SUBSCRIPTION_INVALIDATE_OPERATION = "10";
    public const string MULTI_SUBSCRIBE_OPERATION = "11";
    public const string DISCONNECT_OPERATION = "12";
    public const string TRIGGER_CONNECT_OPERATION = "13";
    public const string UNKNOWN_OPERATION = "10000";

#if (FULL_BUILD)
    public CommandMessage()
    { }
    public CommandMessage( String operation, Object body )
    {
      this.messageId = Guid.NewGuid().ToString().ToUpper();

      Hashtable responseMetadata = (Hashtable) ThreadContext.getProperties()[ ORBConstants.RESPONSE_METADATA ];

      if( responseMetadata != null )
        this.headers = responseMetadata;
      else
        this.headers = new Hashtable();

      this.timestamp = ORBUtil.currentTimeMillis();
      this.body = new BodyHolder();
      this.body.body = body;
      this.timeToLive = 0;
      this.operation = operation;
    }

    public override V3Message execute( Request message, RequestContext context )
    {
      object returnValue = null;

      switch ( operation )
      {
        case SUBSCRIBE_OPERATION:
          {
            IDestination destObj =
              ORBConfig.GetInstance().GetDataServices().GetDestinationManager().GetDestination(destination);
            Hashtable headers = new Hashtable();

            RTMPConnection connection = (RTMPConnection) ConnectionHub.getConnectionLocal();

            if (destObj != null)
            {
              String selectorName = (String) this.headers["DSSelector"];
              String subtopic = (String) this.headers["DSSubtopic"];
              String dsId = connection == null ? (String) this.headers["DSId"] : connection.GetHashCode().ToString();
              String channel = (String) this.headers["DSEndpoint"];

              Subscriber subscriber = SubscriptionsManager.GetInstance().getSubscriber(
                Subscriber.buildId(dsId, destObj.GetName(), subtopic, selectorName));

              if (clientId == null || clientId.Equals(""))
                clientId = Guid.NewGuid().ToString().ToUpper();

              if (subscriber != null)
              {
                if (subscriber.addClient(clientId.ToString()))
                  destObj.GetServiceHandler().HandleSubscribe(subscriber, clientId.ToString(), this);

                return new AckMessage(messageId, clientId, null, headers);
              }

              object wsContext = ThreadContext.getProperties()[ORBConstants.WEB_SOCKET_MODE];

              if (wsContext != null)
              {
                subscriber = new WebSocketSubscriber(selectorName, destObj, (UserContext) wsContext);
              }
              else if (connection != null)
              {
                subscriber = new DedicatedSubscriber(selectorName, destObj);
                subscriber.setChannelId(RTMPHandler.getChannelId());
                subscriber.setConnection(connection);
              }
              else
              {
                subscriber = SubscriberFactory.CreateSubscriber(channel, selectorName, destObj);
              }

              subscriber.setDSId(dsId);
              subscriber.setSubtopic(subtopic);
              subscriber.addClient((String) clientId);

              try
              {
                SubscriptionsManager.GetInstance().AddSubscriber(dsId, destObj.GetName(), subscriber);
              }
              catch (Exception e)
              {
                if (Log.isLogging(LoggingConstants.EXCEPTION))
                  Log.log(LoggingConstants.EXCEPTION, e);
              }

              destObj.GetServiceHandler().HandleSubscribe(subscriber, clientId.ToString(), this);
            }
            else
            {
              String error = "Unknown destination " + destination + ". Cannot handle subscription request";

              if (Log.isLogging(LoggingConstants.ERROR))
                Log.log(LoggingConstants.ERROR, error);

              return new ErrMessage(messageId, new Exception(error));
            }

            return new AckMessage(messageId, clientId, null, headers);
          }
          break;
        case UNSUBSCRIBE_OPERATION:
          {
            String subtopic = (String) this.headers["DSSubtopic"];
            String dsId = (String) this.headers["DSId"];
            String selectorName = (String) this.headers["DSSelector"];

            RTMPConnection connection = (RTMPConnection) ConnectionHub.getConnectionLocal();

            if (connection != null)
              dsId = connection.GetHashCode().ToString();

            Subscriber subscriber = SubscriptionsManager.GetInstance().getSubscriber(
              Subscriber.buildId(dsId, destination, subtopic, selectorName));

            if (subscriber != null)
            {
              SubscriptionsManager.GetInstance().unsubscribe(subscriber, clientId.ToString(), this);
            }
          }
          break;
        case DISCONNECT_OPERATION:
          {
            String dsId = (String) this.headers["DSId"];
            RTMPConnection connection = (RTMPConnection) ConnectionHub.getConnectionLocal();

            if (connection != null)
              dsId = connection.GetHashCode().ToString();

            SubscriptionsManager subscriptionsManager = SubscriptionsManager.GetInstance();
            List<Subscriber> subscribers = subscriptionsManager.getSubscribersByDsId(dsId);

            if (subscribers != null)
              foreach (Subscriber subscriber in subscribers )
                if (subscriber != null)
                  subscriptionsManager.unsubscribe(subscriber, this);

            subscriptionsManager.removeSubscriber(dsId);
          }
          break;
        case POLL_OPERATION:
          {
            String dsId = (String) this.headers["DSId"];

            RTMPConnection connection = (RTMPConnection) ConnectionHub.getConnectionLocal();

            if (connection != null)
              dsId = connection.GetHashCode().ToString() + "";

            try
            {
              WebORBArray<V3Message> messages =
                new WebORBArray<V3Message>(SubscriptionsManager.GetInstance().getMessages(dsId));

              if (messages.Count == 0)
                return new AckMessage(null, null, null, new Hashtable());

              return new CommandMessage(CLIENT_SYNC_OPERATION, messages);
            }
            catch (Exception e)
            {
              String error = "Invalid client id " + dsId;

              if (Log.isLogging(LoggingConstants.ERROR))
                Log.log(LoggingConstants.ERROR, error, e);

              return new ErrMessage(messageId, new Exception(error));
            }
          }
          break;
        case CLIENT_PING_OPERATION:
          {
            Hashtable headers = new Hashtable();

            RTMPConnection connection = (RTMPConnection) ConnectionHub.getConnectionLocal();
            if (connection != null)
              headers.Add("DSId", connection.GetHashCode().ToString());
            else
              headers.Add("DSId", Guid.NewGuid().ToString().ToUpper());

            return new AckMessage(messageId, clientId, null, headers);
          }
          break;
        case LOGOUT_OPERATION:
          {
            ThreadContext.setCallerCredentials(null, null);
            Thread.CurrentPrincipal = null;
          }
          break;
        case LOGIN_OPERATION:
          {

            String credentials = (String) ((IAdaptingType) ((object[]) body.body)[0]).defaultAdapt();
            byte[] bytes = Convert.FromBase64String(credentials);
            credentials = new String(Encoding.UTF8.GetChars(bytes));
            IAuthenticationHandler authHandler = ORBConfig.GetInstance().getSecurity().GetAuthenticationHandler();

            if (authHandler == null)
            {
              ErrMessage errorMessage = new ErrMessage(messageId, new ServiceException("Missing authentication handler"));
              errorMessage.faultCode = "Client.Authentication";
              return errorMessage;
            }

            int index = credentials.IndexOf(":");
            string userid = null;
            string password = null;

            if (index != -1 && index != 0 && index != credentials.Length - 1)
            {
              userid = credentials.Substring(0, index);
              password = credentials.Substring(index + 1);

              try
              {
                IPrincipal principal = authHandler.CheckCredentials(userid, password, message);

                try
                {
                  Thread.CurrentPrincipal = principal;
                  ThreadContext.currentHttpContext().User = principal;
                }
                catch (Exception exception)
                {
                  if (Log.isLogging(LoggingConstants.ERROR))
                    Log.log(LoggingConstants.ERROR,
                            "Unable to set current principal. Make sure your current permission set allows Principal Control",
                            exception);

                  throw exception;
                }

                Credentials creds = new Credentials();
                creds.userid = userid;
                creds.password = password;
                ThreadContext.setCallerCredentials(creds, principal);
              }
              catch (Exception exception)
              {
                ErrMessage errorMessage = new ErrMessage(messageId, exception);
                errorMessage.faultCode = "Client.Authentication";
                return errorMessage;
              }
            }
            else
            {
              ErrMessage errorMessage = new ErrMessage(messageId, new ServiceException("Invalid credentials"));
              errorMessage.faultCode = "Client.Authentication";
              return errorMessage;
            }
          }
          break;
      }

      return new AckMessage( messageId, clientId, returnValue, new Hashtable() );
    }
#else
        public override V3Message execute( Request message, RequestContext context )
        {
            return null;
        } 
#endif
#if FULL_BUILD
    public static CommandMessage CreateSubscribeCommand(string destination, string dsId, string clientId, string selector, string subtopic)
    {
      CommandMessage message = new CommandMessage();
      message.operation = CommandMessage.SUBSCRIBE_OPERATION;
      message.headers.Add( Subscriber.DS_ID, dsId );
      message.headers.Add( Subscriber.DS_SUBTOPIC, subtopic );
      message.headers.Add( DSSELECTOR, selector );
      message.destination = destination;
      message.clientId = clientId;
      return message;
    }
#endif
  }
}
