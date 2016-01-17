using System;

namespace BackendlessAPI.Persistence.Security
{
  public class Remove : AbstractDataPermission
  {
    protected override PersistenceOperation GetOperation()
    {
      return PersistenceOperation.REMOVE;
    }
  }
}
