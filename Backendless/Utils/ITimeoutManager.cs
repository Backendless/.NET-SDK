using System;
namespace BackendlessAPI.Utils
{
  public interface ITimeoutManager
  {
    int NextTimeout();

    int RepeatedTimes();

    void Reset();
  }
}
