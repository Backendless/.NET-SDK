using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace BackendlessAPI
{
  public enum LevelEnum
  {
    REPEATABLE_READ/*( Connection.RANSACTION_REPEATABLE_READ )*/,
    READ_COMMITTED/*( Connection.TRANSACTION_READ_COMMITTED )*/,
    READ_UNCOMMITTED/*( Connection.TRANSACTION_READ_UNCOMMITTED )*/,
    SERIALIZABLE/*( Connection.TRANSACTION_SERIALIZABLE )*/
  }
  public class IsolationLevelEnum
  {
    private int operationId;
    
    public int OperationId
    {
      get
      {
        return operationId;
      }
    }

    IsolationLevelEnum( int operationId )
    {
      this.operationId = operationId;
    }
  }
}
