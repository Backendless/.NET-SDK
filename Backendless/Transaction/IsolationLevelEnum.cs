namespace BackendlessAPI.Transaction
{
  public enum LevelEnum
  {
    READ_UNCOMMITTED = 1,
    READ_COMMITTED = 2,
    REPEATABLE_READ = 4,
    SERIALIZABLE = 8
  }

  public class IsolationLevelEnum
  {
    private int operationId;
    
    public int OperationId
    {
      get => operationId;
    }

    IsolationLevelEnum( LevelEnum operationId )
    {
      this.operationId = (int) operationId;
    }
  }
}
