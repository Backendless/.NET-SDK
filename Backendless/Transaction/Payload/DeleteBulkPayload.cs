using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
