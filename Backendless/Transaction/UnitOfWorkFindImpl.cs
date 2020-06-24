using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction.Operations;

namespace BackendlessAPI.Transaction
{
  class UnitOfWorkFindImpl
  {
    private LinkedList<Operation> operations;
    private OpResultIdGenerator opResultIdGenerator;

    internal UnitOfWorkFindImpl( LinkedList<Operation> operations, OpResultIdGenerator opResultIdGenerator )
    {
      this.operations = operations;
      this.opResultIdGenerator = opResultIdGenerator;
    }

    public OpResult Find( String tableName, DataQueryBuilder queryBuilder )
    {
      BackendlessDataQuery query = queryBuilder.Build();

      String operationResultId = opResultIdGenerator.GenerateOpResultId( OperationType.FIND, tableName );
      OperationFind operationFind = new OperationFind( OperationType.FIND, tableName, operationResultId, query );

      operations.AddLast( operationFind );

      return TransactionHelper.MakeOpResult( tableName, operationResultId, OperationType.FIND );
    }
  }
}
