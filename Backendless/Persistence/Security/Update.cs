using System;

namespace BackendlessAPI.Persistence.Security
{
  public class Update : AbstractDataPermission
  {
    protected override PersistenceOperation GetOperation()
    {
      return PersistenceOperation.UPDATE;
    }
  }
}
