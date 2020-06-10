using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction.Operations;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class OpResult
  {
    internal OpResult( String tableName, String opResultId, OperationType operationType )
    {
      TableName = tableName;
      OpResultId = opResultId;
      OperationType = operationType;
    }

    [SetClientClassMemberName( "tableName" )]
    public String TableName
    {
      get;
      private set;
    }

    [SetClientClassMemberName( "opResultId" )]
    public String OpResultId
    {
      get;
      private set;
    }

    [SetClientClassMemberName( "operationType" )]
    public OperationType OperationType
    {
      get;
      private set;
    }

    public OpResultValueReference ResolveTo( Int32 resultIndex, String propName )
    {
      return new OpResultValueReference( this, resultIndex, propName );
    }

    public OpResultValueReference ResolveTo( int resultIndex )
    {
      return new OpResultValueReference( this, resultIndex );
    }

    public OpResultValueReference ResolveTo( String propName )
    {
      return new OpResultValueReference( this, propName );
    }

    internal Dictionary<String, Object> MakeReference()
    {
      Dictionary<String, Object> referenceMap = new Dictionary<String, Object>();
      referenceMap[ UnitOfWork.REFERENCE_MARKER ] = true;
      referenceMap[ UnitOfWork.OP_RESULT_ID ] = OpResultId;
      return referenceMap;
    }

    public void SetOpResultId( UnitOfWork unitOfWork, String newOpResultId )
    {
      if( unitOfWork.GetOpResultIdStrings().Contains( newOpResultId ) )
        throw new ArgumentException( ExceptionMessage.OP_RESULT_ID_ALREADY_PRESENT );

      foreach( Operation operation in unitOfWork.operations )
        if( operation.OpResultId.Equals( OpResultId ) )
        {
          operation.OpResultId =  newOpResultId;
          break;
        }

      OpResultId = newOpResultId;
    }
  }
}
