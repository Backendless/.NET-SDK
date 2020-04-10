using System;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationFind<T> : Operation<T>
  {
    private T payload;

    public OperationFind()
    {
    }

    public OperationFind( OperationType operationType, String table, String opResultId, T payload )
                                                         : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override T Payload
    {
      get => payload;
      set => payload = value;
    }
  }
}
