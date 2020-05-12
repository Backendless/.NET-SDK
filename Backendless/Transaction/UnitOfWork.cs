using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWork : IUnitOfWork
  {
    public static String REFERENCE_MARKER = "___ref";
    public static String OP_RESULT_ID = "opResultId";
    public static String RESULT_INDEX = "resultIndex";
    public static String PROP_NAME = "propName";

    internal IsolationLevelEnum transactionIsolation = IsolationLevelEnum.REPEATABLE_READ;
    public readonly LinkedList<Operation> operations = new LinkedList<Operation>();
    private readonly List<String> opResultIdStrings;

    private UnitOfWorkCreate unitOfWorkCreate;
    private UnitOfWorkDelete unitOfWorkDelete;
    private UnitOfWorkUpdate unitOfWorkUpdate;
    private UnitOfWorkFind unitOfWorkFind;
    private UnitOfWorkAddRelation unitOfWorkAddRelation;
    private UnitOfWorkSetRelation unitOfWorkSetRelation;
    private UnitOfWorkExecutor unitOfWorkExecutor;

    public UnitOfWork()
    {
      Dictionary<String, Type> clazzes = new Dictionary<String, Type>();
      opResultIdStrings = new List<String>();
      OpResultIdGenerator opResultIdGenerator = new OpResultIdGenerator( opResultIdStrings );
      unitOfWorkCreate = new UnitOfWorkCreateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkDelete = new UnitOfWorkDeleteImpl( operations, opResultIdGenerator );
      unitOfWorkUpdate = new UnitOfWorkUpdateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkFind = new UnitOfWorkFindImpl( operations, opResultIdGenerator );
      RelationOperation relationOperation = new RelationOperationImpl( operations, opResultIdGenerator );
      unitOfWorkAddRelation = new UnitOfWorkAddRelationImpl( relationOperation );
      unitOfWorkSetRelation = new UnitOfWorkSetRelationImpl( relationOperation );
      unitOfWorkExecutor = new UnitOfWorkExecutorImpl( this, clazzes );
    }

    public List<String> GetOpResultIdStrings()
    {
      return opResultIdStrings;
    }

    public UnitOfWorkResult Execute()
    {
      return unitOfWorkExecutor.Execute();
    }

    public void Execute( AsyncCallback<UnitOfWorkResult> callback )
    {
      unitOfWorkExecutor.Equals( callback );
    }

    public OpResult Create<E>( E instance ) where E : class
    {
      return unitOfWorkCreate.Create( instance );
    }

    public OpResult Create( String tableName, Dictionary<String, Object> objectMap )
    {
      return unitOfWorkCreate.Create( tableName, objectMap );
    }

    public OpResult BulkCreate<E>( List<E> instances ) where E : class
    {
      return unitOfWorkCreate.BulkCreate( instances );
    }

    public OpResult BulkCreate( String tableName, List<Dictionary<String, Object>> arrayOfObjectMaps )
    {
      return unitOfWorkCreate.BulkCreate( tableName, arrayOfObjectMaps );
    }

    public OpResult Delete<E>( E instance ) where E : class
    {
      return unitOfWorkDelete.Delete( instance );
    }

    public OpResult Delete( string tableName, Dictionary<string, object> objectMap )
    {
      return unitOfWorkDelete.Delete( tableName, objectMap );
    }

    public OpResult Delete( string tableName, string objectId )
    {
      return unitOfWorkDelete.Delete( tableName, objectId );
    }

    public OpResult Delete( OpResult result )
    {
      return unitOfWorkDelete.Delete( result );
    }

    public OpResult Delete( OpResultValueReference resultIndex )
    {
      return unitOfWorkDelete.Delete( resultIndex );
    }

    public OpResult BulkDelete<E>( List<E> instances ) where E : class
    {
      return unitOfWorkDelete.BulkDelete( instances );
    }

    public OpResult BulkDelete( string tableName, string[] objectIdValues )
    {
      return unitOfWorkDelete.BulkDelete( tableName, objectIdValues );
    }

    public OpResult BulkDelete( string tableName, List<Dictionary<string, object>> arrayOfObjects )
    {
      return unitOfWorkDelete.BulkDelete( tableName, arrayOfObjects );
    }

    public OpResult BulkDelete( string tableName, string whereClause )
    {
      return unitOfWorkDelete.BulkDelete( tableName, whereClause );
    }

    public OpResult BulkDelete( OpResult result )
    {
      return unitOfWorkDelete.BulkDelete( result );
    }

    public OpResult Update<E>( E instance )
    {
      return unitOfWorkUpdate.Update( instance );
    }

    public OpResult Update( string tableName, Dictionary<string, object> objectMap )
    {
      return unitOfWorkUpdate.Update( tableName, objectMap );
    }

    public OpResult Update( OpResult result, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResult result, string propertyName, object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult Update( OpResultValueReference result, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResultValueReference result, string propertyName, object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult BulkUpdate( string tableName, string whereClause, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, whereClause, changes );
    }

    public OpResult BulkUpdate( string tableName, List<string> objectsForChanges, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, objectsForChanges, changes );
    }

    public OpResult BulkUpdate( OpResult objectIdsForChanges, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( objectIdsForChanges, changes );
    }

    public OpResult Find( String tableName, DataQueryBuilder queryBuilder )
    {
      return unitOfWorkFind.Find( tableName, queryBuilder );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( string parentTable, Dictionary<string, object> parentObject, string columnName, List<E> childrenInstance )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenInstance);
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, children );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( string parentTable, string parentObjectId, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenMaps );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, children );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E, U>( E parentObject, string columnName, List<U> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult addToRelation( OpResult parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResult parentObject, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResultValueReference parentObject, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, OpResult children )
    {
        return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children);
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( string parentTable, Dictionary<string, object> parentObject, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, children );
    }

    public OpResult SetRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, whereClauseForChildren);
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( string parentTable, string parentObjectId, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, children );
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, whereClauseForChildren);
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E, U>( E parentObject, string columnName, List<U> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResult parentObject, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances);
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, string[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResultValueReference parentObject, string columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, string whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }
  }
}
