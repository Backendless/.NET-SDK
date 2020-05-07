using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Transaction.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weborb.Service;

namespace BackendlessAPI.Transaction
{
  public class UnitOfWork : IUnitOfWork
  {
    public static String REFERENCE_MARKER = "___ref";
    public static String OP_RESULT_ID = "opResultId";
    public static String RESULT_INDEX = "resultIndex";
    public static String PROP_NAME = "propName";

    internal IsolationLevelEnum transactionIsolation = IsolationLevelEnum.REPEATABLE_READ;
    public readonly LinkedList<Operation> operations = new LinkedList<Operation>();
    private readonly List<String> opResultIdStrings;

    private UnitOfWorkCreate unitOfWorkCreate;
    private UnitOfWorkDelete unitOfWorkDelete;
    private UnitOfWorkUpdate unitOfWorkUpdate;
    private UnitOfWorkFind unitOfWorkFind;
    private UnitOfWorkExecutor unitOfWorkExecutor;

    public UnitOfWork()
    {
      Dictionary<String, Type> clazzes = new Dictionary<String, Type>();
      opResultIdStrings = new List<String>();
      OpResultIdGenerator opResultIdGenerator = new OpResultIdGenerator( opResultIdStrings );
      unitOfWorkCreate = new UnitOfWorkCreateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkDelete = new UnitOfWorkDeleteImpl( operations, opResultIdGenerator );
      unitOfWorkUpdate = new UnitOfWorkUpdateImpl( operations, opResultIdGenerator, clazzes );
      unitOfWorkFind = new UnitOfWorkFindImpl( operations, opResultIdGenerator );
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

    public OpResult Delete<E>( E instance ) where E : class
    {
      return unitOfWorkDelete.Delete( instance );
    }

    public OpResult Delete( string tableName, Dictionary<string, object> objectMap )
    {
      return unitOfWorkDelete.Delete( tableName, objectMap );
    }

    public OpResult Delete( string tableName, string objectId )
    {
      return unitOfWorkDelete.Delete( tableName, objectId );
    }

    public OpResult Delete( OpResult result )
    {
      return unitOfWorkDelete.Delete( result );
    }

    public OpResult Delete( OpResultValueReference resultIndex )
    {
      return unitOfWorkDelete.Delete( resultIndex );
    }

    public OpResult BulkDelete<E>( List<E> instances ) where E : class
    {
      return unitOfWorkDelete.BulkDelete( instances );
    }

    public OpResult BulkDelete( string tableName, string[] objectIdValues )
    {
      return unitOfWorkDelete.BulkDelete( tableName, objectIdValues );
    }

    public OpResult BulkDelete( string tableName, List<Dictionary<string, object>> arrayOfObjects )
    {
      return unitOfWorkDelete.BulkDelete( tableName, arrayOfObjects );
    }

    public OpResult BulkDelete( string tableName, string whereClause )
    {
      return unitOfWorkDelete.BulkDelete( tableName, whereClause );
    }

    public OpResult BulkDelete( OpResult result )
    {
      return unitOfWorkDelete.BulkDelete( result );
    }

    public OpResult Update<E>( E instance )
    {
      return unitOfWorkUpdate.Update( instance );
    }

    public OpResult Update( string tableName, Dictionary<string, object> objectMap )
    {
      return unitOfWorkUpdate.Update( tableName, objectMap );
    }

    public OpResult Update( OpResult result, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResult result, string propertyName, object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult Update( OpResultValueReference result, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.Update( result, changes );
    }

    public OpResult Update( OpResultValueReference result, string propertyName, object propertyValue )
    {
      return unitOfWorkUpdate.Update( result, propertyName, propertyValue );
    }

    public OpResult BulkUpdate( string tableName, string whereClause, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, whereClause, changes );
    }

    public OpResult BulkUpdate( string tableName, List<string> objectsForChanges, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( tableName, objectsForChanges, changes );
    }

    public OpResult BulkUpdate( OpResult objectIdsForChanges, Dictionary<string, object> changes )
    {
      return unitOfWorkUpdate.BulkUpdate( objectIdsForChanges, changes );
    }

    public OpResult Find( String tableName, DataQueryBuilder queryBuilder )
    {
      return unitOfWorkFind.Find( tableName, queryBuilder );
    }
  }
}
