namespace BackendlessAPI.Transaction
{
  public class OperationResult<T>
  {
    private OperationType operationType;
    private T result;

    public OperationResult()
    {
    }

    internal OperationResult( OperationType operationType, T result )
    {
      this.operationType = operationType;
      this.result = result;
    }

    internal OperationType OperationType
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
