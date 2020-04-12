using System;
using BackendlessAPI.Persistence;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkFind
  {
    OpResult Find( String tableName, DataQueryBuilder queryBuilder );
  }
}
