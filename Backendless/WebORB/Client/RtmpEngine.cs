using Weborb.Messaging.Net.RTMP;

namespace Weborb.Client
{
  class RtmpEngine : BaseRtmpEngine
  {
    private const string PROTOCOL = "rtmp://";
    private const int DEFAULT_PORT = 2037;


    public RtmpEngine(string gateway, IdInfo idInfo) : base(gateway, idInfo)
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
      RTMPClient = new RTMPClient(host, port, app);
    }
  }
}
