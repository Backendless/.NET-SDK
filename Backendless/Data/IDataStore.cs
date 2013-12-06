using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;

namespace BackendlessAPI.Data
{
  public interface IDataStore<T>
  {
    T Save( T entity );

    void Save( T entity, AsyncCallback<T> responder );

    long Remove( T entity );

    void Remove( T entity, AsyncCallback<long> responder );

    T FindFirst();

    void FindFirst( AsyncCallback<T> responder );

    T FindLast();

    void FindLast( AsyncCallback<T> responder );

    BackendlessCollection<T> Find();

    BackendlessCollection<T> Find( BackendlessDataQuery dataQueryOptions );

    void Find( AsyncCallback<BackendlessCollection<T>> responder );

    void Find( BackendlessDataQuery dataQueryOptions, AsyncCallback<BackendlessCollection<T>> responder );

    T FindById( string id );

    void FindById( string id, AsyncCallback<T> responder );

    T FindById( string id, IList<string> relations );

    void FindById( string id, IList<string> relations, AsyncCallback<T> responder );

    void LoadRelations( T entity, IList<string> relations );

    void LoadRelations( T entity, IList<string> relations, AsyncCallback<T> responder );
  }
}