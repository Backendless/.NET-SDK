namespace BackendlessAPI.Transaction
{
  interface IUnitOfWork : UnitOfWorkCreate, UnitOfWorkUpdate, UnitOfWorkDelete, UnitOfWorkFind, UnitOfWorkAddRelation, UnitOfWorkSetRelation, UnitOfWorkDeleteRelation, UnitOfWorkExecutor
  {   
  }
}
