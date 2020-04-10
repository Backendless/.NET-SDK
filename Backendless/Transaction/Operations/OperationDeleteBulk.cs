using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDeleteBulk : Operation<List<Object>>
  {
    private List<Object> payload;

    public OperationDeleteBulk()
    {
    }

    public OperationDeleteBulk( OperationType operationType, String table, String opResultId, List<Object> payload ) 
                                                                          : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override List<Object> Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
