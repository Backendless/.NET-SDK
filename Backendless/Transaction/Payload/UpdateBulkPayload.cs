using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction.Payload
{
  class UpdateBulkPayload : Selector
  {
    private Dictionary<String, Object> changes;

    public UpdateBulkPayload() : base()
    {
    }

    public UpdateBulkPayload(String conditional, Object unconditional, Dictionary<String, Object> changes )
                                                                       : base( conditional, unconditional )
    {
      this.changes = changes;
    }

    public Dictionary<String, Object> Changes
    {
      get => changes;
      set => changes = value;
    }
  }
}
