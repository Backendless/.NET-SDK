using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkSetRelationImpl : UnitOfWorkSetRelation
  {
    private RelationOperation relationOperation;

    internal UnitOfWorkSetRelationImpl( RelationOperation relationOperation )
    {
      this.relationOperation = relationOperation;
    }

    public OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( string parentTable, Dictionary<string, object> parentObject, string columnName, List<E> childrenInstance )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, childrenInstance );
    }

    public OpResult SetRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, children );
    }

    public OpResult SetRelation( string parentTable, Dictionary<string, object> parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( string parentTable, string parentObjectId, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, childrenInstances );
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, children );
    }

    public OpResult SetRelation( string parentTable, string parentObjectId, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentTable, parentObjectId, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E, U>( E parentObject, string columnName, List<U> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children);
    }

    public OpResult SetRelation<E>( E parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResult parentObject, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children);
    }

    public OpResult SetRelation( OpResult parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, string[] childrenObjectIds )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenObjectIds );
    }

    public OpResult SetRelation<E>( OpResultValueReference parentObject, string columnName, List<E> childrenInstances )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, childrenInstances );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, OpResult children )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, children );
    }

    public OpResult SetRelation( OpResultValueReference parentObject, string columnName, string whereClauseForChildren )
    {
      return relationOperation.AddOperation( OperationType.SET_RELATION, parentObject, columnName, whereClauseForChildren );
    }
  }
}
