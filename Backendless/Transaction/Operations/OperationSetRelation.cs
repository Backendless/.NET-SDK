using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationSetRelation : Operation
  {
    private Object payload;

    public OperationSetRelation()
    {
    }

    public OperationSetRelation( OperationType operationType, String table, String opResultId, Relation payload )
                                                                       : base( operationType, table, opResultId )
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
