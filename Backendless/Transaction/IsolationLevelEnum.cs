using Weborb.Service;

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
    LevelEnum( IsolationLevelEnum operationId )
    {
      OperationId = (int) operationId;
    }

    [SetClientClassMemberName( "operationId" )]
    public int OperationId
    {
      get;
      private set;
    }
  }
}
