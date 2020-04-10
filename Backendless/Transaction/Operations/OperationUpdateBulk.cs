using System;
using BackendlessAPI.Transaction.Payload;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction.Operations
{
  class OperationUpdateBulk : Operation<UpdateBulkPayload>
  {
    private UpdateBulkPayload payload;

    public OperationUpdateBulk()
    {
    }

    public OperationUpdateBulk( OperationType operationType, String table, String opResultId, UpdateBulkPayload payload )
    : base( operationType, table, opResultId )
    {
      this.payload = payload;
    }

    public override UpdateBulkPayload Payload
    {
      get => payload;
      set => payload = value; 
    }
  }
}
