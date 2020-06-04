using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkDeleteRelationImpl : UnitOfWorkDeleteRelation
  {
    private RelationOperation relationOperation;

    internal UnitOfWorkDeleteRelationImpl(RelationOperation relationOperation)
    {
      this.relationOperation = relationOperation;
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstance )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObject, columnName, childrenInstance );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObject, columnName, children );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObjectId, columnName, children);
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, children);
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, children );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult DeleteRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, children );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentTable, parentObjectId, columnName, childrenMaps );
    }

    public OpResult DeleteRelation<E>( E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( OpResult parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenMaps );
    }

    public OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.DELETE_RELATION, parentObject, columnName, childrenMaps );
    }
  }
}
