using System;
using System.Collections.Generic;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
#if WITHRT
using BackendlessAPI.RT.Data;

#endif

namespace BackendlessAPI.Data
{
  public interface IDataStore<T>
  {
  #region BULK CREATE

    IList<String> Create( IList<T> objects );
  #if !(NET_35 || NET_40)
    Task<IList<String>> CreateAsync( IList<T> objects );
  #endif
    void Create( IList<T> objects, AsyncCallback<IList<String>> responder );

  #endregion

  #region BULK UPDATE

    Int32 Update( String whereClause, Dictionary<String, Object> changes );
  #if !(NET_35 || NET_40)
    Task<Int32> UpdateAsync( String whereClause, Dictionary<String, Object> changes );
  #endif
    void Update( String whereClause, Dictionary<String, Object> changes, AsyncCallback<Int32> callback );

  #endregion

  #region BULK DELETE

    Int32 Remove( String whereClause );
  #if !(NET_35 || NET_40)
    Task<Int32> RemoveAsync( String whereClause );
  #endif
    void Remove( String whereClause, AsyncCallback<Int32> callback );

  #endregion

  #region SAVE OBJECT

    T Save( T entity );
  #if !(NET_35 || NET_40)
    Task<T> SaveAsync( T entity );
  #endif
    void Save( T entity, AsyncCallback<T> responder );

  #endregion

  #region REMOVE OBJECT

    long Remove( T entity );
  #if !(NET_35 || NET_40)
    Task<long> RemoveAsync( T entity );
  #endif
    void Remove( T entity, AsyncCallback<long> responder );

  #endregion

  #region FIND FIRST

    T FindFirst();
    T FindFirst( DataQueryBuilder queryBuilder );
  #if !(NET_35 || NET_40)
    Task<T> FindFirstAsync();
    Task<T> FindFirstAsync( DataQueryBuilder queryBuilder );
  #endif
    void FindFirst( AsyncCallback<T> responder );
    void FindFirst( DataQueryBuilder queryBuilder, AsyncCallback<T> responder );

  #endregion

  #region FIND LAST

    T FindLast();
    T FindLast( DataQueryBuilder queryBuilder );
  #if !(NET_35 || NET_40)
    Task<T> FindLastAsync();
    Task<T> FindLastAsync( DataQueryBuilder queryBuilder );
  #endif
    void FindLast( AsyncCallback<T> responder );
    void FindLast( DataQueryBuilder queryBuilder, AsyncCallback<T> responder );

  #endregion

  #region FIND

    IList<T> Find();
    IList<T> Find( DataQueryBuilder dataQueryBuilder );
  #if !(NET_35 || NET_40)
    Task<IList<T>> FindAsync();
    Task<IList<T>> FindAsync( DataQueryBuilder dataQueryBuilder );
  #endif
    void Find( AsyncCallback<IList<T>> responder );
    void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<T>> responder );

  #endregion

  #region FIND BY ID

    T FindById( String id );
    T FindById( String id, DataQueryBuilder queryBuilder );
    T FindById( T entity, DataQueryBuilder queryBuilder );
    T FindById( String id, Int32? relationsDepth );
    T FindById( String id, IList<String> relations );
    T FindById( String id, IList<String> relations, Int32? relationsDepth );
    T FindById( T entity );
    T FindById( T entity, Int32? relationsDepth );
    T FindById( T entity, IList<String> relations );
    T FindById( T entity, IList<String> relations, Int32? relationsDepth );
  #if !(NET_35 || NET_40)
    Task<T> FindByIdAsync( String id );
    Task<T> FindByIdAsync( String id, DataQueryBuilder queryBuilder );
    Task<T> FindByIdAsync( T entity, DataQueryBuilder queryBuilder );
    Task<T> FindByIdAsync( String id, Int32? relationsDepth );
    Task<T> FindByIdAsync( String id, IList<String> relations );
    Task<T> FindByIdAsync( String id, IList<String> relations, Int32? relationsDepth );
    Task<T> FindByIdAsync( T entity );
    Task<T> FindByIdAsync( T entity, Int32? relationsDepth );
    Task<T> FindByIdAsync( T entity, IList<String> relations );
    Task<T> FindByIdAsync( T entity, IList<String> relations, Int32? relationsDepth );
  #endif
    void FindById( String id, AsyncCallback<T> responder );
    void FindById( String id, DataQueryBuilder queryBuilder, AsyncCallback<T> responder );
    void FindById( T entity, DataQueryBuilder queryBuilder, AsyncCallback<T> responder );
    void FindById( String id, Int32? relationsDepth, AsyncCallback<T> responder );
    void FindById( String id, IList<String> relations, AsyncCallback<T> responder );
    void FindById( String id, IList<String> relations, Int32? relationsDepth, AsyncCallback<T> responder );
    void FindById( T entity, AsyncCallback<T> responder );
    void FindById( T entity, Int32? relationsDepth, AsyncCallback<T> responder );
    void FindById( T entity, IList<String> relations, AsyncCallback<T> responder );
    void FindById( T entity, IList<String> relations, Int32? relationsDepth, AsyncCallback<T> responder );

  #endregion

  #region LOAD RELATIONS

    IList<M> LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder );
  #if !(NET_35 || NET_40)
    Task<IList<M>> LoadRelationsAsync<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder );
  #endif
    void LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                           AsyncCallback<IList<M>> responder );

  #endregion

  #region GET OBJECT COUNT

    Int32 GetObjectCount();
    Int32 GetObjectCount( DataQueryBuilder dataQueryBuilder );
  #if !(NET_35 || NET_40)
    Task<Int32> GetObjectCountAsync();
    Task<Int32> GetObjectCountAsync( DataQueryBuilder dataQueryBuilder );
  #endif
    void GetObjectCount( AsyncCallback<Int32> responder );
    void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<Int32> responder );

  #endregion

  #region ADD RELATION

    void AddRelation( T parent, String columnName, Object[] children );
    Int32 AddRelation( T parent, String columnName, String whereClause );
  #if !(NET_35 || NET_40)
    Task AddRelationAsync( T parent, String columnName, Object[] children );
    Task<Int32> AddRelationAsync( T parent, String columnName, String whereClause );
  #endif
    void AddRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback );
    void AddRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback );

  #endregion

  #region SET RELATION

    Int32 SetRelation( T parent, String columnName, Object[] children );
    Int32 SetRelation( T parent, String columnName, String whereClause );
  #if !(NET_35 || NET_40)
    Task<Int32> SetRelationAsync( T parent, String columnName, Object[] children );
    Task<Int32> SetRelationAsync( T parent, String columnName, String whereClause );
  #endif
    void SetRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback );
    void SetRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback );

  #endregion

  #region DELETE RELATION

    Int32 DeleteRelation( T parent, String columnName, Object[] children );
    Int32 DeleteRelation( T parent, String columnName, String whereClause );
  #if !(NET_35 || NET_40)
    Task<Int32> DeleteRelationAsync( T parent, String columnName, Object[] children );
    Task<Int32> DeleteRelationAsync( T parent, String columnName, String whereClause );
  #endif
    void DeleteRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback );
    void DeleteRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback );

  #endregion

  #region RT

  #if WITHRT
    IEventHandler<T> RT();
  #endif

  #endregion
  }
}