using BackendlessAPI.Transaction.Payload;
using System;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationAddRelation : Operation
  {    
    public OperationAddRelation()
    {
    }

    public OperationAddRelation( OperationType operationType, String table, String opResultId, Relation payload )
                                                                       : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName("payload")]
    public new Relation Payload { get; set; }
  }
}
