using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface RelationOperation
  {
    //Dictionary + array of objectIds
    OpResult AddOperation( OperationType operationType, String parentTable, Dictionary<String, Object> parentObject,
                               String columnName, String[] childrenObjectIds );

    //Dictionary + array of custom classes
    OpResult AddOperation<E>( OperationType operationType, String parentTable,
                             Dictionary<String, Object> parentObject, String columnName, String[] childrenInstances ) where E : class;

    //Dictionary + List of hashmaps
    OpResult AddOperation( OperationType operationType, String parentTable,
                           Dictionary<String, Object> parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Dictionary + OpResult=CREATE_BULK or FIND
    OpResult AddOperation( OperationType operationType, String parentTable, Dictionary<String, Object> parentObject,
                           String columnName, OpResult children );

    //Dictionary + whereClause
    OpResult AddOperation( OperationType operationType, String parentTable, Dictionary<String, Object> parentObject,
                           String columnName, String whereClauseForChildren );

    //String + array of objectIds
    OpResult AddOperation( OperationType operationType, String parentTable, String parentObjectId,
                           String columnName, String[] childrenObjectIds );

    //String + array of custom classes
    OpResult AddOperation<E>( OperationType operationType, String parentTable, String parentObjectId,
                             String columnName, String[] childrenInstances );

    //String + List of hashmaps
    OpResult AddOperation( OperationType operationType, String parentTable, String parentObjectId,
                           String columnName, List<Dictionary<String, Object>> childrenMaps );

    //String + OpResult=CREATE_BULK or FIND
    OpResult AddOperation( OperationType operationType, String parentTable, String parentObjectId,
                           String columnName, OpResult children );

    //String + whereClause
    OpResult AddOperation( OperationType operationType, String parentTable, String parentObjectId,
                           String columnName, String whereClauseForChildren );

    //Custom class + array of objectIds
    OpResult AddOperation<E>( OperationType operationType, E parentObject, String columnName, String[] childrenObjectIds );

    //Custom class + array of custom classes
    OpResult AddOperation<E, U>( OperationType operationType, E parentObject, String columnName, String[] childrenInstances );

    //Custom class + List of hashmaps
    OpResult AddOperation<E>( OperationType operationType, E parentObject, String columnName, List<Dictionary<String, Object>> childrenMaps );

    //Custom class + OpResult=CREATE_BULK or FIND
    OpResult AddOperation<E>( OperationType operationType, E parentObject, String columnName, OpResult children );

    //Custom class + whereClause
    OpResult AddOperation<E>( OperationType operationType, E parentObject, String columnName,
                             String whereClauseForChildren );

    //OpResult=CREATE/UPDATE(getObjectId) + array of objectIds
    OpResult AddOperation( OperationType operationType, OpResult parentObject, String columnName, String[] childrenObjectIds );

    //OpResult=CREATE/UPDATE(getObjectId) + array of custom classes
    OpResult AddOperation<E>( OperationType operationType, OpResult parentObject, String columnName, String[] childrenInstances );

    //OpResult=CREATE/UPDATE(getObjectId) + List of hashmaps
    OpResult AddOperation( OperationType operationType, OpResult parentObject,
                           String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResult=CREATE/UPDATE(getObjectId) + OpResult=CREATE_BULK or FIND
    OpResult AddOperation( OperationType operationType, OpResult parentObject,
                           String columnName, OpResult children );

    //OpResult=CREATE/UPDATE(getObjectId) + where clause
    OpResult AddOperation( OperationType operationType, OpResult parentObject,
                           String columnName, String whereClauseForChildren );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + array of objectIds
    OpResult AddOperation( OperationType operationType, OpResultValueReference parentObject,
                           String columnName, String[] childrenObjectIds );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId)+ array of custom classes
    OpResult AddOperation<E>( OperationType operationType, OpResultValueReference parentObject,
                             String columnName, String[] childrenInstances );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + List of hashmaps
    OpResult AddOperation( OperationType operationType, OpResultValueReference parentObject,
                               String columnName, List<Dictionary<String, Object>> childrenMaps );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + OpResult=CREATE_BULK or FIND
    OpResult AddOperation( OperationType operationType, OpResultValueReference parentObject,
                           String columnName, OpResult children );

    //OpResultValueReference=CREATE_BULK/FIND(getObjectId) + where clause
    OpResult AddOperation( OperationType operationType, OpResultValueReference parentObject,
                           String columnName, String whereClauseForChildren );
  }
}
