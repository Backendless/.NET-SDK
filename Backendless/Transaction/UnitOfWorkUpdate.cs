using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkUpdate
  {
    OpResult Update<E>( E instance );

    OpResult Update( String tableName, Dictionary<String, Object> objectMap );

    //OpResult from CREATE/UPDATE = identification object what will update ( get object id )
    OpResult Update( OpResult result, Dictionary<String, Object> changes );

    //OpResult from CREATE/UPDATE = identification object what will update ( get object id )
    OpResult Update( OpResult result, String propertyName, Object propertyValue );

    //OpResultValueReference from FIND = identification object what will update ( get object id )
    //OpResultValueReference from CREATE_BULK = already an object identifier
    OpResult Update( OpResultValueReference result, Dictionary<String, Object> changes );

    //OpResultValueReference from FIND = identification object what will update ( get object id )
    //OpResultValueReference from CREATE_BULK = already an object identifier
    OpResult Update( OpResultValueReference result, String propertyName, Object propertyValue );

    OpResult BulkUpdate( String tableName, String whereClause, Dictionary<String, Object> changes );

    OpResult BulkUpdate( String tableName, List<String> objectsForChanges, Dictionary<String, Object> changes );

    //OpResult from FIND or CREATE_BULK
    OpResult BulkUpdate( OpResult objectIdsForChanges, Dictionary<String, Object> changes );
  }
}
