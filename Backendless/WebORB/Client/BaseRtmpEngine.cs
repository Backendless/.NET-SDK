using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Messaging.Api.Service;
using Weborb.Messaging.Net.RTMP;
using Weborb.Messaging.Net.RTMP.Event;
using Weborb.Messaging.Net.RTMP.Message;
using Weborb.Messaging.Server.Service;
using Weborb.Reader;
using Weborb.Types;
using Weborb.V3Types;

namespace Weborb.Client
{
  abstract class BaseRtmpEngine : Engine
  {
    private string _host;
    private string _app;
    private int _port;

    protected BaseRTMPClient RTMPClient;

    public virtual BaseRTMPClient RTMP
    {
      get { return RTMPClient; }
    }

    public BaseRtmpEngine(string gateway, IdInfo idInfo) : base(gateway, idInfo)
    {
      string url = gateway.Substring(Protocol.Length, gateway.Length - (Protocol.Length));
      
      int hostSeparatorPos = url.IndexOf("/");
      if(hostSeparatorPos != -1)
      {
        _host = url.Substring(0, hostSeparatorPos);
        _app = url.Substring(hostSeparatorPos + 1, url.Length - hostSeparatorPos - 1);
      }

      int portSeparatorPos = _host.IndexOf(":");
      if(portSeparatorPos != -1)
      {
        _port = int.Parse(_host.Substring(portSeparatorPos + 1, _host.Length - portSeparatorPos - 1));
        _host = _host.Substring(0, portSeparatorPos);
      }
      else
      {
        _port = DefaultPort;
      }

      Init(_host, _port, _app);
    }

    protected abstract string Protocol { get; }
    protected abstract int DefaultPort { get; }
    protected abstract void Init(string host, int port, string app); 

    internal override void Invoke<T>(string className, string methodName, object[] args, IDictionary requestHeaders, IDictionary messageHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo)
    {
      if ( RTMPClient.Connected )
      {
        if ( className == null )
        {
          RTMPClient.invoke(methodName, args, new CallBack<T>(responder));
          return;
        }

        SendRequest(CreateMessageForInvocation(className, methodName, args, messageHeaders), requestHeaders, null, responder, null);
      }
      else
      {
        RTMPClient.ConnectedEvent += call => Invoke<T>( className, methodName, args, requestHeaders, messageHeaders, httpHeaders, responder, asyncStreamSetInfo );
        RTMPClient.Connect();
      }
    }

    public override void SendRequest<T>( V3Message v3Msg, IDictionary requestHeaders, IDictionary httpHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      if (!RTMPClient.Connected)
      {
        RTMPClient.ConnectedEvent += callBack =>SendRequest<T>(v3Msg, requestHeaders, httpHeaders, responder, null );
        RTMPClient.Connect();
        return;
      }
      if (v3Msg.headers == null)
        v3Msg.headers = new Dictionary<object, object>();

      IPendingServiceCall call = new PendingCall("SendRequest");
      call.registerCallback(new CallBack<V3Message>(new Responder<V3Message>(result => ProcessV3Message(result, responder), fault => {})));
      FlexMessage flexPush = new FlexMessage();
      flexPush.setCall(call);
      flexPush.setInvokeId(RTMPClient.GetConnection().getInvokeId());
      flexPush.streamId = RTMPClient.GetConnection().getStreamIdForChannel(3);
      RTMPClient.GetConnection().registerPendingCall(flexPush.getInvokeId(), call);

      flexPush.version = 3;

      //v3Msg.headers.Remove(ORBConstants.IsRTMPChannel);
      flexPush.obj = new Object[] { v3Msg };
      

      Header header = new Header();
      header.setTimer(0);
      header.setChannelId(3);
      header.setDataType((byte)17);
      header.setStreamId(0);

      Packet packet = new Packet(header, flexPush);
      RTMPClient.GetConnection().write(packet);
    }

    internal override void OnSubscribed(string subTopic, string selector, string clientId)
    {
      base.OnSubscribed(subTopic, selector, clientId);
      RTMPClient.setServiceProvider(this);
    }

    public void receive(AsyncMessage obj)
    {
      ProcessV3Message(obj, GetResponder<object>(obj.clientId.ToString()));
    }

    private class CallBack<T> : IPendingServiceCallback
    {
      private Responder<T> _responder;
      public CallBack(Responder<T> responder)
      {
        _responder = responder;
      }

      public void resultReceived(IPendingServiceCall call)
      {
        object result = call.getResult();
        if (result is IAdaptingType)
        {
          IAdaptingType obj = (IAdaptingType) result;
          if (obj.canAdaptTo(typeof(T)))
            _responder.ResponseHandler((T) obj.adapt(typeof(T)));
          else
            _responder.ErrorHandler(new Fault("Wrong generic type", "Responder has wrong generic type"));
        }
        else
        {
          _responder.ResponseHandler((T)result);
        }
      }
    }
  }
}
