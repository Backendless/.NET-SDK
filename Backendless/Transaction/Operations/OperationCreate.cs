using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class OperationCreate : Operation<Dictionary<String, Object>>
  {
    private Dictionary<String, Object> payload;

    public OperationCreate()
    {
    }

    OperationCreate( OperationType operationType, String table, String opResultId, Dictionary<String, Object> payload ) 
                                                                             : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override Dictionary<string, object> Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
