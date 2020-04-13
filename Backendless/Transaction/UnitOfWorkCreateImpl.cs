using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkCreateImpl : UnitOfWorkCreate
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

    public OpResult Create( String tableName, Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      TransactionHelper.MakeReferenceToValueFromOpResult( objectMap );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.CREATE, tableName );
      OperationCreate operationCreate = new OperationCreate( OperationType.CREATE, tableName, operationResultId, objectMap );
      operations.Add( operationCreate );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.CREATE );
    }
    
    public OpResult BulkCreate( String tableName, List<Dictionary<String, Object>> arrayOfObjectMaps )
    {
      if( arrayOfObjectMaps == null )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      foreach( Dictionary<String, Object> mapObject in arrayOfObjectMaps )
        if( mapObject != null )
          TransactionHelper.MakeReferenceToValueFromOpResult( mapObject );
        else
          throw new ArgumentException( ExceptionMessage.NULL_MAP );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.CREATE_BULK, tableName );
      OperationCreateBulk operationCreateBulk = new OperationCreateBulk( OperationType.CREATE_BULK, tableName,
                                                                         operationResultId, arrayOfObjectMaps );

      operations.Add( operationCreateBulk );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.CREATE_BULK );
    }
  }
}
