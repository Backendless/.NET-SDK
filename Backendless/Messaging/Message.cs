using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class Message
  {
    private Dictionary<string, string> _headers;

    [SetClientClassMemberName( "messageId" )]
    public string MessageId { get; set; }

    [SetClientClassMemberName( "data" )]
    public object Data { get; set; }

    [SetClientClassMemberName( "headers" )]
    public Dictionary<string, string> Headers
    {
      get { return _headers ?? (_headers = new Dictionary<string, string>()); }
      set { _headers = value; }
    }

    [SetClientClassMemberName( "publisherId" )]
    public string PublisherId { get; set; }

    [SetClientClassMemberName( "timestamp" )]
    public long Timestamp { get; set; }
  }
}