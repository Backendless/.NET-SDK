using System;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationFind : Operation
  {
    private Object payload;

    public OperationFind()
    {
    }

    public OperationFind( OperationType operationType, String table, String opResultId, Object payload )
                                                        // : base( operationType, table, opResultId ) 
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
