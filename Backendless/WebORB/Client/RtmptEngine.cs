using Weborb.Messaging.Net.RTMPT;

namespace Weborb.Client
{
  class RtmptEngine : BaseRtmpEngine
  {
    private const string PROTOCOL = "rtmpt://";
    private const int DEFAULT_PORT = 80;


    public RtmptEngine(string gateway, IdInfo idInfo) : base(gateway, idInfo)
    {
    }

    protected override string Protocol
    {
      get { return PROTOCOL; }
    }

    protected override int DefaultPort
    {
      get { return DEFAULT_PORT; }
    }

    protected override void Init(string host, int port, string app)
    {
      RTMPClient = new RTMPTClient(host, port, app);
    }
  }
}
