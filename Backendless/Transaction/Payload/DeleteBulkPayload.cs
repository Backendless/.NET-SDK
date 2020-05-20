using System;

namespace BackendlessAPI.Transaction.Payload
{
  class DeleteBulkPayload : Selector
  {
    public DeleteBulkPayload() : base()
    {
    }

    public DeleteBulkPayload( String conditional, Object unconditional ) : base( conditional, unconditional )
    {
    }
  }
}
