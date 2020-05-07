using System;
using BackendlessAPI.Transaction.Payload;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationSetRelation : Operation
  {
    public OperationSetRelation()
    {
    }

    public OperationSetRelation( OperationType operationType, String table, String opResultId, Relation payload )
                                                                       : base( operationType, table, opResultId )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public override Object Payload { get; set; }
  }
}
