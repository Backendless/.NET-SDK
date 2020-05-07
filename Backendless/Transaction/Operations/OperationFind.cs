using System;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationFind : Operation
  {
    public OperationFind()
    {
    }

    public OperationFind( OperationType operationType, String table, String opResultId, Object payload )
                                                        // : base( operationType, table, opResultId ) 
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public override Object Payload { get; set; }
  }
}
