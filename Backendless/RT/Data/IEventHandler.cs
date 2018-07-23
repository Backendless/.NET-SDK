using System;
using BackendlessAPI.RT;

namespace BackendlessAPI.RT.Data
{
  public delegate void ObjectCreated<T>( T obj );
  public delegate void ObjectUpdated<T>( T obj );
  public delegate void ObjectDeleted<T>( T obj );
  public delegate void MultipleObjectsUpdated( BulkEvent bulkEvent );
  public delegate void MultipleObjectsDeleted( BulkEvent bulkEvent );
  public delegate void HandleDataError( RTErrorType errorType, Exception.BackendlessFault backendlessFault );

  public interface IEventHandler<T>
  {
    void SetErrorHandler( HandleDataError errorHandler );

    void AddCreateListener( ObjectCreated<T> objectCreated );

    void AddCreateListener( String whereClause, ObjectCreated<T> callback );

    void RemoveCreateListeners();

    void RemoveCreateListener( ObjectCreated<T> objectCreated );

    void RemoveCreateListener( String whereClause, ObjectCreated<T> callback );

    void RemoveCreateListeners( String whereClause );

    void AddUpdateListener( ObjectUpdated<T> callback );

    void AddUpdateListener( String whereClause, ObjectUpdated<T> callback );

    void RemoveUpdateListeners();

    void RemoveUpdateListener( String whereClause, ObjectUpdated<T> callback );

    void RemoveUpdateListener( ObjectUpdated<T> callback );

    void RemoveUpdateListeners( String whereClause );

    void AddDeleteListener( ObjectDeleted<T> callback );

    void AddDeleteListener( String whereClause, ObjectDeleted<T> callback );

    void RemoveDeleteListeners();

    void RemoveDeleteListener( String whereClause, ObjectDeleted<T> callback );

    void RemoveDeleteListener( ObjectDeleted<T> callback );

    void RemoveDeleteListeners( String whereClause );

    void AddBulkUpdateListener( MultipleObjectsUpdated callback );

    void AddBulkUpdateListener( String whereClause, MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListeners();

    void RemoveBulkUpdateListener( String whereClause, MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListener( MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListeners( String whereClause );

    void AddBulkDeleteListener( MultipleObjectsDeleted callback );

    void AddBulkDeleteListener( String whereClause, MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListeners();

    void RemoveBulkDeleteListener( String whereClause, MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListener( MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListeners( String whereClause );
  }
}
