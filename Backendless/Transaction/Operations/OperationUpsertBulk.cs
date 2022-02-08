using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpsertBulk : Operation
  {
    public OperationUpsertBulk()
    {
    }

    public OperationUpsertBulk( OperationType operationType, String table, String opResultId, List<Dictionary<String, Object>> payload )
                                                        : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public override Object Payload { get; set; }
  }
}
