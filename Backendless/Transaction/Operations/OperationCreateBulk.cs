using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationCreateBulk : Operation
  {
    private Object payload;
    public OperationCreateBulk()
    {
    }

    public OperationCreateBulk( OperationType operationType, String table, String opResultId, List<Dictionary<String, Object>> payload )
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
