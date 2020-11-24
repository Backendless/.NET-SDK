using System;
using BackendlessAPI.RT;

namespace BackendlessAPI.RT.Data
{
  public delegate void ObjectCreated<T>( T obj );
  public delegate void ObjectUpdated<T>( T obj );
  public delegate void ObjectDeleted<T>( T obj );
  public delegate void MultipleObjectsUpdated( BulkEvent bulkEvent );
  public delegate void MultipleObjectsDeleted( BulkEvent bulkEvent );
  public delegate void MultipleObjectsCreated( BulkEvent bulkEvent );
  public delegate void HandleDataError( RTErrorType errorType, Exception.BackendlessFault backendlessFault );

  public interface IEventHandler<T>
  {
    void SetErrorHandler( HandleDataError errorHandler );

    void AddCreateListener( ObjectCreated<T> objectCreated );

    void AddCreateListener( string whereClause, ObjectCreated<T> callback );

    void RemoveCreateListeners();

    void RemoveCreateListener( ObjectCreated<T> objectCreated );

    void RemoveCreateListener( string whereClause, ObjectCreated<T> callback );

    void RemoveCreateListeners( string whereClause );

    void AddUpdateListener( ObjectUpdated<T> callback );

    void AddUpdateListener( string whereClause, ObjectUpdated<T> callback );

    void RemoveUpdateListeners();

    void RemoveUpdateListener( string whereClause, ObjectUpdated<T> callback );

    void RemoveUpdateListener( ObjectUpdated<T> callback );

    void RemoveUpdateListeners( string whereClause );

    void AddDeleteListener( ObjectDeleted<T> callback );

    void AddDeleteListener( string whereClause, ObjectDeleted<T> callback );

    void RemoveDeleteListeners();

    void RemoveDeleteListener( string whereClause, ObjectDeleted<T> callback );

    void RemoveDeleteListener( ObjectDeleted<T> callback );

    void RemoveDeleteListeners( string whereClause );

    void AddBulkCreateListener( MultipleObjectsCreated callback );

    void RemoveBulkCreateListener( MultipleObjectsCreated callback );

    void RemoveBulkCreateListeners();

    void AddBulkUpdateListener( MultipleObjectsUpdated callback );

    void AddBulkUpdateListener( string whereClause, MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListeners();

    void RemoveBulkUpdateListener( string whereClause, MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListener( MultipleObjectsUpdated callback );

    void RemoveBulkUpdateListeners( string whereClause );

    void AddBulkDeleteListener( MultipleObjectsDeleted callback );

    void AddBulkDeleteListener( string whereClause, MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListeners();

    void RemoveBulkDeleteListener( string whereClause, MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListener( MultipleObjectsDeleted callback );

    void RemoveBulkDeleteListeners( string whereClause );
  }
}
