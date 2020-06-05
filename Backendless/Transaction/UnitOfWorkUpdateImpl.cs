using System;
using System.Collections.Generic;
using BackendlessAPI.Exception;
using BackendlessAPI.Transaction.Operations;
using BackendlessAPI.Transaction.Payload;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkUpdateImpl : UnitOfWorkUpdate
  {
    private LinkedList<Operation> operations;
    private OpResultIdGenerator opResultIdGenerator;
    private Dictionary<String, Type> clazzes;

    internal UnitOfWorkUpdateImpl(LinkedList<Operation> operations, OpResultIdGenerator opResultIdGenerator, Dictionary<String, Type> clazzes )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
      this.clazzes = clazzes;
    }

    public OpResult Update<E>( E instance )
    {
      Dictionary<String, Object> entityMap = TransactionHelper.ConvertInstanceToMap( instance );
      String tableName = instance.GetType().Name;
      clazzes[ "tableName" ] = instance.GetType();

      return Update( tableName, entityMap );
    }
    public OpResult Update( OpResult result, String propertyName, Object propertyValue )
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ propertyName ] = propertyValue;

      return Update( result, changes );
    }

    public OpResult Update( OpResult result, Dictionary<String, Object> changes )
    {
      if( result == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( !OperationTypeUtil.supportEntityDescriptionResultType.Contains( result.GetOperationType() ) )
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      changes[ "objectId" ] = result.ResolveTo( "objectId" ).MakeReference();

      return Update( result.GetTableName(), changes );
    }

    public OpResult Update( OpResultValueReference result, String propertyName, Object propertyValue )
    {
      Dictionary<String, Object> changes = new Dictionary<String, Object>();
      changes[ propertyName ] = propertyValue;

      return Update( result, changes );
    }

    public OpResult Update( OpResultValueReference result, Dictionary<String, Object> changes )
    {
      if( result == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( result.GetResultIndex() == null || result.GetPropName() != null )
        throw new ArgumentException( ExceptionMessage.OP_RESULT_INDEX_YES_PROP_NAME_NOT );

      if( OperationTypeUtil.supportCollectionEntityDescriptionType.Contains( result.GetOpResult().GetOperationType() ) )
        changes[ "objectId" ] = result.ResolveTo( "objectId" ).MakeReference();
      else if( OperationTypeUtil.supportListIdsResultType.Contains( result.GetOpResult().GetOperationType() ) )
        changes[ "objectId" ] = result.MakeReference();
      else
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      return Update( result.GetOpResult().GetTableName(), changes );
    }

    public OpResult Update( String tableName, Dictionary<String, Object> objectMap )
    {
      if( objectMap == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      TransactionHelper.MakeReferenceToValueFromOpResult( objectMap );
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.UPDATE, tableName );
      OperationUpdate operationUpdate = new OperationUpdate( OperationType.UPDATE, tableName, operationResultId, objectMap );
      operations.AddLast( operationUpdate );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.UPDATE );
    }

    public OpResult BulkUpdate( String tableName, String whereClause, Dictionary<String, Object> changes )
    {
      return BulkUpdate( tableName, whereClause, null, changes );
    }

    public OpResult BulkUpdate( String tableName, List<String> objectsForChanges, Dictionary<String, Object> changes )
    {
      if( objectsForChanges == null )
        throw new ArgumentException( ExceptionMessage.NULL_BULK );

      return BulkUpdate( tableName, null, objectsForChanges, changes );
    }

    public OpResult BulkUpdate( OpResult objectIdsForChanges, Dictionary<String, Object> changes )
    {
      if( objectIdsForChanges == null )
        throw new ArgumentException( ExceptionMessage.NULL_OP_RESULT );

      if( !( OperationTypeUtil.supportCollectionEntityDescriptionType.Contains( objectIdsForChanges.GetOperationType() ) ||
                          OperationTypeUtil.supportListIdsResultType.Contains( objectIdsForChanges.GetOperationType() ) ) )
        throw new ArgumentException( ExceptionMessage.REF_TYPE_NOT_SUPPORT );

      return BulkUpdate( objectIdsForChanges.GetTableName(), null, objectIdsForChanges.MakeReference(), changes );
    }

    private OpResult BulkUpdate( String tableName, String whereClause, Object objectsForChanges, Dictionary<String, Object> changes )
    {
      if( changes == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      TransactionHelper.RemoveSystemField( changes );
      TransactionHelper.MakeReferenceToValueFromOpResult( changes );
      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.UPDATE_BULK, tableName );
      UpdateBulkPayload updateBulkPayload = new UpdateBulkPayload( whereClause, objectsForChanges, changes );
      OperationUpdateBulk operationUpdateBulk = new OperationUpdateBulk( OperationType.UPDATE_BULK, tableName,
                                                                       operationResultId, updateBulkPayload );
      operations.AddLast( operationUpdateBulk );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.UPDATE_BULK );
    }
  }
}
