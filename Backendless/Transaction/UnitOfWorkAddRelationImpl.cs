using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkAddRelationImpl
  {
    private RelationOperationImpl relationOperation;

    internal UnitOfWorkAddRelationImpl( RelationOperationImpl relationOperation )
    {
      this.relationOperation = relationOperation;
    }
    public OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( string parentTable, Dictionary<string, object> parentObject, string columnName, List<E> childrenInstance )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObject, columnName, childrenInstance );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObject, columnName, children );
    }

    public OpResult AddToRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( string parentTable, string parentObjectId, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObjectId, columnName, childrenMaps );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObjectId, columnName, children );
    }

    public OpResult AddToRelation( string parentTable, string parentObjectId, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E, U>( E parentObject, string columnName, List<U> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, children );
    }

    public OpResult AddToRelation<E>( E parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResult parentObject, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, children );
    }

    public OpResult AddToRelation( OpResult parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult AddToRelation<E>( OpResultValueReference parentObject, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, List<Dictionary<string, object>> childrenMaps )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, childrenMaps );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, children );
    }

    public OpResult AddToRelation( OpResultValueReference parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.ADD_RELATION, parentObject, columnName, whereClauseForChildren );
    }
  }
}
