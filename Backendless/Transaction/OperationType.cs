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
    UPSERT,
    UPSERT_BULK,
    DELETE,       
    DELETE_BULK,
    FIND,
    ADD_RELATION,            
    SET_RELATION,
    DELETE_RELATION
  };

  public class OperationTypeUtil
  {
    internal static String OperationName
    {
      get;
      private set;
    }

    internal static String GetOperationName( OperationType opType )
    {
      OperationName = opType.ToString().ToLower();

      if( OperationName.Contains( "_b" ) )
        return OperationName.Replace( "_b", "B" );

      else if( OperationName.Contains( "d_r" ) )
        return OperationName.Replace( "d_r", "dToR" );

      else if( OperationName.Contains( "_r" ) )
        return OperationName.Replace( "_r", "R" );

      return OperationName;
    }

    internal static ReadOnlyCollection<OperationType> supportCollectionEntityDescriptionType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.FIND } );

    internal static ReadOnlyCollection<OperationType> supportListIdsResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.CREATE_BULK, OperationType.UPSERT_BULK } );

    internal static ReadOnlyCollection<OperationType> supportDeletionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.DELETE } );

    internal static ReadOnlyCollection<OperationType> supportIntResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.UPDATE_BULK, OperationType.DELETE_BULK,
              OperationType.ADD_RELATION, OperationType.SET_RELATION, OperationType.DELETE_RELATION } );

    internal static ReadOnlyCollection<OperationType> supportEntityDescriptionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { OperationType.CREATE, OperationType.UPDATE, OperationType.UPSERT } );
  }
}
