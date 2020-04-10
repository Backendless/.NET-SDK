using BackendlessAPI.Transaction.Payload;
using System;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationAddRelation : Operation<Relation>
  {
    private Relation payload;
    
    public OperationAddRelation()
    {
    }

    public OperationAddRelation( OperationType operationType, String table, String opResultId, Relation payload )
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
