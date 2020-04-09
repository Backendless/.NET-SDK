﻿using System;
using BackendlessAPI.Transaction.Payload;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class OperationDeleteRelation : Operation<Relation>
  {
    private Relation payload;

    public OperationDeleteRelation()
    {
    }

    public OperationDeleteRelation( OperationType operationType, String table, String objectIds, Relation payload )
                                                                          : base( operationType, table, objectIds )
    {
      this.payload = payload;
    }

    public override Relation Payload
    {
    get => payload;
    set => payload = value;
    }
  }
}
