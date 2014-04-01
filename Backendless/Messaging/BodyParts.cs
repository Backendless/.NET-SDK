using System;
using Weborb.Service;

namespace BackendlessAPI.Messaging
{
  public class BodyParts
  {
    private String textMessage;
    private String htmlMessage;

    public BodyParts( String textMessage, String htmlMessage )
    {
      this.textMessage = textMessage;
      this.htmlMessage = htmlMessage;
    }

    [SetClientClassMemberName( "textMessage" )]
    public String TextMessage
    {
      get
      {
        return textMessage;
      }
    }

    [SetClientClassMemberName( "htmlMessage" )]
    public String HtmlMessage
    {
      get
      {
        return htmlMessage;
      }
    }
  }
}
