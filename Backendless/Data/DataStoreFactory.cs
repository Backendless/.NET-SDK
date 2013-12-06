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
      public T Save( T entity )
      {
        return Backendless.Persistence.Save( entity );
      }

      public void Save( T entity, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Save( entity, responder );
      }

      public long Remove( T entity )
      {
        return Backendless.Persistence.Remove( entity );
      }

      public void Remove( T entity, AsyncCallback<long> responder )
      {
        Backendless.Persistence.Remove( entity, responder );
      }

      public T FindFirst()
      {
        return Backendless.Persistence.First<T>();
      }

      public void FindFirst( AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( responder );
      }

      public T FindLast()
      {
        return Backendless.Persistence.Last<T>();
      }

      public void FindLast( AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( responder );
      }

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

      public T FindById( string id )
      {
        return Backendless.Persistence.FindById<T>( id, null );
      }

      public void FindById( string id, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, null, responder );
      }

      public T FindById( string id, IList<string> relations )
      {
        return Backendless.Persistence.FindById<T>( id, relations );
      }

      public void FindById( string id, IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, relations, responder );
      }

      public void LoadRelations( T entity, IList<string> relations )
      {
        Backendless.Persistence.LoadRelations( entity, relations );
      }

      public void LoadRelations( T entity, IList<string> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.LoadRelations( entity, relations, responder );
      }
    }
  }
}