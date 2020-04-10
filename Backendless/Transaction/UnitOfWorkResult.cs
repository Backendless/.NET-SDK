using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkResult
  {
    private bool success;
    private TransactionOperationError error;
    private Dictionary<String, Object> results;

    public UnitOfWorkResult()
    {
    }

    public UnitOfWorkResult( bool success, TransactionOperationError error )
    {
      this.success = success;
      this.error = error;
    }

    public bool Success
    {
      get => success;
      set => success = value;
    }

    public TransactionOperationError Error
    {
      get => error;
      set => error = value;
    }

    public Dictionary<String, Object> Results
    {
      get => results;
      set => results = value;
    }

    public override string ToString()
    {
      String error = this.error != null ? this.error.ToString() : "error=null";
      return "UnitOfWorkResult{success=" + success + ", " + error + ", results=" + results + "}";
    }
  }
}
