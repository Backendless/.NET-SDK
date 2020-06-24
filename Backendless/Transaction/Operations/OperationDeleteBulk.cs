using BackendlessAPI.Transaction.Payload;
using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDeleteBulk : Operation
  {
    public OperationDeleteBulk()
    {
    }

    public OperationDeleteBulk( OperationType operationType, String table, String opResultId, DeleteBulkPayload payload ) 
                                                                          : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public new DeleteBulkPayload Payload { get; set; }
  }
}
