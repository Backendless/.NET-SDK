using System;
using System.Collections.Generic;

namespace BackendlessAPI.Persistence
{
  public class DataPermission
  {
    public readonly Find FIND = new Find();
    public readonly Update UPDATE = new Update();
    public readonly Remove REMOVE = new Remove();

    public class Find : AbstractDataPermission
    {
      protected override PersistenceOperation getOperation()
      {
        return PersistenceOperation.FIND;
      }
    }

    public class Update : AbstractDataPermission
    {
      protected override PersistenceOperation getOperation()
      {
        return PersistenceOperation.UPDATE;
      }
    }

    public class Remove : AbstractDataPermission
    {
      protected override PersistenceOperation getOperation()
      {
        return PersistenceOperation.REMOVE;
      }
    }
  }
}
