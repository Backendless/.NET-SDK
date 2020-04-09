using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class OperationCreateBulk : Operation<List<Dictionary<String, Object>>>
  {
    private List<Dictionary<String, Object>> payload;
    public OperationCreateBulk()
    {
    }

    public OperationCreateBulk( OperationType operationType, String table, String opResultId, List<Dictionary<String, Object>> payload )
                                                                                              : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override List<Dictionary<string, object>> Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
