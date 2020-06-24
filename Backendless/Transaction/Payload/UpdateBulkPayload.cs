using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Payload
{
  class UpdateBulkPayload : Selector
  {
    public UpdateBulkPayload() : base()
    {
    }

    public UpdateBulkPayload(String conditional, Object unconditional, Dictionary<String, Object> changes )
                                                                       : base( conditional, unconditional )
    {
      Changes = changes;
    }

    [SetClientClassMemberName("changes")]
    public Dictionary<String, Object> Changes { get; set; }

    [SetClientClassMemberName("query")]
    public Object Query{ get; set; }
  }
}
