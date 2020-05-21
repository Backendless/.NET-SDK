using BackendlessAPI.Async;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkExecutor
  {
    UnitOfWorkResult Execute();
    void Execute( AsyncCallback<UnitOfWorkResult> callback );
  }
}
