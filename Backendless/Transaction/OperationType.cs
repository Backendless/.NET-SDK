using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BackendlessAPI.Transaction
{
  public class OperationType
  {
    private OperationType( string operationName )
    {
      this.operationName = operationName;
    }

    private String operationName;
    public String OperationName
    {
      get => operationName;
    }

    public static OperationType CREATE { get => new OperationType("create"); }
    public static OperationType CREATE_BULK { get => new OperationType( "createBulk" ); }
    public static OperationType UPDATE { get => new OperationType( "update" ); }
    public static OperationType UPDATE_BULK { get => new OperationType( "updateBulk" ); }
    public static OperationType DELETE { get => new OperationType( "delete" ); }
    public static OperationType DELETE_BULK { get => new OperationType( "deleteBulk" ); }
    public static OperationType FIND { get => new OperationType( "find" ); }
    public static OperationType ADD_RELATION{ get => new OperationType( "addToRelation" ); }
    public static OperationType SET_RELATION { get => new OperationType( "setRelation" ); }
    public static OperationType DELETE_RELATION { get => new OperationType( "deleteRelation" ); }


    public static ReadOnlyCollection<OperationType> supportCollectionEntityDescriptionType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { FIND } );

    public static ReadOnlyCollection<OperationType> supportListIdsResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { CREATE_BULK } );

    public static ReadOnlyCollection<OperationType> supportDeletionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { DELETE } );

    public static ReadOnlyCollection<OperationType> supportIntResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { UPDATE_BULK, DELETE_BULK, ADD_RELATION, SET_RELATION, DELETE_RELATION } );

    public static ReadOnlyCollection<OperationType> supportEntityDescriptionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { CREATE, UPDATE } );
  }
}
