using System;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationSetRelation : Operation<Relation>
  {
    private Relation payload;

    public OperationSetRelation()
    {
    }

    public OperationSetRelation( OperationType operationType, String table, String opResultId, Relation payload )
                                                                       : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override Relation Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
