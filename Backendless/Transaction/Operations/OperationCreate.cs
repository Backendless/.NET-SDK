using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationCreate : Operation
  {
    private Object payload;

    public OperationCreate()
    {
    }

    public OperationCreate( OperationType operationType, String table, String opResultId, Dictionary<String, Object> payload ) 
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
