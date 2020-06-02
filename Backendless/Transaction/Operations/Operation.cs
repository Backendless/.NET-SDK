using System;
using Weborb.Service;

namespace BackendlessAPI.Transaction.Operations
{
  public abstract class Operation
  {
    public Operation()
    {
    }

    public Operation( OperationType operationType, String table, String opResultId )
    {
      OperationType = operationType;
      Table = table;
      OpResultId = opResultId;
    }

    [SetClientClassMemberName( "operationType" )]
    public OperationType OperationType { get; set; }

    [SetClientClassMemberName( "table" )]
    public String Table { get; set; }

    [SetClientClassMemberName( "opResultId" )]
    public String OpResultId { get; set; }
    
    [SetClientClassMemberName("payload")]
    public virtual Object Payload{ get; set; } 

    public override string ToString()
    {
      return "Operation{operationType=" + OperationType + ", table=" + Table + ", opResultId=" + OpResultId + ", payload=" + Payload + "}";
    }
  }
}
