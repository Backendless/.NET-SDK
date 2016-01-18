using System;
using System.Collections.Generic;
using System.Threading;
using Weborb.Util.Logging;
using Weborb.V3Types;

namespace Weborb.Client
{
  public class Subscription
  {
    public const String ReceiveMessagesError = "Unable to receive messages.";
    public delegate void SubscribedHandler();
    public event SubscribedHandler Subscribed;
    public string ClientId;

    private String _subTopic;
    private String _selector;
    //private object _responder;
    private static Object _staticLock = new Object();
    private Boolean _isSubscribed;
    private Engine _engine;
    private bool _isSubscriptionInProgress;
    private AutoResetEvent SubscritionEndedEvent = new AutoResetEvent(false);

    internal void InvokeSubscribed()
    {
      SubscribedHandler handler = Subscribed;
      if ( handler != null ) handler();
    }

    public static String GetIdBySubTopicSelector(String subTopic, String selector)
    {
      return subTopic + selector;
    }

    public Subscription(String subTopic, String selector, Engine engine)
    {
      SubTopic = subTopic;
      _selector = selector;
      _engine = engine;
      ClientId = Guid.NewGuid().ToString();
    }

    public void SetResponder<T>(Responder<T> responder)
    {
      _engine.SetResponder(ClientId, responder);
    }

    public Responder<T> GetResponder<T>()
    {
      return _engine.GetResponder<T>(ClientId);
    }

    public String SubTopic
    {
      get { return _subTopic; }
      set { _subTopic = value; }
    }

    public String Selector
    {
      get { return _selector; }
    }

    public Boolean IsSubscribed
    {
      get { return _isSubscribed; }
    }

    public String Getid()
    {
      return GetIdBySubTopicSelector(SubTopic, Selector);
    }

    public void Unsubscribe()
    {
      if (!_isSubscribed)
        return;

      CommandMessage message = new CommandMessage();
      message.operation = CommandMessage.UNSUBSCRIBE_OPERATION;
      InitCommandMessage(message, SubTopic, Selector, _engine.IdInfo, ClientId);
      _engine.SendRequest(message, new Responder<object>(
                                     o =>
                                     {
                                       _isSubscribed = false;
                                       _engine.OnUnsubscribed(ClientId);

                                       if ( Log.isLogging( LoggingConstants.INFO ) )
                                         Log.log( LoggingConstants.INFO, "Unsubscribed subscription with client id - " + ClientId );
                                     },
                                     fault => { throw new Exception(fault.Message); }
                                     )
        );
    }

    public void Subscribe<T>()
    {
      ThreadPool.QueueUserWorkItem(
        state =>
          {
            lock (_staticLock)
            {
              if (_engine.IdInfo.DsId == null)
              {
                InitDsId<T>();
                return;
              }

              AutoResetEvent autoResetEvent = new AutoResetEvent(false);

              _isSubscriptionInProgress = true;

              if (IsSubscribed)
                return;

              CommandMessage message = new CommandMessage();
              message.operation = CommandMessage.SUBSCRIBE_OPERATION;
              InitCommandMessage(message, SubTopic, Selector, _engine.IdInfo, ClientId);
              _engine.SendRequest(message, new Responder<T>(
                                             o =>
                                               {
                                                 autoResetEvent.Set();

                                                 if (Log.isLogging(LoggingConstants.INFO))
                                                   Log.log(LoggingConstants.INFO, "Client " + ClientId + " subscribed");

                                                 _isSubscribed = true;
                                                 _engine.OnSubscribed(SubTopic, Selector, ClientId);
                                               },
                                             fault => { throw new Exception(fault.Message); }
                                             )
                );
              WaitHandle.WaitAll(new WaitHandle[] {autoResetEvent}, 5000);
            }
          });
    }

    public static void InitCommandMessage(CommandMessage message, String subTopic, String selector, IdInfo idInfo, string clientId)
    {
      message.destination = idInfo.Destination;
      message.clientId = clientId;
      Dictionary<String, String> headers = new Dictionary<String, String>();
      headers.Add("DSId", idInfo.DsId);

      if (selector != null)
        headers.Add(CommandMessage.DSSELECTOR, selector);

      if (subTopic != null)
        headers.Add("DSSubtopic", subTopic);

      message.headers = headers;
    }

    protected void InitDsId<T>()
    {
      CommandMessage message = new CommandMessage();
      message.operation = CommandMessage.CLIENT_PING_OPERATION;
      _engine.SendRequest(message, new Responder<object>(o=>Subscribe<T>(), null));
    }
  }
}
