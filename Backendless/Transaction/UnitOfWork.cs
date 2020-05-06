using BackendlessAPI.Async;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWork// : IUnitOfWork
  {
    internal static String REFERENCE_MARKER = "___ref";
    internal static String OP_RESULT_ID = "opResultId";
    internal static String RESULT_INDEX = "resultIndex";
    internal static String PROP_NAME = "propName";

    internal IsolationLevelEnum transactionIsolation = IsolationLevelEnum.REPEATABLE_READ;
    internal readonly LinkedList<Operation> operations = new LinkedList<Operation>();
    private readonly List<String> opResultIdStrings;

    private readonly UnitOfWorkExecutor unitOfWorkExecutor;
    private readonly UnitOfWorkCreate unitOfWorkCreate;

    public UnitOfWork()
    {
      Dictionary<String, Type> clazzes = new Dictionary<String, Type>();
      opResultIdStrings = new List<String>();
      OpResultIdGenerator opResultIdGenerator = new OpResultIdGenerator( opResultIdStrings );
      unitOfWorkCreate = new UnitOfWorkCreateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkExecutor = new UnitOfWorkExecutorImpl( this, clazzes );
    }
    
    public List<String> GetOpResultIdStrings()
    {
      return opResultIdStrings;
    }

    public UnitOfWorkResult Execute()
    {
      return unitOfWorkExecutor.Execute();
    }

    public void Execute( AsyncCallback<UnitOfWorkResult> callback )
    {
      unitOfWorkExecutor.Equals( callback );
    }

    public OpResult Create<E>( E instance ) where E : class
    {
      return unitOfWorkCreate.Create( instance );
    }

    public OpResult Create( String tableName, Dictionary<String, Object> objectMap )
    {
      return unitOfWorkCreate.Create( tableName, objectMap );
    }

    public OpResult BulkCreate<E>( List<E> instances ) where E : class
    {
      return unitOfWorkCreate.BulkCreate( instances );
    }

    public OpResult BulkCreate( String tableName, List<Dictionary<String, Object>> arrayOfObjectMaps )
    {
      return unitOfWorkCreate.BulkCreate( tableName, arrayOfObjectMaps );
    }

  }
}
