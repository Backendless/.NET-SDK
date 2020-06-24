using System;
using BackendlessAPI.Transaction.Payload;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDeleteRelation : Operation
  {
    public OperationDeleteRelation()
    {
    }

    public OperationDeleteRelation( OperationType operationType, String table, String objectIds, Relation payload )
                                                                          : base( operationType, table, objectIds )
    {
      Payload = payload;
    }

    [SetClientClassMemberName( "payload" )]
    public new Relation Payload { get; set; }
  }
}
