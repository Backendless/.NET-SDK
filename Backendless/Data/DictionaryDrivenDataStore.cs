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

    public Dictionary<string, object> Save( Dictionary<string, object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity };
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args );
    }

    public void Save( Dictionary<string, object> entity, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity };
      Invoker.InvokeAsync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args, responder );
    }

    public long Remove( Dictionary<string, object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity };
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args );
    }

    public void Remove( Dictionary<string, object> entity, AsyncCallback<long> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args, responder );
    }

    public long Remove( string objectId )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, objectId };
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args );
    }

    public void Remove( string objectId, AsyncCallback<long> responder )
    {
      if( string.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, objectId };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args, responder );
    }

    public Dictionary<string, object> FindFirst()
    {
      return FindFirst( null, 0 );
    }

    public Dictionary<string, object> FindFirst( int relationsDepth )
    {
      return FindFirst( null, relationsDepth );
    }

    public Dictionary<string, object> FindFirst( IList<string> relations )
    {
      return FindFirst( relations, 0 );
    }

    public Dictionary<string, object> FindFirst( IList<string> relations, int relationsDepth )
    {
      if( relations == null )
        relations = new List<String>();

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", args );
    }

    public void FindFirst( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindFirst( null, 0, responder );
    }

    public void FindFirst( int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindFirst( null, relationsDepth, responder );
    }

    public void FindFirst( IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindFirst( relations, 0, responder );
    }

    public void FindFirst( IList<string> relations, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( relations == null )
        relations = new List<String>();

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", args, responder );
    }

    public Dictionary<string, object> FindLast()
    {
      return FindLast( null, 0 );
    }

    public Dictionary<string, object> FindLast( int relationsDepth )
    {
      return FindLast( null, relationsDepth );
    }

    public Dictionary<string, object> FindLast( IList<string> relations )
    {
      return FindLast( relations, 0 );
    }

    public Dictionary<string, object> FindLast( IList<string> relations, int relationsDepth )
    {
      if( relations == null )
        relations = new List<String>();

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, relations, relationsDepth };
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", args );
    }

    public void FindLast( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindLast( null, 0, responder );
    }

    public void FindLast( int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindLast( null, relationsDepth, responder );
    }

    public void FindLast( IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindLast( relations, 0, responder );
    }

    public void FindLast( IList<string> relations, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      if( relations == null )
        relations = new List<String>();

      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", args, responder );
    }

    public BackendlessCollection<Dictionary<string, object>> Find()
    {
      return Find( (BackendlessDataQuery) null );
    }

    public BackendlessCollection<Dictionary<string, object>> Find( BackendlessDataQuery dataQuery )
    {
      PersistenceService.CheckPageSizeAndOffset( dataQuery );
      object[] args = new object[] { Backendless.AppId, Backendless.VersionNum, tableName, dataQuery };
      var result = Invoker.InvokeSync<BackendlessCollection<Dictionary<string, object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args );
      result.Query = dataQuery;
      result.TableName = tableName;
      return result;
    }

    public void Find( AsyncCallback<BackendlessCollection<Dictionary<string, object>>> responder )
    {
      Find( null, responder );
    }

    public void Find( BackendlessDataQuery dataQuery, AsyncCallback<BackendlessCollection<Dictionary<string, object>>> callback )
    {
      var responder = new AsyncCallback<BackendlessCollection<Dictionary<string, object>>>(
      r =>
      {
        r.Query = dataQuery;
        r.TableName = tableName;

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

      object[] args = new object[] { Backendless.AppId, Backendless.VersionNum, tableName, dataQuery };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args, responder );
    }

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

      object[] args = new object[] { Backendless.AppId, Backendless.VersionNum, tableName, id, relations, relationsDepth };
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

      object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity, relations, relationsDepth };
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

      object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, id, relations, relationsDepth };
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

      object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity, relations, relationsDepth };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", args, responder );
    }

    public void LoadRelations( Dictionary<string, object> entity, IList<string> relations )
    {
      LoadRelations( entity, relations, 0 );
    }

    public void LoadRelations( Dictionary<string, object> entity, int relationsDepth )
    {
      LoadRelations( entity, null, relationsDepth );
    }

    public void LoadRelations( Dictionary<string, object> entity, IList<string> relations, int relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      if( relations == null )
        relations = new List<String>();

      Object[] methodArgs = null;

      if( relationsDepth == 0 )
        methodArgs = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity, relations };
      else
        methodArgs = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity, relations, relationsDepth };

      Dictionary<string, object> loadedRelations = Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations", methodArgs, true );

      foreach( string key in loadedRelations.Keys )
      {
        entity.Remove( key );
        entity.Add( key, loadedRelations[ key ] );
      }
    }

    public void LoadRelations( Dictionary<string, object> entity, IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      LoadRelations( entity, relations, 0, responder );
    }

    public void LoadRelations( Dictionary<string, object> entity, int relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      LoadRelations( entity, null, relationsDepth, responder );
    }

    public void LoadRelations( Dictionary<string, object> entity, IList<string> relations, int relationsDepth, AsyncCallback<Dictionary<string, object>> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      var responder = new AsyncCallback<Dictionary<string, object>>(
        response =>
        {
          foreach( string key in response.Keys )
          {
            entity.Remove( key );
            entity.Add( key, response[ key ] );
          }

          if( callback != null )
            callback.ResponseHandler.Invoke( response );
        },
        fault =>
        {
          if( callback != null )
            callback.ErrorHandler.Invoke( fault );
        } );

      object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, tableName, entity, relations, relationsDepth };
      Invoker.InvokeAsync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations", args, responder );
    }
  }
}
