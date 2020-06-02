using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class OperationResult
  {
    public OperationResult()
    {
    }

    public OperationResult( OperationType operationType, object result )
    {
      OperationType = operationType;
      Result = result;
    }

    [SetClientClassMemberName("operationType")]
    public OperationType OperationType { get; set; }

    [SetClientClassMemberName("result")]
    public object Result { get; set; }
    
  }
}
