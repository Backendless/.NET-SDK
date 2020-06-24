using System;
using BackendlessAPI.Transaction.Payload;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpdateBulk : Operation
  {
    public OperationUpdateBulk()
    {
    }

    public OperationUpdateBulk( OperationType operationType, String table, String opResultId, UpdateBulkPayload payload )
                                                                               : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName("payload")]
    public override Object Payload { get; set; }
  }
}
