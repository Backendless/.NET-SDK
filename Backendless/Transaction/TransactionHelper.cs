using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class TransactionHelper
  {
    private static String LAST_LOGIN_COLUM_NAME = "lastLogin";
    private static String PASSWORD_KEY = "password";
    private static String SOCIAL_ACCOUNT_COLUMN_NAME = "socialAccount";
    private static String USER_STATUS_COLUMN_NAME = "userStatus";

    static void RemoveSystemField( Dictionary<String, Object> changes )
    {
      changes.Remove( LAST_LOGIN_COLUM_NAME );
      changes.Remove( PASSWORD_KEY );
      changes.Remove( SOCIAL_ACCOUNT_COLUMN_NAME );
      changes.Remove( USER_STATUS_COLUMN_NAME );
      changes.Remove( "objectId" );
      changes.Remove( "created" );
      changes.Remove( "updated" );
    }

    static OpResult MakeOpResult( String tableName, String operationResultId, OperationType operationType )
    {
      return new OpResult( tableName, operationResultId, operationType );
    }

    static List<Dictionary<String, Object>> ConvertInstancesToMaps( List<Dictionary<String, Object>> instances )
    {
      if( instances == null )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      List<Dictionary<String, Object>> serializedEntities = new List<Dictionary<String, Object>>();
      foreach( Dictionary<String, Object> entity in instances )
      {
        serializedEntities.Add( entity );
      }

      return serializedEntities;
    }
  }
}
