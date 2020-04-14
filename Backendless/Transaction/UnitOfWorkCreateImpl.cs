﻿using BackendlessAPI.Exception;
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
    private List<Operation<Dictionary<String, Object>>> operations;   
    private OpResultIdGenerator opResultIdGenerator;
    private Dictionary<String, Object> clazzes;

    UnitOfWorkCreateImpl( List<Operation<Dictionary<String, Object>>> operations, OpResultIdGenerator opResultIdGenerator, Dictionary<String, Object> clazzes )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
      this.clazzes = clazzes;
    }

    public OpResult Create<E>( E instance ) where E : class
    {
      Dictionary<String, Object> entityMap = TransactionHelper.ConvertInstanceToMaps<E>( instance );
      String tableName = instance.GetType().Name;
      clazzes[ "tableName" ] = instance;
      return Create( tableName, entityMap );
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

    public OpResult BulkCreate<E>( List<E> instances ) where E : class
    {
      List<Dictionary<String, Object>> serializedEntities = new List<Dictionary<string, object>>();
      int iterator = 0;

      while( instances.Count - 1 != iterator )
      {
        serializedEntities.Add( TransactionHelper.ConvertInstanceToMaps<E>( instances[ iterator ] ) );
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
        operations.Add( operationCreateBulk );
        if( iterator == 0)
          return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.CREATE );
      }

      throw new System.Exception( "Error" );
    }
  }
}
