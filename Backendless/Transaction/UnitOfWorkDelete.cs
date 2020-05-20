using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkDelete
  {
    OpResult Delete<E>( E instance );

    OpResult Delete( String tableName, Dictionary<String, Object> objectMap );

    OpResult Delete( String tableName, String objectId );

    OpResult Delete( OpResult result );

    OpResult Delete( OpResultValueReference resultIndex );

    OpResult BulkDelete<E>( List<E> instances );

    OpResult BulkDelete( String tableName, String[] objectIdValues );

    OpResult BulkDelete( String tableName, List<Dictionary<String, Object>> arrayOfObjects );

    OpResult BulkDelete( String tableName, String whereClause );

    OpResult BulkDelete( OpResult result );
  }
}
