using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationDeleteRelation : Operation
  {
    private Object payload;

    public OperationDeleteRelation()
    {
    }

    public OperationDeleteRelation( OperationType operationType, String table, String objectIds, Relation payload )
                                                                          : base( operationType, table, objectIds )
    {
      this.payload = payload;
    }

    public override Object Payload
    {
    get => payload;
    set => payload = value;
    }
  }
}
