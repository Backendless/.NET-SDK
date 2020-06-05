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
    private UnitOfWorkAddRelationImpl unitOfWorkAddRelation;
    private UnitOfWorkSetRelationImpl unitOfWorkSetRelation;
    private UnitOfWorkDeleteRelationImpl unitOfWorkDeleteRelation;
    private UnitOfWorkExecutorImpl unitOfWorkExecutor;
    public UnitOfWork()
    {
      Dictionary<String, Type> clazzes = new Dictionary<String, Type>();
      opResultIdStrings = new List<String>();
      OpResultIdGenerator opResultIdGenerator = new OpResultIdGenerator( opResultIdStrings );
      unitOfWorkCreate = new UnitOfWorkCreateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkDelete = new UnitOfWorkDeleteImpl( operations, opResultIdGenerator );
      unitOfWorkUpdate = new UnitOfWorkUpdateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkFind = new UnitOfWorkFindImpl( operations, opResultIdGenerator );
      RelationOperationImpl relationOperation = new RelationOperationImpl( operations, opResultIdGenerator );
      unitOfWorkAddRelation = new UnitOfWorkAddRelationImpl( relationOperation );
      unitOfWorkSetRelation = new UnitOfWorkSetRelationImpl( relationOperation );
      unitOfWorkDeleteRelation = new UnitOfWorkDeleteRelationImpl( relationOperation );
      unitOfWorkExecutor = new UnitOfWorkExecutorImpl( this, clazzes );
    }

    public List<String> GetOpResultIdStrings()
    {
      return opResultIdStrings;
    }

    public IsolationLevelEnum GetTransactionIsolation()
    {
      return transactionIsolation;
    }
    
    public void SetTransactionIsolation(  IsolationLevelEnum transactionIsolation )
    {
      this.transactionIsolation = transactionIsolation;
    }

    public UnitOfWorkResult Execute()
    {
      return unitOfWorkExecutor.Execute();
    }

    public void Execute( AsyncCallback<UnitOfWorkResult> callback )
    {
      unitOfWorkExecutor.Equals( callback );
    }

    public OpResult Create<E>( E instance ) 
    {
      return unitOfWorkCreate.Create( instance );
    }

    public OpResult Create( String tableName, Dictionary<String, Object> objectMap )
    {
      return unitOfWorkCreate.Create( tableName, objectMap );
    }

    public OpResult BulkCreate<E>( List<E> instances )
    {
      return unitOfWorkCreate.BulkCreate( instances );
    }

    public OpResult BulkCreate( String tableName, List<Dictionary<String, Object>> arrayOfObjectMaps )
    {
      return unitOfWorkCreate.BulkCreate( tableName, arrayOfObjectMaps );
    }

    public OpResult Delete<E>( E instance )
    {
      return unitOfWorkDelete.Delete( instance );
    }

    public OpResult Delete( String tableName, Dictionary<String, Object> objectMap )
    {
      return unitOfWorkDelete.Delete( tableName, objectMap );
    }

    public OpResult Delete( String tableName, String objectId )
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

    public OpResult BulkDelete<E>( List<E> instances )
    {
      return unitOfWorkDelete.BulkDelete( instances );
    }

    public OpResult BulkDelete( String tableName, String[] objectIdValues )
    {
      return unitOfWorkDelete.BulkDelete( tableName, objectIdValues );
    }

    public OpResult BulkDelete( String tableName, List<Dictionary<String, Object>> arrayOfObjects )
    {
      return unitOfWorkDelete.BulkDelete( tableName, arrayOfObjects );
    }

    public OpResult BulkDelete( String tableName, String whereClause )
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

    public OpResult Update( String tableName, Dictionary<String, Object> objectMap )
    {
      return unitOfWorkUpdate.Update( tableName, objectMap );
    }

    public OpResult Update( OpResult result, Dictionary<String, Object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResult result, String propertyName, Object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult Update( OpResultValueReference result, Dictionary<String,Object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResultValueReference result, String propertyName, Object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult BulkUpdate( String tableName, String whereClause, Dictionary<String, Object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, whereClause, changes );
    }

    public OpResult BulkUpdate( String tableName, List<String> objectsForChanges, Dictionary<String, Object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, objectsForChanges, changes );
    }

    public OpResult BulkUpdate( OpResult objectIdsForChanges, Dictionary<String, Object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( objectIdsForChanges, changes );
    }

    public OpResult Find( String tableName, DataQueryBuilder queryBuilder )
    {
      return unitOfWorkFind.Find( tableName, queryBuilder );
    }

    public OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstance )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenInstance);
    }

    public OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, children );
    }

    public OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, childrenMaps );
    }

    public OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, children );
    }

    public OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation<E>( E parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation<E>( E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation<E>( E parentObject, String columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children );
    }

    public OpResult AddToRelation<E>( E parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( OpResult parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResult parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResult parentObject, String columnName, OpResult children )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children );
    }

    public OpResult AddToRelation( OpResult parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, String columnName, OpResult children )
    {
        return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, children);
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkAddRelation.AddToRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, children );
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, object> parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObject, columnName, whereClauseForChildren);
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, children );
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentTable, parentObjectId, columnName, whereClauseForChildren);
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances);
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, OpResult children )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkSetRelation.SetRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, object>> childrenMaps )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, children );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( String parentTable, String parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( String parentTable, String parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObjectId, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, OpResult children )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObjectId, columnName, children );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, OpResult children )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, children );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, OpResult children )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, children );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, OpResult children )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, children );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren )
    {
      return unitOfWorkDeleteRelation.DeleteRelation( parentObject, columnName, whereClauseForChildren );
    }
  }
}
