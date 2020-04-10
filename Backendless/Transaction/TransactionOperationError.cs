using System;
using BackendlessAPI.Transaction.Operations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class TransactionOperationError
  {
    private Operation<Object> operation;
    private String message;

    public TransactionOperationError()
    {
    }

    public TransactionOperationError( Operation<Object> operation, String message )
    {
      this.operation = operation;
      this.message = message;
    }

    public String Message
    {
      get => message;
      set => message = value;
    }

    public Operation<Object> GetOperation()
    {
      return operation;
    }
    public void SetOperation( Operation<Object> operation )
    {
      this.operation = operation;
    }

    public override string ToString()
    {
      return "TransactionOperationError{operation=" + operation + ", message=" + message + "}";
    }
  }
}
