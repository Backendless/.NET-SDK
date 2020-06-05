namespace BackendlessAPI.Transaction
{
  interface IUnitOfWork : UnitOfWorkCreate, UnitOfWorkUpdate, UnitOfWorkDelete, UnitOfWorkFind
  {   
  }
}
