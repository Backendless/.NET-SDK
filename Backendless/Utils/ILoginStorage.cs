using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendlessAPI.Utils
{
  public interface ILoginStorage
  {
    string UserId
    {
      get;
      set;
    }

    string UserToken
    {
      get;
      set;
    }

    bool HasData
    {
      get;
    }

    void SaveData();
    void DeleteFiles();
  }
}
