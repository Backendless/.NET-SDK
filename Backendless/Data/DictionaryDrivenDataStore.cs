using System;
using System.Collections.Generic;
using BackendlessAPI.Exception;
using BackendlessAPI.Engine;
using BackendlessAPI.Service;
using BackendlessAPI.Persistence;
using BackendlessAPI.Async;

namespace BackendlessAPI.Data
{
  class DictionaryDrivenDataStore : IDataStore<Dictionary<String, Object>>
  {
    private const string PERSISTENCE_MANAGER_SERVER_ALIAS = "com.backendless.services.persistence.PersistenceService";
    private String tableName;

    public DictionaryDrivenDataStore( String tableName )
    {
      this.tableName = tableName;
    }

    #region Save
    public Dictionary<string, object> Save( Dictionary<string, object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { tableName, entity };
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args );
    }

    public void Save( Dictionary<string, object> entity, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { tableName, entity };
      Invoker.InvokeAsync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args, responder );
    }
    #endregion
    #region Remove
    public long Remove( Dictionary<string, object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { tableName, entity };
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args );
    }

    public void Remove( Dictionary<string, object> entity, AsyncCallback<long> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { tableName, entity };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args, responder );
    }

    public long Remove( string objectId )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      Object[] args = new Object[] { tableName, objectId };
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args );
    }

    public void Remove( string objectId, AsyncCallback<long> responder )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      Object[] args = new Object[] { tableName, objectId };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args, responder );
    }
    #endregion
    #region First
    public Dictionary<string, object> FindFirst()
    {
      return FindFirst( DataQueryBuilder.Create() );
    }

    public Dictionary<string, object> FindFirst( DataQueryBuilder queryBuilder )
    {
      List<String> relations = queryBuilder.GetRelated();

      if( relations == null )
        relations = new List<String>();

      int relationsDepth = queryBuilder.GetRelationsDepth();
      Object[] args = new Object[] { tableName, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", args );
    }

    public void FindFirst( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindFirst( DataQueryBuilder.Create(), responder );
    }

    public void FindFirst( DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<string, object>> responder )
    {
      List<String> relations = queryBuilder.GetRelated();

      if( relations == null )
        relations = new List<String>();

      int relationsDepth = queryBuilder.GetRelationsDepth();
      Object[] args = new Object[] { tableName, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", args, responder );
    }
    #endregion
    #region Last
    public Dictionary<string, object> FindLast()
    {
      return FindLast( DataQueryBuilder.Create() );
    }

    public Dictionary<string, object> FindLast( DataQueryBuilder queryBuilder )
    {
      List<String> relations = queryBuilder.GetRelated();

      if( relations == null )
        relations = new List<String>();

      int relationsDepth = queryBuilder.GetRelationsDepth();
      Object[] args = new Object[] { tableName, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", args );
    }

    public void FindLast( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindLast( DataQueryBuilder.Create(), responder );
    }

    public void FindLast( DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<string, object>> responder )
    {
      List<String> relations = queryBuilder.GetRelated();

      if( relations == null )
        relations = new List<String>();

      int relationsDepth = queryBuilder.GetRelationsDepth();
      Object[] args = new Object[] { tableName, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", args, responder );
    }
    #endregion 
    #region Find
    public IList<Dictionary<string, object>> Find()
    {
      return Find( (DataQueryBuilder) null );
    }
    
    public IList<Dictionary<string, object>> Find( DataQueryBuilder dataQueryBuilder )
    {
      BackendlessDataQuery dataQuery = null;

      if( dataQueryBuilder != null )
      {
        dataQuery = dataQueryBuilder.Build();
        PersistenceService.CheckPageSizeAndOffset( dataQuery );
      }

      object[] args = new object[] { tableName, dataQueryBuilder };
      var result = Invoker.InvokeSync<IList<Dictionary<string, object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args );
      return (IList<Dictionary<string, object>>) result;
    }

    public void Find( AsyncCallback<IList<Dictionary<string, object>>> responder )
    {
      Find( (DataQueryBuilder) null, responder );
    }
    
    public void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<Dictionary<string, object>>> callback )
    {
      var responder = new AsyncCallback<IList<Dictionary<string, object>>>(
      r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      },
      f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );

      BackendlessDataQuery dataQuery = null;

      if( dataQueryBuilder != null )
        dataQuery = dataQueryBuilder.Build();

      object[] args = new object[] { tableName, dataQuery };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args, responder );
    }
    #endregion
    #region Find By Id
    public Dictionary<string, object> FindById( string id )
    {
      return FindById( id, 0 );
    }

    public Dictionary<string, object> FindById( string id, int relationsDepth )
    {
      return FindById( id, null, relationsDepth );
    }

    public Dictionary<string, object> FindById( string id, IList<string> relations )
    {
      return FindById( id, relations, 0 );
    }

    public Dictionary<string, object> FindById( string id, IList<string> relations, int relationsDepth )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      if( relations == null )
        relations = new List<String>();

      object[] args = new object[] { tableName, id, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", args );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity )
    {
      return FindById( entity, null, 0 );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, int relationsDepth )
    {
      return FindById( entity, null, relationsDepth );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, IList<string> relations )
    {
      return FindById( entity, relations, 0 );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, IList<string> relations, int relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      if( relations == null )
        relations = new List<String>();

      object[] args = new Object[] { tableName, entity, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", args );
    }

    public void FindById( string id, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, null, 0, responder );
    }

    public void FindById( string id, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, null, relationsDepth, responder );
    }

    public void FindById( string id, IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, relations, 0, responder );
    }

    public void FindById( string id, IList<string> relations, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      if( relations == null )
        relations = new List<String>();

      object[] args = new Object[] { tableName, id, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", args, responder );
    }

    public void FindById( Dictionary<string, object> entity, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, null, 0, responder );
    }

    public void FindById( Dictionary<string, object> entity, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, null, relationsDepth, responder );
    }

    public void FindById( Dictionary<string, object> entity, IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, relations, 0, responder );
    }

    public void FindById( Dictionary<string, object> entity, IList<string> relations, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      if( relations == null )
        relations = new List<String>();

      object[] args = new Object[] { tableName, entity, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", args, responder );
    }
    #endregion
    #region Load Relations
    public IList<M> LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder )
    {
      return Backendless.Persistence.LoadRelations<M>( tableName, objectId, queryBuilder );
    }

    public void LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder, AsyncCallback<IList<M>> responder ) 
    {
      Backendless.Persistence.LoadRelations<M>( tableName, objectId, queryBuilder, responder );
    }
    #endregion
    #region Get Object Count
    public int GetObjectCount()
    {
      return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName }, true );
    }

    public int GetObjectCount( DataQueryBuilder dataQueryBuilder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName, dataQuery }, true );
    }

    public void GetObjectCount( AsyncCallback<int> responder )
    {
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName }, true, responder );
    }

    public void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> responder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName, dataQuery }, true, responder );
    }
    #endregion
    #region ADD RELATION
    public void AddRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }

    public void AddRelation( Dictionary<string, object> parent, string columnName, object[] children, AsyncCallback<object> callback )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }

    public int AddRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }

    public void AddRelation( Dictionary<string, object> parent, string columnName, string whereClause, AsyncCallback<int> callback )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause, callback );
    }

    #endregion
    #region SET RELATION
    public void SetRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }

    public void SetRelation( Dictionary<string, object> parent, string columnName, object[] children, AsyncCallback<object> callback )
    {
      Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }

    public int SetRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }

    public void SetRelation( Dictionary<string, object> parent, string columnName, string whereClause, AsyncCallback<int> callback )
    {
      Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause, callback );
    }

    #endregion
    #region DELETE RELATION
    public void DeleteRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }

    public void DeleteRelation( Dictionary<string, object> parent, string columnName, object[] children, AsyncCallback<object> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }

    public int DeleteRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }

    public void DeleteRelation( Dictionary<string, object> parent, string columnName, string whereClause, AsyncCallback<int> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause, callback );
    }
    #endregion
  }
}