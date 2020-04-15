using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using Weborb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weborb.Types;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkExecutorImpl : UnitOfWorkExecutor
  {
    private static String TRANSACTION_MANAGER_SERVER_ALIAS = "com.backendless.services.transaction.TransactionService";

    private UnitOfWork unitOfWork;
    private Dictionary<String, Type> clazzes;

    internal UnitOfWorkExecutorImpl( UnitOfWork unitOfWork, Dictionary<String, Type> clazzes )
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
      foreach( KeyValuePair<String, Type> entry in clazzes )
        Types.AddClientClassMapping( entry.Key, entry.Value );

      if( isAsync )
        Invoker.InvokeAsync<UnitOfWorkResult>( TRANSACTION_MANAGER_SERVER_ALIAS, "execute", args, false, callback );
      else
        return Invoker.InvokeSync<UnitOfWorkResult>( TRANSACTION_MANAGER_SERVER_ALIAS, "execute", args, false );

      return null;
    }
  }
}
