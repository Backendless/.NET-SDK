using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpdateBulk : Operation
  {
    private Object payload;

    public OperationUpdateBulk()
    {
    }

    public OperationUpdateBulk( OperationType operationType, String table, String opResultId, UpdateBulkPayload payload )
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
