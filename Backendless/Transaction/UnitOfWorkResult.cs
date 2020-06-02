using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWorkResult
  {
    public UnitOfWorkResult()
    {
    }

    public UnitOfWorkResult( bool success, TransactionOperationError error )
    {
      Success = success;
      Error = error;
    }

    [SetClientClassMemberName("success")]
    public bool Success { get; set; }

    [SetClientClassMemberName( "error" )]
    public TransactionOperationError Error { get; set; }

    [SetClientClassMemberName( "results" )]
    public Dictionary<String, OperationResult> Results { get; set; }
    public override string ToString()
    {
      String error = Error != null ? Error.ToString() : "error=null";
      return "UnitOfWorkResult{success=" + Success + ", " + error + ", results=" + Results + "}";
    }
  }
}
