using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkSetRelationImpl
  {
    private RelationOperationImpl relationOperation;

    internal UnitOfWorkSetRelationImpl( RelationOperationImpl relationOperation )
    {
      this.relationOperation = relationOperation;
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstance )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, childrenInstance );
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, children );
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, children );
    }

    public OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children);
    }

    public OpResult SetRelation<E>( E parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children);
    }

    public OpResult SetRelation( OpResult parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }
  }
}
