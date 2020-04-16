using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDelete : Operation
  {
    private Object payload;

    public OperationDelete()
    { 
    }

    public OperationDelete( OperationType operationType, String table, String opResultId, Object payload ) 
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
