using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkCreateImpl //: UnitOfWorkCreate
  {
    private List<Operation<Object>> operations;
    private OpResultIdGenerator opResultIdGenerator;
    private Dictionary<String, Object> clazzes;

    UnitOfWorkCreateImpl( List<Operation<Object>> operations, OpResultIdGenerator opResultIdGenerator, Dictionary<String, Object> clazzes )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
      this.clazzes = clazzes;
    }

    /*public OpResult Create( String table, Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );
      
    }
    */
  }
}
