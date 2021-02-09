using System;

namespace BackendlessAPI.Xamarin
{
  internal interface ILocalStorage
  {
    Boolean HasData { get; }

    void SaveData();

    String GetData();

    void DeleteFiles();
  }
}
