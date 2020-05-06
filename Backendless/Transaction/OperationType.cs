using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public enum OperationType
  {
    CREATE,
    CREATE_BULK,
    UPDATE,
    UPDATE_BULK,
    DELETE,       
    DELETE_BULK,
    FIND,
    ADD_RELATION,            
    SET_RELATION,
    DELETE_RELATION
  };

  public class OperationTypeUtil
  {
    private static String operationName;
    internal static String GetOperationName( OperationType opType )
    {
      operationName = opType.ToString().ToLower();

      if( operationName.Contains( "_b" ) )
        return operationName.Replace( "_b", "B" );

      else if( operationName.Contains( "d_r" ) )
        return operationName.Replace( "d_r", "dToR" );

      else if( operationName.Contains( "_r" ) )
        return operationName.Replace( "_r", "R" );

      return operationName;
    }

    internal static ReadOnlyCollection<OperationType> supportCollectionEntityDescriptionType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.FIND } );

    internal static ReadOnlyCollection<OperationType> supportListIdsResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.CREATE_BULK } );

    internal static ReadOnlyCollection<OperationType> supportDeletionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.DELETE } );

    internal static ReadOnlyCollection<OperationType> supportIntResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.UPDATE_BULK, OperationType.DELETE_BULK,
              OperationType.ADD_RELATION, OperationType.SET_RELATION, OperationType.DELETE_RELATION } );

    internal static ReadOnlyCollection<OperationType> supportEntityDescriptionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.CREATE, OperationType.UPDATE } );
  }
}
