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

    public UnitOfWorkResult( Boolean success, TransactionOperationError error )
    {
      Success = success;
      Error = error;
    }

    [SetClientClassMemberName("success")]
    public Boolean Success { get; set; }

    [SetClientClassMemberName( "error" )]
    public TransactionOperationError Error { get; set; }

    [SetClientClassMemberName( "results" )]
    public Dictionary<String, OperationResult> Results { get; set; }
    public override String ToString()
    {
      String error = Error != null ? Error.ToString() : "error=null";
      return "UnitOfWorkResult{success=" + Success + ", " + error + ", results=" + Results + "}";
    }
  }
}
