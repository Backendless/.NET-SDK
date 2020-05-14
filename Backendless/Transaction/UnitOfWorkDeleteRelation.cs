using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkDeleteRelation
  {
    //Dictionary + array of objectId
    OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds );

    //Dictionary + List<Dictionary>
    //Dictionary + custom classes
    OpResult DeleteRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstances );

    //Dictionary + List of hashmaps
    OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Dictionary + OpResult = CREATE_BULK
    OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children );

    //Dictionary + whereClause
    OpResult DeleteRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String whereClauseForChildren );

    //String + array of objects
    OpResult DeleteRelation( String parentTable, String parentObject, String columnName, String[] childrenObjectIds );

    //String + List<Dictionary>
    //String + custom classes
    OpResult DeleteRelation<E>( String parentTable, String parentObject, String columnName, List<E> childrenInstances );

    //String + List of Dicionaries
    OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //String + OpResult = CREATE_BULK
    OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, OpResult children );

    //String + whereClause
    OpResult DeleteRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren );

    //Custom class + array of objectIds
    OpResult DeleteRelation<E>( E parentObject, String columnName, String[] childrenObjectIds );

    //Custom class + List of hashmaps
    //Custom class + List of custom classes
    OpResult DeleteRelation<E, U>( E parentObject, String columnName, List<U> childrenInstances );

    //Custom class + List of hashmaps
    OpResult DeleteRelation<E>( E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Custom class + OpResult = CREATE_BULK
    OpResult DeleteRelation<E>( E parentObject, String columnName, OpResult children );

    //Custom class + whereClause
    OpResult DeleteRelation<E>( E parentObject, String columnName, String whereClauseForChildren );

    //OpResult=CREATE/UPDATE( getObjectId ) + array of objectIds
    OpResult DeleteRelation( OpResult parentObject, String columnName, String[] childrenObjectIds );

    //OpResult=CREATE/UPDATE( getObjectId ) + List<Dictionary>
    //OpResult=CREATE/UPDATE( getObjectId ) + List of custom classes
    OpResult DeleteRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances );

    //OpResult=CREATE/UPDATE(getObjectId) + List of hashmaps
    OpResult DeleteRelation( OpResult parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResult = CREATE/UPDATE( getObjectId ) + OpResult = CREATE_BULK
    OpResult DeleteRelation( OpResult parentObject, String columnName, OpResult children );

    //OpResult = CREATE/UPDATE( getObjectId ) + whereClause
    OpResult DeleteRelation( OpResult parentObject, String columnName, String whereClauseForChildren );

    //OpResult = CREATE_BULK( resultIndex ) + array of objectIds
    OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds );

    //OpResult = CREATE_BULK( resultIndex ) + List<Dictionary>
    //OpResult = CREATE_BULK( resultIndex ) + List of custom classes
    OpResult DeleteRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + List of hashmaps
    OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResult = CREATE_BULK( resultIndex ) + OpResult = CREATE_BULK
    OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, OpResult children );

    //OpResult = CREATE_BULK( resultIndex ) + whereClause
    OpResult DeleteRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren );
  }
}
