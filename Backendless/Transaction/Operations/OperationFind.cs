using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
