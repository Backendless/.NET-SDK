using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction.Operations;
using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkDeleteImpl
  {
    private LinkedList<Operation> operations;
    OpResultIdGenerator opResultIdGenerator;
    internal UnitOfWorkDeleteImpl( LinkedList<Operation> operations, OpResultIdGenerator opResultIdGenerator )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
    }

    public OpResult Delete<E>( E instance )
    {
      String tableName = instance.GetType().Name;
      String objectId = (String) TransactionHelper.ConvertInstanceToObjectIdOrLeaveReference<E>( instance );
      return Delete( tableName, objectId );
    }

    public OpResult Delete( String tableName, Dictionary<String, Object> objectMap )
    {
      String objectId = TransactionHelper.ConvertObjectMapToObjectId( objectMap );
      return Delete( tableName, objectId );
    }

    public OpResult Delete( String tableName, String objectId )
    {
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE, tableName );
      OperationDelete operationDelete = new OperationDelete( OperationType.DELETE, tableName, operationResultId, objectId );
      operations.AddLast( operationDelete );
      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.DELETE );
    }

    public OpResult Delete( OpResult result )
    {
      if( result == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( !OperationTypeUtil.supportEntityDescriptionResultType.Contains( result.OperationType ) )
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE, result.TableName );
      OperationDelete operationDelete = new OperationDelete( OperationType.DELETE, result.TableName, operationResultId,
                                                                           result.ResolveTo( "objectId" ).MakeReference() );
      operations.AddLast( operationDelete );
      return TransactionHelper.MakeOpResult( result.TableName, operationResultId, OperationType.DELETE );
    }

    public OpResult Delete( OpResultValueReference resultIndex )
    {
      if( resultIndex == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT_VALUE_REFERENCE );

      if( resultIndex.ResultIndex == null || resultIndex.PropName != null )
        throw new ArgumentException( ExceptionMessage.OP_RESULT_INDEX_YES_PROP_NAME_NOT );

      Dictionary<String, Object> referenceToObjectId = TransactionHelper.ConvertCreateBulkOrFindResultIndexToObjectId( resultIndex );
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE, resultIndex.OpResult.TableName );
      OperationDelete operationDelete = new OperationDelete( OperationType.DELETE, resultIndex.OpResult.TableName,
                                                                                   operationResultId, referenceToObjectId );
      operations.AddLast( operationDelete );
      return TransactionHelper.MakeOpResult( resultIndex.OpResult.TableName, operationResultId, OperationType.DELETE );
    }

    public OpResult BulkDelete<E>( List<E> instances )
    {
      if( instances == null )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      String tableName = instances[ 0 ].GetType().Name;
      List<Object> objectIds = new List<Object>();

      foreach( E inst in instances )
        objectIds.Add( TransactionHelper.ConvertInstanceToObjectIdOrLeaveReference<E>( inst ) );

      return BulkDelete( tableName, null, objectIds );
    }

    public OpResult BulkDelete( String tableName, List<Dictionary<String, Object>> arrayOfObjects )
    {
      if( arrayOfObjects == null || arrayOfObjects.Count == 0 )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      List<Object> objectIds = TransactionHelper.ConvertMapsToObjectIds( arrayOfObjects );
      return BulkDelete( tableName, null, objectIds );
    }

    public OpResult BulkDelete( String tableName, String[] objectIdValues )
    {
      if( objectIdValues == null || objectIdValues.Length == 0 )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      return BulkDelete( tableName, null, objectIdValues );
    }

    public OpResult BulkDelete( String tableName, String whereClause )
    {
      if( whereClause == null )
        throw new ArgumentException( ExceptionMessage.NULL_WHERE );

      return BulkDelete( tableName, whereClause, null );
    }

    public OpResult BulkDelete( OpResult result )
    {
      if( result == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( !( OperationTypeUtil.supportCollectionEntityDescriptionType.Contains( result.OperationType ) ||
                                                          OperationTypeUtil.supportListIdsResultType.Contains( result.OperationType ) ) )
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      return BulkDelete( result.TableName, null, result.MakeReference() );
    }

    private OpResult BulkDelete( String tableName, String whereClause, Object unconditional )
    {
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE_BULK, tableName );
      DeleteBulkPayload deleteBulkPayload = new DeleteBulkPayload( whereClause, unconditional );
      OperationDeleteBulk operationDeleteBulk = new OperationDeleteBulk( OperationType.DELETE_BULK, tableName, operationResultId,
                                                                                                             deleteBulkPayload );
      operations.AddLast( operationDeleteBulk );
      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.DELETE_BULK );
    }
  }
}
