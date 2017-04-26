using System;
using System.Collections.Generic;
using System.Text;

namespace BackendlessAPI.Utils
{
  public interface ILoginStorage
  {
    string ObjectId
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
