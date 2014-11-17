using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;

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
      #region Remove
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

      public T FindFirst( int relationsDepth )
      {
        return Backendless.Persistence.First<T>( relationsDepth );
      }

      public T FindFirst( IList<string> relations )
      {
        return Backendless.Persistence.First<T>( relations );
      }

      public T FindFirst( IList<string> relations, int relationsDepth )
      {
        return Backendless.Persistence.First<T>( relations, relationsDepth );
      }

      public void FindFirst( AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( responder );
      }

      public void FindFirst( int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( relationsDepth, responder );
      }

      public void FindFirst( IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( relations, responder );
      }

      public void FindFirst( IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( relations, relationsDepth, responder );
      }
      #endregion
      #region FindLast
      public T FindLast()
      {
        return Backendless.Persistence.Last<T>();
      }

      public T FindLast( int relationsDepth )
      {
        return Backendless.Persistence.Last<T>( relationsDepth );
      }

      public T FindLast( IList<string> relations )
      {
        return Backendless.Persistence.Last<T>( relations );
      }

      public T FindLast( IList<string> relations, int relationsDepth )
      {
        return Backendless.Persistence.Last<T>( relations, relationsDepth );
      }

      public void FindLast( AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( responder );
      }

      public void FindLast( int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( relationsDepth, responder );
      }

      public void FindLast( IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( relations, responder );
      }

      public void FindLast( IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( relations, relationsDepth, responder );
      }
      #endregion
      #region Find
      public BackendlessCollection<T> Find()
      {
        return Backendless.Persistence.Find<T>( null );
      }

      public BackendlessCollection<T> Find( BackendlessDataQuery dataQueryOptions )
      {
        return Backendless.Persistence.Find<T>( dataQueryOptions );
      }

      public void Find( AsyncCallback<BackendlessCollection<T>> responder )
      {
        Backendless.Persistence.Find( null, responder );
      }

      public void Find( BackendlessDataQuery dataQueryOptions, AsyncCallback<BackendlessCollection<T>> responder )
      {
        Backendless.Persistence.Find( dataQueryOptions, responder );
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
      public void LoadRelations( T entity, IList<string> relations )
      {
        Backendless.Persistence.LoadRelations( entity, relations );
      }

      public void LoadRelations( T entity, int relationsDepth )
      {
        Backendless.Persistence.LoadRelations( entity, relationsDepth );
      }

      public void LoadRelations( T entity, IList<string> relations, int relationsDepth )
      {
        Backendless.Persistence.LoadRelations( entity, relations, relationsDepth );
      }

      public void LoadRelations( T entity, IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.LoadRelations( entity, relations, responder );
      }

      public void LoadRelations( T entity, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.LoadRelations( entity, relationsDepth, responder );
      }

      public void LoadRelations( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.LoadRelations( entity, relations, relationsDepth, responder );
      }
      #endregion
    }
  }
}