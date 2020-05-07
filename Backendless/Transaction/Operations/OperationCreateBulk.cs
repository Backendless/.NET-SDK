using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationCreateBulk : Operation
  {
    public OperationCreateBulk()
    {
    }

    public OperationCreateBulk( OperationType operationType, String table, String opResultId, List<Dictionary<String, Object>> payload )
                                                                                              : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public override Object Payload { get; set; }
  }
}
