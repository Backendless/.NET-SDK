using System;
using BackendlessAPI.RT.Users;
using Weborb.Service;

namespace BackendlessAPI.RT.Command
{
  public class Command<T>
  {
    [SetClientClassMemberName( "type" )]
    public String Type
    {
      get;
      set;
    }

    [SetClientClassMemberName( "data" )]
    public T Data
    {
      get;
      set;
    }

    [SetClientClassMemberName( "userInfo" )]
    public UserInfo UserInfo
    {
      get;
      set;
    }

    public override String ToString()
    {
      return "RTCommand{" + "dataType=" + typeof( T ) + ", type='" + Type + '\'' + ", data=" + Data + ", userInfo=" + UserInfo + '}';
    }
  }
}
