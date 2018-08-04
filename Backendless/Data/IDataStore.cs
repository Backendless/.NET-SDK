﻿using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
#if WITHRT
using BackendlessAPI.RT.Data;
#endif

namespace BackendlessAPI.Data
{
  public interface IDataStore<T>
  {
  #region BULL CREATE

    IList<string> Create( IList<T> objects );
    void Create( IList<T> objects, AsyncCallback<IList<string>> responder );

  #endregion

  #region BULK UPDATE

    int Update( string whereClause, Dictionary<string, object> changes );
    void Update( string whereClause, Dictionary<string, object> changes, AsyncCallback<int> callback );

  #endregion

  #region BULK DELETE

    int Remove( string whereClause );
    void Remove( string whereClause, AsyncCallback<int> callback );

  #endregion

  #region SAVE OBJECT

    T Save( T entity );

    void Save( T entity, AsyncCallback<T> responder );

  #endregion

  #region REMOVE OBJECT

    long Remove( T entity );
    void Remove( T entity, AsyncCallback<long> responder );

  #endregion

  #region FIND FIRST

    T FindFirst();

    T FindFirst( DataQueryBuilder queryBuilder );

    void FindFirst( AsyncCallback<T> responder );

    void FindFirst( DataQueryBuilder queryBuilder, AsyncCallback<T> responder );

  #endregion

  #region FIND LAST

    T FindLast();

    T FindLast( DataQueryBuilder queryBuilder );

    void FindLast( AsyncCallback<T> responder );

    void FindLast( DataQueryBuilder queryBuilder, AsyncCallback<T> responder );

  #endregion

  #region FIND

    IList<T> Find();

    IList<T> Find( DataQueryBuilder dataQueryBuilder );

    void Find( AsyncCallback<IList<T>> responder );

    void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<T>> responder );

  #endregion

  #region FIND BY ID

    T FindById( string id );

    T FindById( string id, int relationsDepth );

    T FindById( string id, IList<string> relations );

    T FindById( string id, IList<string> relations, int relationsDepth );

    T FindById( T entity );

    T FindById( T entity, int relationsDepth );

    T FindById( T entity, IList<string> relations );

    T FindById( T entity, IList<string> relations, int relationsDepth );

    void FindById( string id, AsyncCallback<T> responder );

    void FindById( string id, int relationsDepth, AsyncCallback<T> responder );

    void FindById( string id, IList<string> relations, AsyncCallback<T> responder );

    void FindById( string id, IList<string> relations, int relationsDepth, AsyncCallback<T> responder );

    void FindById( T entity, AsyncCallback<T> responder );

    void FindById( T entity, int relationsDepth, AsyncCallback<T> responder );

    void FindById( T entity, IList<string> relations, AsyncCallback<T> responder );

    void FindById( T entity, IList<string> relations, int relationsDepth, AsyncCallback<T> responder );

  #endregion

  #region LOAD RELATIONS

    IList<M> LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder );

    void LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                           AsyncCallback<IList<M>> responder );

  #endregion

  #region GET OBJECT COUNT

    int GetObjectCount();

    int GetObjectCount( DataQueryBuilder dataQueryBuilder );

    void GetObjectCount( AsyncCallback<int> responder );

    void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> responder );

  #endregion

  #region ADD RELATION

    void AddRelation( T parent, string columnName, object[] children );
    void AddRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );

    int AddRelation( T parent, string columnName, string whereClause );
    void AddRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );

  #endregion

  #region SET RELATION

    int SetRelation( T parent, string columnName, object[] children );
    void SetRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );

    int SetRelation( T parent, string columnName, string whereClause );
    void SetRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );

  #endregion

  #region DELETE RELATION

    int DeleteRelation( T parent, string columnName, object[] children );
    void DeleteRelation( T parent, string columnName, object[] children, AsyncCallback<int> callback );

    int DeleteRelation( T parent, string columnName, string whereClause );
    void DeleteRelation( T parent, string columnName, string whereClause, AsyncCallback<int> callback );

  #endregion

  #region RT

  #if WITHRT
    IEventHandler<T> RT();
  #endif

  #endregion
  }
}