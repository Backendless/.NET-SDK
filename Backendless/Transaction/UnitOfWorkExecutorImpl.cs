using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkExecutorImpl : UnitOfWorkExecutor
  {
    private static String TRANSACTION_MANAGER_SERVER_ALIAS = "com.backendless.services.transaction.TransactionService";

    private UnitOfWork unitOfWork;
    private Dictionary<String, Object> clazzes;

    UnitOfWorkExecutorImpl( UnitOfWork unitOfWork, Dictionary<String, Object> clazzes )
    {
      this.unitOfWork = unitOfWork;
      this.clazzes = clazzes;
    }

    public UnitOfWorkResult Execute()
    {
      return Execute( null, false );
    }

    public void Execute( AsyncCallback<UnitOfWorkResult> callback )
    {
      Execute( callback, true );
    }

    private UnitOfWorkResult Execute( AsyncCallback<UnitOfWorkResult> callback, bool isAsync )
    {
      if( unitOfWork.Operations == null || unitOfWork.Operations.Count == 0 )
        throw new ArgumentException( ExceptionMessage.LIST_OPERATIONS_NULL );

      Object[] args = new Object[] { unitOfWork };

      if( isAsync )
        Invoker.InvokeAsync<UnitOfWorkResult>( TRANSACTION_MANAGER_SERVER_ALIAS, "execute", args, callback );
      else
        return Invoker.InvokeSync<UnitOfWorkResult>( TRANSACTION_MANAGER_SERVER_ALIAS, "execute", args );

      return null;
    }
  }
}
