using System;
using System.Collections.Generic;
using BackendlessAPI.Service;
using Weborb.Service;

namespace BackendlessAPI.RT.Data
{
  public class EventHandlerImpl<T> : RTListenerImpl, IEventHandler<T>
  {
    private readonly Type type;
    private readonly String tableName;
    private HandleDataError errorHandler;

    public EventHandlerImpl( Type type )
    {
      this.type = type;
      this.tableName = PersistenceService.GetTypeName( type );
    }

    public EventHandlerImpl( String tableName )
    {
      this.type = typeof( Dictionary<String, Object> );
      this.tableName = tableName;
    }

    public void SetErrorHandler( HandleDataError errorHandler )
    {
      this.errorHandler = errorHandler;
    }

    #region CREATE LISTENER
    public void AddCreateListener( ObjectCreated<T> listener )
    {
      AddCreateListener( null, listener );
    }

    public void AddCreateListener( String whereClause, ObjectCreated<T> listener )
    {
      DataSubscription subscription = new DataSubscription( RTDataEvents.created, tableName, CreateTypedCallback<T>( listener, type ) )
              .WithWhere( whereClause );

      AddEventListener( subscription );
    }

    public void RemoveCreateListeners()
    {
      RemoveListeners( RTDataEvents.created );
    }

    public void RemoveCreateListener( String whereClause, ObjectCreated<T> listener )
    {
      RemoveListeners( RTDataEvents.created, whereClause, listener );
    }

    public void RemoveCreateListener( ObjectCreated<T> listener )
    {
      RemoveListeners( RTDataEvents.created, listener );
    }

    public void RemoveCreateListeners( String whereClause )
    {
      RemoveListeners( RTDataEvents.created, whereClause );
    }
    #endregion
    #region UPDATE LISTENER
    public void AddUpdateListener( ObjectUpdated<T> listener )
    {
      AddUpdateListener( null, listener );
    }

    public void AddUpdateListener( String whereClause, ObjectUpdated<T> listener )
    {
      DataSubscription subscription = new DataSubscription( RTDataEvents.updated, tableName, CreateTypedCallback<T>( listener, type ) ).WithWhere( whereClause );
      AddEventListener( subscription );
    }

    public void RemoveUpdateListeners()
    {
      RemoveListeners( RTDataEvents.updated );
    }

    public void RemoveUpdateListener( String whereClause, ObjectUpdated<T> listener )
    {
      RemoveListeners( RTDataEvents.updated, whereClause, listener );
    }

    public void RemoveUpdateListener( ObjectUpdated<T> listener )
    {
      RemoveListeners( RTDataEvents.updated, listener );
    }

    public void RemoveUpdateListeners( String whereClause )
    {
      RemoveListeners( RTDataEvents.updated, whereClause );
    }
    #endregion
    #region DELETE LISTENER
    public void AddDeleteListener( ObjectDeleted<T> listener )
    {
      AddDeleteListener( null, listener );
    }

    public void AddDeleteListener( String whereClause, ObjectDeleted<T> listener )
    {
      DataSubscription subscription = new DataSubscription( RTDataEvents.deleted, tableName, CreateTypedCallback<T>( listener, type ) )
              .WithWhere( whereClause );

      AddEventListener( subscription );
    }

    public void RemoveDeleteListeners()
    {
      RemoveListeners( RTDataEvents.deleted );
    }

    public void RemoveDeleteListener( String whereClause, ObjectDeleted<T> listener )
    {
      RemoveListeners( RTDataEvents.deleted, whereClause, listener );
    }

    public void RemoveDeleteListener( ObjectDeleted<T> listener )
    {
      RemoveListeners( RTDataEvents.deleted, listener );
    }

    public void RemoveDeleteListeners( String whereClause )
    {
      RemoveListeners( RTDataEvents.deleted, whereClause );
    }
    #endregion
    #region BULK UPDATE LISTENER
    public void AddBulkUpdateListener( MultipleObjectsUpdated listener )
    {
      AddBulkUpdateListener( null, listener );
    }

    public void AddBulkUpdateListener( String whereClause, MultipleObjectsUpdated listener )
    {
      DataSubscription subscription = new DataSubscription( RTDataEvents.bulk_updated, tableName, CreateTypedCallback<BulkEvent>( listener, typeof( BulkEvent ) ) )
            .WithWhere( whereClause );

      AddEventListener( subscription );
    }

    public void RemoveBulkUpdateListeners()
    {
      RemoveListeners( RTDataEvents.bulk_updated );
    }

    public void RemoveBulkUpdateListener( String whereClause, MultipleObjectsUpdated listener )
    {
      RemoveListeners( RTDataEvents.bulk_updated, whereClause, listener );
    }

    public void RemoveBulkUpdateListener( MultipleObjectsUpdated listener )
    {
      RemoveListeners( RTDataEvents.bulk_updated, listener );
    }

    public void RemoveBulkUpdateListeners( String whereClause )
    {
      RemoveListeners( RTDataEvents.bulk_updated, whereClause );
    }
    #endregion
    #region BULK DELETE LISTENER
    public void AddBulkDeleteListener( MultipleObjectsDeleted listener )
    {
      AddBulkDeleteListener( null, listener );
    }

    public void AddBulkDeleteListener( String whereClause, MultipleObjectsDeleted listener )
    {
      DataSubscription subscription = new DataSubscription( RTDataEvents.bulk_deleted, tableName, CreateTypedCallback<BulkEvent>( listener, typeof( BulkEvent ) ) )
              .WithWhere( whereClause );

      AddEventListener( subscription );
    }

    public void RemoveBulkDeleteListeners()
    {
      RemoveListeners( RTDataEvents.bulk_deleted );
    }

    public void RemoveBulkDeleteListener( String whereClause, MultipleObjectsDeleted listener )
    {
      RemoveListeners( RTDataEvents.bulk_deleted, whereClause, listener );
    }

    public void RemoveBulkDeleteListener( MultipleObjectsDeleted listener )
    {
      RemoveListeners( RTDataEvents.bulk_deleted, listener );
    }

    public void RemoveBulkDeleteListeners( String whereClause )
    {
      RemoveListeners( RTDataEvents.bulk_deleted, whereClause );
    }
    #endregion
    #region BULK CREATE LISTENER
    public void AddBulkCreateListener( MultipleObjectsCreated listener )
    {
      DataSubscription subscription =
        new DataSubscription( RTDataEvents.bulk_created, tableName, CreateTypedCallback<IList<String>>( listener, typeof( IList<String> ) ) );

      AddEventListener( subscription );
    }

    public void RemoveBulkCreateListener( MultipleObjectsCreated listener )
    {
      RemoveListeners( RTDataEvents.bulk_created, listener );
    }

    public void RemoveBulkCreateListeners()
    {
      RemoveListeners( RTDataEvents.bulk_created );
    }
    #endregion

    #region CREATE CALLBACK
    private IRTCallback CreateTypedCallback<U>( Delegate callback, Type type )
    {
      return new RTCallback<U>( callback,
      response =>
      {
        try
        {
          U adaptedResponse = (U) response.adapt( type );
          callback.DynamicInvoke( adaptedResponse );
        }
        catch( System.Exception ex )
        {
          HandleFault( callback, new Exception.BackendlessFault( ex ) );
        }
      },
      fault =>
      {
        HandleFault( callback, fault );
      } );
    }

    private void HandleFault( Delegate callback, Exception.BackendlessFault backendlessFault )
    {
      if( errorHandler == null )
        return;

      if( callback is ObjectCreated<T> )
        errorHandler( RTErrorType.OBJECTCREATED, backendlessFault );
      else if( callback is ObjectUpdated<T> )
        errorHandler( RTErrorType.OBJECTUPDATED, backendlessFault );
      else if( callback is ObjectDeleted<T> )
        errorHandler( RTErrorType.OBJECTDELETED, backendlessFault );
      else if( callback is MultipleObjectsCreated )
        errorHandler( RTErrorType.BULKCREATE, backendlessFault );
      else if( callback is MultipleObjectsUpdated )
        errorHandler( RTErrorType.BULKUPDATE, backendlessFault );
      else if( callback is MultipleObjectsDeleted )
        errorHandler( RTErrorType.BULKDELETE, backendlessFault );
    }
    #endregion

    #region REMOVE LISTENERS (PRIVATE)
    private void RemoveListeners( RTDataEvents dataEvent )
    {
      RemoveEventListener( ( subscriptionToCheck ) => { return IsEventSubscription( subscriptionToCheck, dataEvent ); } );
    }

    private void RemoveListeners( RTDataEvents dataEvent, String whereClause )
    {
      CheckWhereClause( whereClause );
      RemoveEventListener( ( subscriptionToCheck ) =>
      {
        return IsEventSubscription( subscriptionToCheck, dataEvent ) &&
          whereClause.Equals( ( (DataSubscription) subscriptionToCheck ).WhereClause );
      } );
    }

    private void RemoveListeners( RTDataEvents dataEvent, Object listener )
    {
      CheckCallback( listener );
      RemoveEventListener( ( subscriptionToCheck ) =>
      {
        return IsEventSubscription( subscriptionToCheck, dataEvent ) &&
          subscriptionToCheck.Callback.UsersCallback.Equals( listener );
      } );
    }

    private void RemoveListeners( RTDataEvents dataEvent, String whereClause, Object listener )
    {
      CheckCallback( listener );
      CheckWhereClause( whereClause );
      RemoveEventListener( ( subscriptionToCheck ) =>
      {
        return IsEventSubscription( subscriptionToCheck, dataEvent ) &&
          subscriptionToCheck.Callback.UsersCallback.Equals( listener ) &&
                             whereClause.Equals( ( (DataSubscription) subscriptionToCheck ).WhereClause );
      } );
    }
    #endregion

    private Boolean IsEventSubscription( RTSubscription subscription, RTDataEvents dataEvent )
    {
      if( !( subscription is DataSubscription ) )
        return false;

      DataSubscription dataSubscription = (DataSubscription) subscription;
      return dataSubscription.SubscriptionName == SubscriptionNames.OBJECTS_CHANGES && dataSubscription.Event == dataEvent;
    }


    private void CheckCallback( Object callback )
    {
      if( callback == null )
        throw new ArgumentNullException( "Callback can not be null" );
    }

    private void CheckWhereClause( String whereClause )
    {
      if( whereClause == null )
        throw new ArgumentNullException( "whereClause can not be null" );
    }
  }
}