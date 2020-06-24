using System;
using BackendlessAPI.Transaction.Payload;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDelete : Operation
  {
    public OperationDelete()
    { 
    }

    public OperationDelete( OperationType operationType, String table, String opResultId, Object payload ) 
                                                                : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public override Object Payload { get; set; }
  }
}
