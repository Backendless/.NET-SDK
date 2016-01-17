using System;
using BackendlessAPI.Persistence.Security;

namespace BackendlessAPI.Persistence
{
  public class DataPermission
  {
    public static readonly Find FIND = new Find();
    public static readonly Update UPDATE = new Update();
    public static readonly Remove REMOVE = new Remove();
  }
}
