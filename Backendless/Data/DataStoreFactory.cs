using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Service;
using BackendlessAPI.RT.Data;

namespace BackendlessAPI.Data
{
  internal static class DataStoreFactory
  {
    internal static IDataStore<T> CreateDataStore<T>()
    {
      return new DataStoreImpl<T>();
    }

    private class DataStoreImpl<T> : IDataStore<T>
    {
      private IEventHandler<T> eventHandler = EventHandlerFactory.Of<T>();

      #region Bulk Create
      public IList<string> Create( IList<T> objects )
      {
        return Backendless.Persistence.Create( objects );
      }

      public void Create( IList<T> objects, AsyncCallback<IList<string>> responder )
      {
        Backendless.Persistence.Create( objects, responder );
      }
      #endregion
      #region Bulk Update
      public int Update( string whereClause, Dictionary<string, object> changes )
      {
        return Backendless.Persistence.Update( PersistenceService.GetTypeName( typeof( T ) ), whereClause, changes );
      }
      public void Update( string whereClause, Dictionary<string, object> changes, AsyncCallback<int> callback )
      {
        Backendless.Persistence.Update( PersistenceService.GetTypeName( typeof( T ) ), whereClause, changes, callback );
      }
      #endregion
      #region Bulk Delete
      public int Remove( string whereClause )
      {
        return Backendless.Persistence.Remove( PersistenceService.GetTypeName( typeof( T ) ), whereClause );
      }
      public void Remove( string whereClause, AsyncCallback<int> callback )
      {
        Backendless.Persistence.Remove( PersistenceService.GetTypeName( typeof( T ) ), whereClause, callback );
      }
      #endregion
      #region Save
      public T Save( T entity )
      {
        return Backendless.Persistence.Save( entity );
      }

      public void Save( T entity, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Save( entity, responder );
      }
      #endregion
      #region Remove by Object
      public long Remove( T entity )
      {
        return Backendless.Persistence.Remove( entity );
      }

      public void Remove( T entity, AsyncCallback<long> responder )
      {
        Backendless.Persistence.Remove( entity, responder );
      }
      #endregion
      #region FindFirst
      public T FindFirst()
      {
        return Backendless.Persistence.First<T>();
      }

      public T FindFirst( DataQueryBuilder queryBuilder )
      {
        return Backendless.Persistence.First<T>( queryBuilder );
      }

      public void FindFirst( AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( responder );
      }

      public void FindFirst( DataQueryBuilder queryBuilder, AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( queryBuilder, responder );
      }
      #endregion
      #region FindLast
      public T FindLast()
      {
        return Backendless.Persistence.Last<T>();
      }

      public T FindLast( DataQueryBuilder queryBuilder )
      {
        return Backendless.Persistence.Last<T>( queryBuilder );
      }

      public void FindLast( AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( responder );
      }

      public void FindLast( DataQueryBuilder queryBuilder, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( queryBuilder, responder );
      }
      #endregion
      #region Find
      public IList<T> Find()
      {
        return Backendless.Persistence.Find<T>( (DataQueryBuilder) null );
      }

      public IList<T> Find( DataQueryBuilder dataQueryBuilder )
      {
        return Backendless.Persistence.Find<T>( dataQueryBuilder );
      }

      public void Find( AsyncCallback<IList<T>> responder )
      {
        Backendless.Persistence.Find( (DataQueryBuilder) null, responder );
      }

      public void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<T>> responder )
      {
        Backendless.Persistence.Find( dataQueryBuilder, responder );
      }
      #endregion
      #region FindById with ID

      public T FindById( string id )
      {
        return Backendless.Persistence.FindById<T>( id, null );
      }

      public T FindById( string id, int relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( id, null, relationsDepth );
      }

      public T FindById( string id, IList<string> relations )
      {
        return Backendless.Persistence.FindById<T>( id, relations );
      }

      public T FindById( string id, IList<string> relations, int relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( id, relations, relationsDepth );
      }

      public void FindById( string id, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, null, responder );
      }

      public void FindById( string id, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, null, relationsDepth, responder );
      }

      public void FindById( string id, IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, relations, responder );
      }

      public void FindById( string id, IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, relations, relationsDepth, responder );
      }

      #endregion
      #region FindById with Object

      public T FindById( T entity )
      {
        return Backendless.Persistence.FindById<T>( entity );
      }

      public T FindById( T entity, int relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( entity, relationsDepth );
      }

      public T FindById( T entity, IList<string> relations )
      {
        return Backendless.Persistence.FindById<T>( entity, relations );
      }

      public T FindById( T entity, IList<string> relations, int relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( entity, relations, relationsDepth );
      }

      public void FindById( T entity, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, responder );
      }

      public void FindById( T entity, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relationsDepth, responder );
      }

      public void FindById( T entity, IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relations, responder );
      }

      public void FindById( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relations, relationsDepth, responder );
      }
      #endregion
      #region LoadRelations
      public IList<M> LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder )
      {
        return Backendless.Persistence.LoadRelations( PersistenceService.GetTypeName( typeof( T ) ), objectId, queryBuilder );
      }

      public void LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder, AsyncCallback<IList<M>> responder )
      {
        Backendless.Persistence.LoadRelations( PersistenceService.GetTypeName( typeof( T ) ), objectId, queryBuilder, responder );
      }
      #endregion
      #region Get Object Count
      public int GetObjectCount()
      {
        return Backendless.Persistence.GetObjectCount<T>();
      }

      public int GetObjectCount( DataQueryBuilder dataQueryBuilder )
      {
        return Backendless.Persistence.GetObjectCount<T>( dataQueryBuilder );
      }

      public void GetObjectCount( AsyncCallback<int> responder )
      {
        Backendless.Persistence.GetObjectCount<T>( responder );
      }

      public void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> responder )
      {
        Backendless.Persistence.GetObjectCount<T>( dataQueryBuilder, responder );
      }
      #endregion
      #region ADD RELATION
      public void AddRelation( T parent, string columnName, object[] children )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children );
      }

      public void AddRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children, callback );
      }

      public int AddRelation( T parent, string columnName, string whereClause )
      {
        return Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause );
      }

      public void AddRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause, callback );
      }
      #endregion
      #region SET RELATION
      public int SetRelation( T parent, string columnName, object[] children )
      {
        return Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children );
      }

      public void SetRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback )
      {
        Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children, callback );
      }

      public int SetRelation( T parent, string columnName, string whereClause )
      {
        return Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause );
      }

      public void SetRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback )
      {
        Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause, callback );
      }

      #endregion
      #region DELETE RELATION
      public int DeleteRelation( T parent, string columnName, object[] children )
      {
        return Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children );
      }

      public void DeleteRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback )
      {
        Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, children, callback );
      }

      public int DeleteRelation( T parent, string columnName, string whereClause )
      {
        return Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause );
      }

      public void DeleteRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback )
      {
        Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName, whereClause );
      }
      #endregion
      #region RT
      public IEventHandler<T> RT()
      {
        return this.eventHandler;
      }
      #endregion
    }
  }
}