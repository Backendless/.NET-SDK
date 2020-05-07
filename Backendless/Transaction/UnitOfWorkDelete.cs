﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkDelete
  {
    OpResult Delete<E>( E instance ) where E : class;

    OpResult Delete( String tableName, Dictionary<String, Object> objectMap );

    OpResult Delete( String tableName, String objectId );

    OpResult Delete( OpResult result );

    OpResult Delete( OpResultValueReference resultIndex );

    OpResult BulkDelete<E>( List<E> instances ) where E : class;

    OpResult BulkDelete( String tableName, String[] objectIdValues );

    OpResult BulkDelete( String tableName, List<Dictionary<String, Object>> arrayOfObjects );

    OpResult BulkDelete( String tableName, String whereClause );

    OpResult BulkDelete( OpResult result );
  }
}