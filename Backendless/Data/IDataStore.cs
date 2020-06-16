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

    IList<string> Create( IList<T> objects );
  #if !(NET_35 || NET_40)
    Task<IList<string>> CreateAsync( IList<T> objects );
  #endif
    void Create( IList<T> objects, AsyncCallback<IList<string>> responder );

  #endregion

  #region BULK UPDATE

    int Update( string whereClause, Dictionary<string, object> changes );
  #if !(NET_35 || NET_40)
    Task<int> UpdateAsync( string whereClause, Dictionary<string, object> changes );
  #endif
    void Update( string whereClause, Dictionary<string, object> changes, AsyncCallback<int> callback );

  #endregion

  #region BULK DELETE

    int Remove( string whereClause );
  #if !(NET_35 || NET_40)
    Task<int> RemoveAsync( string whereClause );
  #endif
    void Remove( string whereClause, AsyncCallback<int> callback );

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

    T FindById( string id );
    T FindById( string id, DataQueryBuilder queryBuilder );
    T FindById( T entity, DataQueryBuilder queryBuilder );
    T FindById( string id, int? relationsDepth );
    T FindById( string id, IList<string> relations );
    T FindById( string id, IList<string> relations, int? relationsDepth );
    T FindById( T entity );
    T FindById( T entity, int? relationsDepth );
    T FindById( T entity, IList<string> relations );
    T FindById( T entity, IList<string> relations, int? relationsDepth );
  #if !(NET_35 || NET_40)
    Task<T> FindByIdAsync( string id );
    Task<T> FindByIdAsync( string id, DataQueryBuilder queryBuilder );
    Task<T> FindByIdAsync( T entity, DataQueryBuilder queryBuilder );
    Task<T> FindByIdAsync( string id, int? relationsDepth );
    Task<T> FindByIdAsync( string id, IList<string> relations );
    Task<T> FindByIdAsync( string id, IList<string> relations, int? relationsDepth );
    Task<T> FindByIdAsync( T entity );
    Task<T> FindByIdAsync( T entity, int? relationsDepth );
    Task<T> FindByIdAsync( T entity, IList<string> relations );
    Task<T> FindByIdAsync( T entity, IList<string> relations, int? relationsDepth );
  #endif
    void FindById( string id, AsyncCallback<T> responder );
    void FindById( string id, DataQueryBuilder queryBuilder, AsyncCallback<T> responder );
    void FindById( T entity, DataQueryBuilder queryBuilder, AsyncCallback<T> responder );
    void FindById( string id, int? relationsDepth, AsyncCallback<T> responder );
    void FindById( string id, IList<string> relations, AsyncCallback<T> responder );
    void FindById( string id, IList<string> relations, int? relationsDepth, AsyncCallback<T> responder );
    void FindById( T entity, AsyncCallback<T> responder );
    void FindById( T entity, int? relationsDepth, AsyncCallback<T> responder );
    void FindById( T entity, IList<string> relations, AsyncCallback<T> responder );
    void FindById( T entity, IList<string> relations, int? relationsDepth, AsyncCallback<T> responder );

  #endregion

  #region LOAD RELATIONS

    IList<M> LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder );
  #if !(NET_35 || NET_40)
    Task<IList<M>> LoadRelationsAsync<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder );
  #endif
    void LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                           AsyncCallback<IList<M>> responder );

  #endregion

  #region GET OBJECT COUNT

    int GetObjectCount();
    int GetObjectCount( DataQueryBuilder dataQueryBuilder );
  #if !(NET_35 || NET_40)
    Task<int> GetObjectCountAsync();
    Task<int> GetObjectCountAsync( DataQueryBuilder dataQueryBuilder );
  #endif
    void GetObjectCount( AsyncCallback<int> responder );
    void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> responder );

  #endregion

  #region ADD RELATION

    void AddRelation( T parent, string columnName, object[] children );
    int AddRelation( T parent, string columnName, string whereClause );
  #if !(NET_35 || NET_40)
    Task AddRelationAsync( T parent, string columnName, object[] children );
    Task<int> AddRelationAsync( T parent, string columnName, string whereClause );
  #endif
    void AddRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );
    void AddRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );

  #endregion

  #region SET RELATION

    int SetRelation( T parent, string columnName, object[] children );
    int SetRelation( T parent, string columnName, string whereClause );
  #if !(NET_35 || NET_40)
    Task<int> SetRelationAsync( T parent, string columnName, object[] children );
    Task<int> SetRelationAsync( T parent, string columnName, string whereClause );
  #endif
    void SetRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );
    void SetRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );

  #endregion

  #region DELETE RELATION

    int DeleteRelation( T parent, string columnName, object[] children );
    int DeleteRelation( T parent, string columnName, string whereClause );
  #if !(NET_35 || NET_40)
    Task<int> DeleteRelationAsync( T parent, string columnName, object[] children );
    Task<int> DeleteRelationAsync( T parent, string columnName, string whereClause );
  #endif
    void DeleteRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );
    void DeleteRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );

  #endregion

  #region RT

  #if WITHRT
    IEventHandler<T> RT();
  #endif

  #endregion
  }
}