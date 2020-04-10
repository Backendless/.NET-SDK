using System;
using System.Collections.Generic;

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
