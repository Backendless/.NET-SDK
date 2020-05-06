using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationCreate : Operation
  {
    public OperationCreate()
    {
    }

    public OperationCreate( OperationType operationType, String table, String opResultId, Dictionary<String, Object> payload ) 
                                                                             : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName("payload")]
    public override Object Payload{ get; set; }
  }
}
