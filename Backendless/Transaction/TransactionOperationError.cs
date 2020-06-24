using System;
using BackendlessAPI.Transaction.Operations;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class TransactionOperationError
  {
    public TransactionOperationError()
    {
    }

    public TransactionOperationError( Operation operation, String message )
    {
      Operation = operation;
      Message = message;
    }

    [SetClientClassMemberName( "message" )]
    public String Message { get; set; }

    public Operation Operation{ get; set; }

    public override string ToString()
    {
      return "TransactionOperationError{operation=" + Operation + ", message=" + Message + "}";
    }
  }
}
