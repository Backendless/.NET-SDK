using BackendlessAPI.Transaction.Payload;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDeleteBulk : Operation
  {
    private Object payload;

    public OperationDeleteBulk()
    {
    }

    public OperationDeleteBulk( OperationType operationType, String table, String opResultId, DeleteBulkPayload payload ) 
                                                                          : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override Object Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
