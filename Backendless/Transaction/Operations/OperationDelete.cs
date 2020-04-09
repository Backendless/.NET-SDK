using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction
{
  class OperationDelete : Operation<DeleteBulkPayload>
  {
    private DeleteBulkPayload payload;

    public OperationDelete()
    { 
    }

    public OperationDelete( OperationType operationType, String table, String opResultId, DeleteBulkPayload payload ) 
                                                                : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override DeleteBulkPayload Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
