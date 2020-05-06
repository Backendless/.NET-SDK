using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkCreateImpl : UnitOfWorkCreate
  {
    private LinkedList<Operation> operations;   
    private OpResultIdGenerator opResultIdGenerator;
    private Dictionary<String, Type> clazzes;

    internal UnitOfWorkCreateImpl( LinkedList<Operation> operations, OpResultIdGenerator opResultIdGenerator, Dictionary<String, Type> clazzes )
    {
      this.operations =  operations;
      this.opResultIdGenerator = opResultIdGenerator;
      this.clazzes = clazzes;
    }

    public OpResult Create<E>( E instance ) where E : class
    {
      Dictionary<String, Object> entityMap = TransactionHelper.ConvertInstanceToMap<E>( instance );
      String tableName = instance.GetType().Name;
      clazzes[ "tableName" ] = instance.GetType();
      return Create( tableName, entityMap );
    }

    public OpResult Create( String tableName, Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      TransactionHelper.MakeReferenceToValueFromOpResult( objectMap );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.CREATE, tableName );
      OperationCreate operationCreate = new OperationCreate( OperationType.CREATE, tableName, operationResultId, objectMap );
      operations.AddLast( operationCreate );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.CREATE );
    }

    public OpResult BulkCreate<E>( List<E> instances ) where E : class
    {
      List<Dictionary<String, Object>> serializedEntities = new List<Dictionary<string, object>>();
      int iterator = 0;

      while( instances.Count != iterator )
      {
        serializedEntities.Add( TransactionHelper.ConvertInstanceToMap<E>( instances[ iterator ] ) );
        iterator++;
      }

      String tableName = instances[ 0 ].GetType().Name;
      return BulkCreate( tableName, serializedEntities );
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

      int iterator = arrayOfObjectMaps.Count - 1;

      foreach( Dictionary<String, Object> objectMap in arrayOfObjectMaps )
      {
        iterator--;
        TransactionHelper.MakeReferenceToValueFromOpResult( objectMap );
        String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.CREATE, tableName );
        OperationCreate operationCreateBulk = new OperationCreate( OperationType.CREATE, tableName, operationResultId, objectMap );
        operations.AddLast( operationCreateBulk );
        if( iterator == 0)
          return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.CREATE );
      }

      throw new System.Exception( "Error" );
    }
  }
}
