using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction.Operations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkDeleteImpl : UnitOfWorkDelete
  {
    private List<Operation> operations;
    OpResultIdGenerator opResultIdGenerator;
    internal UnitOfWorkDeleteImpl( List<Operation> operations, OpResultIdGenerator opResultIdGenerator )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
    }

    public OpResult Delete<E>( E instance ) where E : class
    {
      Dictionary<String, Object> entityMap = TransactionHelper.ConvertInstanceToMap( instance );
      String tableName = instance.GetType().Name;

      return Delete( tableName, entityMap );
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

      operations.Add( operationDelete );
      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.DELETE );
    }

    public OpResult Delete( OpResult result )
    {
      if( result == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( !OperationTypeUtil.supportEntityDescriptionResultType.Contains( result.GetOperationType() ) )
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE, result.GetTableName() );
      OperationDelete operationDelete = new OperationDelete( OperationType.DELETE, result.GetTableName(), operationResultId,
                                                              result.ResolveTo( "objectId" ).MakeReference() );
      operations.Add( operationDelete );

      return TransactionHelper.MakeOpResult( result.GetTableName(), operationResultId, OperationType.DELETE );
    }

    public OpResult Delete( OpResultValueReference resultIndex )
    {
      if( resultIndex == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT_VALUE_REFERENCE );

      if( resultIndex.GetResultIndex() == null || resultIndex.GetPropName() == null )
        throw new ArgumentException( ExceptionMessage.OP_RESULT_INDEX_YES_PROP_NAME_NOT );

      Dictionary<String, Object> referenceToObjectId = TransactionHelper.ConvertCreateBulkOrFindResultIndexToObjectId( resultIndex );

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE, resultIndex.GetOpResult().GetTableName() );
      OperationDelete operationDelete = new OperationDelete( OperationType.DELETE, resultIndex.GetOpResult().GetTableName(),
                                                                                   operationResultId, referenceToObjectId );
      operations.Add( operationDelete );

      return TransactionHelper.MakeOpResult( resultIndex.GetOpResult().GetTableName(), operationResultId, OperationType.DELETE );
    }

    public OpResult BulkDelete<E>( List<E> instances ) where E : class
    {
      List<Dictionary<String, Object>> serializedEntities = new List<Dictionary<string, object>>();

      for( int i = 0; i < serializedEntities.Count; i++ )
        serializedEntities.Add( new Dictionary<String, Object>( TransactionHelper.ConvertInstanceToMap<E>( instances[ i ] ) ) );

      String tableName = instances[0].GetType().Name;
      return BulkDelete( tableName, serializedEntities );
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

      if( !( OperationTypeUtil.supportCollectionEntityDescriptionType.Contains( result.GetOperationType()) ||
                                                          OperationTypeUtil.supportListIdsResultType.Contains( result.GetOperationType())))
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      return BulkDelete( result.GetTableName(), null, result.MakeReference() );
    }

    private OpResult BulkDelete( String tableName, String whereClause, Object unconditional )
    {
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.DELETE_BULK, tableName );
      DeleteBulkPayload deleteBulkPayload = new DeleteBulkPayload( whereClause, unconditional );
      OperationDeleteBulk operationDeleteBulk = new OperationDeleteBulk( OperationType.DELETE_BULK, tableName, operationResultId,
                                                                                            deleteBulkPayload );
      operations.Add( operationDeleteBulk );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.DELETE_BULK );
    }
  }
}
