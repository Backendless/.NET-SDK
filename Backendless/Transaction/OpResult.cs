using BackendlessAPI.Exception;
using System;
using System.Collections.Generic;
using BackendlessAPI.Transaction.Operations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI.Transaction
{
  public class OpResult
  {
    private String tableName;
    private String opResultId;
    private OperationType operationType;

    internal OpResult( String tableName, String opResultId, OperationType operationType )
    {
      this.tableName = tableName;
      this.opResultId = opResultId;
      this.operationType = operationType;
    }

    public String GetTableName()
    {
      return tableName;
    }

    public String GetOpResultId()
    {
      return opResultId;
    }

    public OperationType GetOperationType()
    {
      return operationType;
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
      referenceMap[ UnitOfWork.OP_RESULT_ID ] = opResultId;
      return referenceMap;
    }

    public void SetOpResultId( UnitOfWork unitOfWork, String newOpResultId )
    {
      if( unitOfWork.GetOpResultIdStrings().Contains( newOpResultId ) )
        throw new ArgumentException( ExceptionMessage.OP_RESULT_ID_ALREADY_PRESENT );

      foreach( Operation operation in unitOfWork.Operations )
        if( operation.OpResultId.Equals( opResultId ) )
        {
          operation.OpResultId =  newOpResultId;
          break;
        }

      opResultId = newOpResultId;
    }
  }
}
