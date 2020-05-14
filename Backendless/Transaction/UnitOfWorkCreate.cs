using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkCreate
  {
    OpResult Create<E>( E instance ); 
    OpResult Create( String table, Dictionary<String, Object> objectMap );

    OpResult BulkCreate<E>( List<E> instances );

    OpResult BulkCreate( String table, List<Dictionary<String, Object>> ArrayOfObjectMaps );
  }
}
