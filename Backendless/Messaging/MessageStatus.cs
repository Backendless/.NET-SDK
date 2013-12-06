using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class MessageStatus
  {
    public MessageStatus()
    {
    }

    public MessageStatus( string messageId )
    {
      this.MessageId = messageId;
    }

    [SetClientClassMemberName( "status" )]
    public PublishStatusEnum Status { get; set; }

    [SetClientClassMemberName( "messageId" )]
    public string MessageId { get; set; }

    [SetClientClassMemberName( "errorMessage" )]
    public string ErrorMessage { get; set; }
  }
}
