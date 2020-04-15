using System;
using BackendlessAPI.Transaction.Operations;

namespace BackendlessAPI.Transaction
{
  public class TransactionOperationError
  {
    private Operation operation;
    private String message;

    public TransactionOperationError()
    {
    }

    public TransactionOperationError( Operation operation, String message )
    {
      this.operation = operation;
      this.message = message;
    }

    public String Message
    {
      get => message;
      set => message = value;
    }

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
      return "TransactionOperationError{operation=" + operation + ", message=" + message + "}";
    }
  }
}
