using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpdate : Operation
  {
    public OperationUpdate()
    {
    }

    public OperationUpdate( OperationType operationType, String table, String opResultId, Dictionary<String, Object> payload )
                                                                                     : base( operationType, table, opResultId)
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public new Dictionary<String, Object> Payload { get; set; }
  }
}
