using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkAddRelation
  {
    //Dictionary + array of objectIds
    OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, String[] childrenObjectIds );

    //Dictionary + array of custom classes
    OpResult AddToRelation<E>( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<E> childrenInstance );

    //Dicionary + List of Dictionaries
    OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Dictionary + OpResult = CREATE_BULK
    OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject, String columnName, OpResult children );

    //Dictionary + whereClause
    OpResult AddToRelation( String parentTable, Dictionary<String, Object> parentObject,
                            String columnName, String whereClauseForChildren );

    //String + array of objectIds
    OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, String[] childrenObjectIds );

    //String + array of custom classes
    OpResult AddToRelation<E>( String parentTable, String parentObjectId, String columnName, List<E> childrenInstances );

    //String + List of hashmaps
    OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //String + OpResult=CREATE_BULK
    OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, OpResult children );

    //String + whereClause
    OpResult AddToRelation( String parentTable, String parentObjectId, String columnName, String whereClauseForChildren );

    //Custom class + array of objectIds
    OpResult AddToRelation<E>( E parentObject, String columnName, String[] childrenObjectIds );

    //Custom class + array of custom classes
    OpResult AddToRelation<E,U>( E parentObject, String columnName, List<U> childrenInstances );

    //Custom class + List of Dictionaries
    OpResult AddToRelation<E>( E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Custom class + OpResult=CREATE_BULK
    OpResult AddToRelation<E>( E parentObject, String columnName, OpResult children );

    //Custom class + whereClause
    OpResult AddToRelation<E>( E parentObject, String columnName, String whereClauseForChildren );

    //OpResult=CREATE/UPDATE(getObjectId) + array of objectIds
    OpResult addToRelation( OpResult parentObject, String columnName, String[] childrenObjectIds );

    //OpResult=CREATE/UPDATE(getObjectId) + array of custom classes
    OpResult AddToRelation<E>( OpResult parentObject, String columnName, List<E> childrenInstances );

    // OpResult=CREATE/UPDATE(getObjectId) + List of Dictionaries
    OpResult AddToRelation( OpResult parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResult=CREATE/UPDATE(getObjectId) + OpResult=CREATE_BULK
    OpResult AddToRelation( OpResult parentObject, String columnName, OpResult children );

    //OpResult=CREATE/UPDATE(getObjectId) + where clause
    OpResult AddToRelation( OpResult parentObject, String columnName, String whereClauseForChildren );

    //OpResult=CREATE_BULK(resultIndex) + array of objectIds
    OpResult AddToRelation( OpResultValueReference parentObject, String columnName, String[] childrenObjectIds );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId)+ array of custom classes
    OpResult AddToRelation<E>( OpResultValueReference parentObject, String columnName, List<E> childrenInstances );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + List of hashmaps
    OpResult AddToRelation( OpResultValueReference parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResult=CREATE_BULK(resultIndex) + OpResult=CREATE_BULK
    OpResult AddToRelation( OpResultValueReference parentObject, String columnName, OpResult children );

    //OpResult=CREATE_BULK(resultIndex) + where clause
    OpResult AddToRelation( OpResultValueReference parentObject, String columnName, String whereClauseForChildren );
  }
}
