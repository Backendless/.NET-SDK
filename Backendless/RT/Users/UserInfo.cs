using System;
using Weborb.Service;

namespace BackendlessAPI.RT.Users
{
  public class UserInfo
  {
    [SetClientClassMemberName( "connectionId" )]
    public String ConnectionId
    {
      get;
      set;
    }

    [SetClientClassMemberName( "userId" )]
    public String UserId
    {
      get;
      set;
    }

    public override String ToString()
    {
      return "UserInfo{" + "connectionId='" + ConnectionId + '\'' + ", userId='" + UserId + '\'' + '}';
    }
  }
}
