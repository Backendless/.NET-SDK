using System;
using BackendlessAPI.Exception;

namespace BackendlessAPI.Async
{
  public abstract class Fault : Result<BackendlessFault>
  {
  }
}
