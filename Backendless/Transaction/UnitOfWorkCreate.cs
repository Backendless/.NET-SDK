using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  interface UnitOfWorkCreate
  {
    OpResult Create( String table, Dictionary<String, Object> objectMap );
    OpResult BulkCreate( String table, List<Dictionary<String, Object>> ArrayOfObjectMaps );
  }
}
