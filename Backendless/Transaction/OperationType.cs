using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class OperationType
  {
    private String operationName;
    private OperationType( string operationName )
    {
      this.operationName = operationName;
    }

    [SetClientClassMemberName("operationName")]
    public String OperationName
    {
      get => operationName;
    }

    internal static OperationType CREATE { get => new OperationType("create"); }
    internal static OperationType CREATE_BULK { get => new OperationType( "createBulk" ); }
    internal static OperationType UPDATE { get => new OperationType( "update" ); }
    internal static OperationType UPDATE_BULK { get => new OperationType( "updateBulk" ); }
    internal static OperationType DELETE { get => new OperationType( "delete" ); }
    internal static OperationType DELETE_BULK { get => new OperationType( "deleteBulk" ); }
    internal static OperationType FIND { get => new OperationType( "find" ); }
    internal static OperationType ADD_RELATION{ get => new OperationType( "addToRelation" ); }
    internal static OperationType SET_RELATION { get => new OperationType( "setRelation" ); }
    internal static OperationType DELETE_RELATION { get => new OperationType( "deleteRelation" ); }


    internal static ReadOnlyCollection<OperationType> supportCollectionEntityDescriptionType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { FIND } );

    internal static ReadOnlyCollection<OperationType> supportListIdsResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { CREATE_BULK } );

    internal static ReadOnlyCollection<OperationType> supportDeletionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { DELETE } );

    internal static ReadOnlyCollection<OperationType> supportIntResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { UPDATE_BULK, DELETE_BULK, ADD_RELATION, SET_RELATION, DELETE_RELATION } );

    internal static ReadOnlyCollection<OperationType> supportEntityDescriptionResultType = new ReadOnlyCollection<OperationType>
              ( new List<OperationType> { CREATE, UPDATE } );
  }
}
