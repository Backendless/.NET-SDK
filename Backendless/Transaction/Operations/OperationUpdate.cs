using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpdate : Operation<Dictionary<String, Object>>
  {
    private Dictionary<String, Object> payload;

    public OperationUpdate()
    {
    }

    public OperationUpdate( OperationType operationType, String table, String opResultId, Dictionary<String, Object> payload )
    : base( operationType, table, opResultId)
    {
      this.payload = payload;
    }

    public override Dictionary<String, Object> Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
