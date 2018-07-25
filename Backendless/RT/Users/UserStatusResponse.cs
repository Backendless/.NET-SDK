using System;
using System.Collections;
using Weborb.Service;

namespace BackendlessAPI.RT.Users
{
  public class UserStatusResponse
  {
    [SetClientClassMemberName( "status" )]
    public UserStatus Status
    {
      get;
      set;
    }

    [SetClientClassMemberName( "data" )]
    public UserInfo[] Data
    {
      get;
      set;
    }

    public override String ToString()
    {
      #if NET_35
      return "UserStatusResponse{" + "status=" + Status + '}';
      #else
      return "UserStatusResponse{" + "status=" + Status + ", data=" + String.Join( ", ", (IEnumerable) Data ) + '}';
      #endif
    }
  }
}
