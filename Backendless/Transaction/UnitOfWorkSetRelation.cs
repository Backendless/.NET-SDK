using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkSetRelation
  {
    //Dictionary + array of objectIds
    OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds );

    //Dictionary + List<Dictionary>
    //Dictionary + List of custom classes
    OpResult SetRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstance );

    //Dictionary + OpResult = CREATE_BULK
    OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children );

    //Dictionary + whereClause
    OpResult SetRelation( String parentTable, Dictionary<String, Object> parentObject,
                            String columnName, String whereClauseForChildren );

    //String + array of objectIds
    OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds );

    //String + List of Dictionary
    //String + List of custom classes
    OpResult SetRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances );

    //String + OpResult = CREATE_BULK
    OpResult SetRelation( String parentTable, String parentObjectId, String columnName, OpResult children );

    //String + whereClause
    OpResult SetRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren );

    //Custom class + array of objectIds
    OpResult SetRelation<E>( E parentObject, String columnName, String[] childrenObjectIds );

    //Custom class + List<Dictionary>
    // Custom class + List of custom classes
    OpResult SetRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances );

    //Custom class + OpResult = CREATE_BULK
    OpResult SetRelation<E>( E parentObject, String columnName, OpResult children );

    //Custom class + whereClause
    OpResult SetRelation<E>( E parentObject, String columnName, String whereClauseForChildren );

    //OpResult = CREATE/UPDATE( getObjectId ) + array of objectIds
    OpResult SetRelation( OpResult parentObject, String columnName, String[] childrenObjectIds );

    //OpResult = CREATE/UPDATE( getObjectId ) + List<Dictionary>
    //OpResult = CREATE/UPDATE( getObjectId ) + List of custom classes
    OpResult SetRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances );

    //OpResult = CREATE/UPDATE( getObjectId ) + OpResult = CREATE_BULK
    OpResult SetRelation( OpResult parentObject, String columnName, OpResult children );

    //OpResult = CREATE/UPDATE( getObjectId ) + whereClause
    OpResult SetRelation( OpResult parentObject, String columnName, String whereClauseForChildren );

    //OpResult = CREATE_BULK( resultIndex ) + array of objectIds
    OpResult SetRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds );

    //OpResult = CREATE_BULK( resultIndex) + List<Dictionary>
    //OpResult = CREATE_BULK( resultIndex ) + List of custom classes
    OpResult SetRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances );

    //OpResult = CREATE_BULK( resultIndex ) + OpResult = CREATE_BULK
    OpResult SetRelation( OpResultValueReference parentObject, String columnName, OpResult children );

    //OpResult = CREATE_BULK( resultIndex ) + whereClause
    OpResult SetRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren );
  }
}
