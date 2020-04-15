using BackendlessAPI.Transaction.Payload;
using System;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationAddRelation : Operation
  {
    private Object payload;
    
    public OperationAddRelation()
    {
    }

    public OperationAddRelation( OperationType operationType, String table, String opResultId, Relation payload )
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
