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

    T FindFirst( int relationsDepth );

    T FindFirst( IList<string> relations );

    T FindFirst( IList<string> relations, int relationsDepth );

    void FindFirst( AsyncCallback<T> responder );

    void FindFirst( int relationsDepth, AsyncCallback<T> responder );

    void FindFirst( IList<string> relations, AsyncCallback<T> responder );

    void FindFirst( IList<string> relations, int relationsDepth, AsyncCallback<T> responder );

    T FindLast();

    T FindLast( int relationsDepth );

    T FindLast( IList<string> relations );

    T FindLast( IList<string> relations, int relationsDepth );

    void FindLast( AsyncCallback<T> responder );

    void FindLast( int relationsDepth, AsyncCallback<T> responder );

    void FindLast( IList<string> relations, AsyncCallback<T> responder );

    void FindLast( IList<string> relations, int relationsDepth, AsyncCallback<T> responder );

    BackendlessCollection<T> Find();

    BackendlessCollection<T> Find( BackendlessDataQuery dataQueryOptions );

    void Find( AsyncCallback<BackendlessCollection<T>> responder );

    void Find( BackendlessDataQuery dataQueryOptions, AsyncCallback<BackendlessCollection<T>> responder );

    T FindById( string id );

    T FindById( string id, int relationsDepth );

    T FindById( string id, IList<string> relations );

    T FindById( string id, IList<string> relations, int relationsDepth );

    void FindById( string id, AsyncCallback<T> responder );

    void FindById( string id, int relationsDepth, AsyncCallback<T> responder );

    void FindById( string id, IList<string> relations, AsyncCallback<T> responder );

    void FindById( string id, IList<string> relations, int relationsDepth, AsyncCallback<T> responder );

    void LoadRelations( T entity, IList<string> relations );

    void LoadRelations( T entity, int relationsDepth );

    void LoadRelations( T entity, IList<string> relations, int relationsDepth );

    void LoadRelations( T entity, IList<string> relations, AsyncCallback<T> responder );

    void LoadRelations( T entity, int relationsDepth, AsyncCallback<T> responder );

    void LoadRelations( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> responder );
  }
}