using BackendlessAPI.Exception;
using BackendlessAPI.Transaction;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;

namespace BackendlessAPI
{
  class UnitOfWorkUpsertImpl
  {
    private LinkedList<Operation> operations;
    private OpResultIdGenerator opResultIdGenerator;
    private Dictionary<String, Type> clazzes;

    internal UnitOfWorkUpsertImpl( LinkedList<Operation> operations, OpResultIdGenerator opResultIdGenerator,
                          Dictionary<String, Type> clazzes )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
      this.clazzes = clazzes;
    }

    public OpResult Upsert<E>( E instance )
    {
      Dictionary<String, Object> entityMap = TransactionHelper.ConvertInstanceToMap( instance );
      String tableName = instance.GetType().Name;

      clazzes[ "tableName" ] = instance.GetType();

      return Upsert( tableName, entityMap );
    }

    public OpResult Upsert( String tableName, Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      TransactionHelper.MakeReferenceToValueFromOpResult( objectMap );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.UPSERT, tableName );
      OperationUpsert operationUpsert = new OperationUpsert( OperationType.UPSERT, tableName, operationResultId, objectMap );

      operations.AddLast( operationUpsert );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.UPSERT );
    }

    public OpResult BulkUpsert<E>( List<E> instances )
    {
      if( instances == null )
        throw new ArgumentException( ExceptionMessage.NULL_INSTANCE );

      String tableName = instances[ 0 ].GetType().Name;
      List<Dictionary<String, Object>> serializedEntities = new List<Dictionary<String, Object>>();
      int iterator = 0;

      while( instances.Count != iterator )
      {
        serializedEntities.Add( TransactionHelper.ConvertInstanceToMap<E>( instances[ iterator ] ) );
        iterator++;
      }

      return BulkUpsert( tableName, serializedEntities );
    }

    public OpResult BulkUpsert( String tableName, List<Dictionary<String, Object>> arrayOfObjectMaps )
    {
      if( arrayOfObjectMaps == null || arrayOfObjectMaps.Count == 0 )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      foreach( Dictionary<String, Object> mapObject in arrayOfObjectMaps )
        if( mapObject != null )
          TransactionHelper.MakeReferenceToValueFromOpResult( mapObject );
        else
          throw new ArgumentException( ExceptionMessage.NULL_MAP );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.UPSERT_BULK, tableName );
      OperationUpsertBulk operationUpsertBulk = new OperationUpsertBulk( OperationType.UPSERT_BULK, tableName,
                                                                         operationResultId, arrayOfObjectMaps );

      operations.AddLast( operationUpsertBulk );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.UPSERT_BULK );
    }
  }
}
