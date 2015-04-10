using System;

namespace BackendlessAPI.Persistence.Security
{
  public class Find : AbstractDataPermission
  {
    protected override PersistenceOperation GetOperation()
    {
      return PersistenceOperation.FIND;
    }
  }
}
