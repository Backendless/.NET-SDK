namespace BackendlessAPI.Transaction
{
  public enum IsolationLevelEnum
  {
    READ_UNCOMMITTED = 1,
    READ_COMMITTED = 2,
    REPEATABLE_READ = 4,
    SERIALIZABLE = 8
  }

  public class LevelEnum
  {
    private int operationId;
    
    public int OperationId
    {
      get => operationId;
    }

    LevelEnum( IsolationLevelEnum operationId )
    {
      this.operationId = (int) operationId;
    }
  }
}
