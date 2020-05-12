using System;
using BackendlessAPI.Transaction.Operations;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class TransactionOperationError
  {
    private Operation operation;

    public TransactionOperationError()
    {
    }

    public TransactionOperationError( Operation operation, String message )
    {
      this.operation = operation;
      Message = message;
    }

    [SetClientClassMemberName( "message" )]
    public String Message { get; set; }

    public Operation GetOperation()
    {
      return operation;
    }
    public void SetOperation( Operation operation )
    {
      this.operation = operation;
    }

    public override string ToString()
    {
      return "TransactionOperationError{operation=" + operation + ", message=" + Message + "}";
    }
  }
}
