using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class OperationResult<T>
  {
    private OperationType operationType;
    private T result;

    public OperationResult()
    {
    }

    public OperationResult( OperationType operationType, T result )
    {
      this.operationType = operationType;
      this.result = result;
    }

    public OperationType OperationType
    {
      get => operationType;
      set => operationType = value;
    }

    public T Result
    {
      get => result;
      set => result = value;
    }
  }
}
