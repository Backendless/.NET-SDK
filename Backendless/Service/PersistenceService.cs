﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Persistence;
using BackendlessAPI.Property;
using Weborb.Service;
using Weborb.Types;
using BackendlessAPI.Utils;

namespace BackendlessAPI.Service
{
  public class PersistenceService
  {
    private const String PERSISTENCE_MANAGER_SERVER_ALIAS = "com.backendless.services.persistence.PersistenceService";
    private const String DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE = "objectId";
    private const String DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE = "created";
    private const String DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE = "updated";
    private const String DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE = "ObjectId";
    private const String DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE = "Created";
    private const String DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE = "Updated";
    private static readonly Dictionary<String, DictionaryDrivenDataStore> dataStores = new Dictionary<String, DictionaryDrivenDataStore>();
    public const String LOAD_ALL_RELATIONS = "*";

    public readonly DataPermission Permissions = new DataPermission();

    public PersistenceService()
    {
      Types.AddClientClassMapping( "com.backendless.services.persistence.BackendlessDataQuery", typeof( BackendlessDataQuery ) );
      Types.AddClientClassMapping( "com.backendless.services.persistence.ObjectProperty", typeof( ObjectProperty ) );
      Types.AddClientClassMapping( "com.backendless.services.persistence.QueryOptions", typeof( QueryOptions ) );
    }
    #region Bulk Create
    internal IList<String> Create<T>( IList<T> objects )
    {
      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<IList<String>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk",
                     new Object[] { GetTypeName( typeof( T ) ), objects }, false );
    }

    internal void Create<T>( IList<T> objects, AsyncCallback<IList<String>> callback )
    {
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk",
                     new Object[] { GetTypeName( typeof( T ) ), objects }, false, callback );
    }
    #endregion
    #region Bulk Update
    internal int Update( String tableName, String whereClause, Dictionary<String, Object> changes )
    {
      return Update( tableName, whereClause, changes, null, false );
    }

    internal void Update( String tableName, String whereClause, Dictionary<String, Object> changes, AsyncCallback<int> callback )
    {
      Update( tableName, whereClause, changes, callback, true );
    }
    private int Update( String tableName, String whereClause, Dictionary<String, Object> changes, AsyncCallback<int> callback, bool async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      if( changes == null || changes.Count == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Object with changes" ) );

      Object[] args = new Object[] { tableName, whereClause, changes };

      if( async )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args );

      // not used
      return -1;
    }
    #endregion
    #region Bulk Delete
    internal int Remove( String tableName, String whereClause )
    {
      return Remove( tableName, whereClause, null, false );
    }

    internal void Remove( String tableName, String whereClause, AsyncCallback<int> callback )
    {
      Remove( tableName, whereClause, callback, true );
    }
    private int Remove( String tableName, String whereClause, AsyncCallback<int> callback, bool async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      Object[] args = new Object[] { tableName, whereClause };

      if( async )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args );

      // not used
      return -1;
    }
    #endregion

    #region

    public T DeepSave<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentException( ExceptionMessage.NULL_ENTITY );

        return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deepSave",
          new Object[] { entity.GetType().Name, entity }, true );
    }

    public void DeepSave<T>( T entity, AsyncCallback<T> callback )
    {
      if( entity == null )
        throw new ArgumentException( ExceptionMessage.NULL_ENTITY );

      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "deepSave", new Object[] { entity.GetType().Name, entity }, true, callback );
    }

    #endregion

    #region Save
    public T Save<T>( T entity, Boolean isUpsert = false )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      CheckEntityStructure<T>();

      //return GetEntityId( entity ) == null ? Create( entity ) : Update( entity );
      String operation;
      String objectId = GetEntityId( entity );

      if( isUpsert )
        operation = "upsert";
      else if( objectId == null )
        operation = "create";
      else
        operation = "save";

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, operation,
                                    new Object[] { GetTypeName( typeof( T ) ), entity }, true );
    }

    public void Save<T>( T entity, AsyncCallback<T> callback, Boolean isUpsert = false )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      CheckEntityStructure<T>();

      /*if( GetEntityId( entity ) == null )
        Create( entity, callback );
      else
        Update( entity, callback );*/

      String operation;
      String objectId = GetEntityId( entity );


      if( isUpsert )
        operation = "upsert";
      else if( objectId == null )
        operation = "create";
      else
        operation = "save";

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, operation,
                           new Object[] { GetTypeName( typeof( T ) ), entity }, true, callback );
    }
    #endregion
    #region Create
    internal T Create<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "create",
                                    new Object[] { GetTypeName( typeof( T ) ), entity }, true );
    }

    internal void Create<T>( T entity, AsyncCallback<T> callback )
    {
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "create",
                           new Object[] {GetTypeName( typeof( T ) ), entity}, true, callback );
    }
    #endregion
    #region Update
    internal T Update<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      T result = Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "update",
                                    new Object[] {GetTypeName( typeof( T ) ), entity}, true );
      return result;
    }

    internal void Update<T>( T entity, AsyncCallback<T> callback )
    {
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "update",
                           new object[] {GetTypeName( typeof( T ) ), entity}, true, callback );
    }
    #endregion
    #region Remove by Object
    internal long Remove<T>( T entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                                       new Object[] {GetTypeName( typeof( T ) ), entity} );
    }

    internal void Remove<T>( T entity, AsyncCallback<long> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                           new Object[] {GetTypeName( typeof( T ) ), entity}, callback );
    }
    #endregion 
    #region Remove by objectId
    internal long Remove<T>( String objectId )
    {
      if( String.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                                       new Object[] { GetTypeName( typeof( T ) ), objectId } );
    }

    internal void Remove<T>( String objectId, AsyncCallback<long> callback )
    {
      if( String.IsNullOrEmpty( objectId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove",
                           new Object[] { GetTypeName( typeof( T ) ), objectId }, callback );
    }
    #endregion 
    #region FindById with Id
    internal T FindById<T>( String id, IList<String> relations )
    {
      return FindById<T>( id, relations, 0 );
    }

    internal T FindById<T>( String id, IList<String> relations, int? relationsDepth )
    {
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs<T>( id, relations, relationsDepth ), true );
    }

    internal void FindById<T>( String id, IList<String> relations, AsyncCallback<T> callback )
    {
      FindById<T>( id, relations, 0, callback );
    }

    internal void FindById<T>( String id, IList<String> relations, int? relationsDepth, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs<T>( id, relations, relationsDepth ), true, callback );
    }
    #endregion
    #region FindById with Object
    internal T FindById<T>( T entity )
    {
      return FindById<T>( entity, null, 0 );
    }

    internal T FindById<T>( T entity, int? relationsDepth )
    {
      return FindById<T>( entity, null, relationsDepth );
    }

    internal T FindById<T>( T entity, IList<String> relations )
    {
      return FindById<T>( entity, relations, 0 );
    }

    internal T FindById<T>( T entity, IList<String> relations, int? relationsDepth )
    {
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs<T>( entity, relations, relationsDepth ), true );
    }

    internal void FindById<T>( T entity, AsyncCallback<T> responder )
    {
      FindById( entity, null, 0, responder );
    }

    internal void FindById<T>( T entity, int? relationsDepth, AsyncCallback<T> responder )
    {
      FindById( entity, null, relationsDepth, responder );
    }

    internal void FindById<T>( T entity, IList<String> relations, AsyncCallback<T> responder )
    {
      FindById( entity, relations, 0, responder );
    }

    internal void FindById<T>( T entity, IList<String> relations, int? relationsDepth, AsyncCallback<T> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( entity, relations, relationsDepth ), true, responder );
    }

    internal T FindByPrimaryKey<T>( T entity, DataQueryBuilder queryBuilder )
    {
      if( entity == null )
        throw new ArgumentException( ExceptionMessage.NULL_ENTITY );

      if( queryBuilder == null )
        queryBuilder = DataQueryBuilder.Create();

      AddWeborbPropertyMapping<T>();
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                                                                        new Object[] { GetTypeName( typeof( T ) ), entity, queryBuilder.Build() }, true );      
    }

    internal T FindByIdViaDataQueryBuilder<T>( String id, DataQueryBuilder queryBuilder )
    {
      if( id == null )
        throw new ArgumentException( ExceptionMessage.NULL_ID );

      if( queryBuilder == null )
        queryBuilder = DataQueryBuilder.Create();

      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { GetTypeName( typeof( T ) ), id, queryBuilder.Build() }, true );
    }

    internal void FindByPrimaryKey<T>( T entity, DataQueryBuilder queryBuilder, AsyncCallback<T> callback )
    {
        if( entity == null )
          throw new ArgumentException( ExceptionMessage.NULL_ENTITY );

        if( queryBuilder == null )
          queryBuilder = DataQueryBuilder.Create();

        AddWeborbPropertyMapping<T>();
        Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { GetTypeName( typeof( T ) ), entity, queryBuilder.Build() }, true, callback );
    }

    internal void FindByIdViaDataQueryBuilder<T>( String id, DataQueryBuilder queryBuilder, AsyncCallback<T> callback )
    {
      if( id == null )
        throw new ArgumentException( ExceptionMessage.NULL_ID );

      if( queryBuilder == null )
        queryBuilder = DataQueryBuilder.Create();

      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { GetTypeName( typeof( T ) ), id, queryBuilder.Build() }, true, callback );
    }

    #endregion

    #region Group
    internal GroupResult Group<T>(GroupDataQueryBuilder dataQueryBuilder)
    {
      return Invoker.InvokeSync<GroupResult>(PERSISTENCE_MANAGER_SERVER_ALIAS, "group", new Object[] { GetTypeName(typeof(T)), dataQueryBuilder.Build() });
    }

    internal void Group<T>(GroupDataQueryBuilder dataQueryBuilder, AsyncCallback<GroupResult> callback)
    {
      Invoker.InvokeAsync(PERSISTENCE_MANAGER_SERVER_ALIAS, "group", new Object[] { GetTypeName(typeof(T)), dataQueryBuilder }, callback);
    }
    #endregion
    #region LoadRelations

    internal IList<T> LoadRelations<T>( String parentType, String objectId, LoadRelationsQueryBuilder<T> queryBuilder )
    {
      return LoadRelationsImpl( parentType, objectId, queryBuilder, null );
    }

    internal void LoadRelations<T>( String parentType, String objectId, LoadRelationsQueryBuilder<T> queryBuilder, AsyncCallback<IList<T>> responder )
    {
      LoadRelationsImpl( parentType, objectId, queryBuilder, responder );
    }

    private IList<T> LoadRelationsImpl<T>( String parentType, String objectId, LoadRelationsQueryBuilder<T> queryBuilder, AsyncCallback<IList<T>> responder )
    {
      if( String.IsNullOrEmpty( objectId ) )
      {
        if( responder != null )
          responder.ErrorHandler( new BackendlessFault( ExceptionMessage.NULL_ID ) );
        else
          throw new ArgumentNullException( ExceptionMessage.NULL_ID );
      }

      if( queryBuilder == null )
      {
        String error = "Cannot execute load relations request. The queryBuilder argument must not be null";

        if( responder != null )
          responder.ErrorHandler( new BackendlessFault( error ) );
        else
          throw new ArgumentNullException( error );
      }

      BackendlessDataQuery dataQuery = queryBuilder.Build();
      String relationName = dataQuery.QueryOptions.Related[ 0 ];
      int pageSize = dataQuery.PageSize;
      int offset = dataQuery.Offset;

      AddWeborbPropertyMapping<T>();
      Object[] args = new Object[] { parentType, objectId, relationName, pageSize, offset };

      if( responder == null )
      {
        return (IList<T>) Invoker.InvokeSync<IList<T>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations", args, true );
      }
      else
      {
        Invoker.InvokeAsync<IList<T>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "loadRelations", args, true, responder );
        return null;
      }
    }
    #endregion
    #region Describe
    public List<ObjectProperty> Describe( String className )
    {
      if( String.IsNullOrEmpty( className ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY_NAME );

      return Invoker.InvokeSync<List<ObjectProperty>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "describe",
                                                       new Object[] {className} );
    }

    public void Describe( String className, AsyncCallback< List < ObjectProperty > > callback )
    {
      if( String.IsNullOrEmpty( className ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY_NAME );

      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "describe",
                           new Object[] {className}, callback );
    }
    #endregion
    #region Find
    
    public IList<T> Find<T>( DataQueryBuilder dataQueryBuilder )
    {
      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();
      
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      AddWeborbPropertyMapping<T>();
      var result = Invoker.InvokeSync<IList<T>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find",
                                                                 new Object[] { GetTypeName( typeof( T ) ), dataQuery}, true );
      return (IList<T>) result;
    }

    public void Find<T>( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<T>> callback )
    {
      var responder = new AsyncCallback<IList<T>>( r =>
        {
          if( callback != null )
            callback.ResponseHandler.Invoke( r );
        }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );

      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();
      
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      AddWeborbPropertyMapping<T>();
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find",
                           new Object[] {GetTypeName( typeof( T ) ), dataQuery},
                           true,
                           responder );
    }
    #endregion
    #region First

    public T First<T>()
    {
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", new Object[] { typeof(T).Name }, true );
    }

    public void First<T>( AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", new Object[] { typeof(T).Name }, true, callback );
    }
    #endregion 
    #region Last

    public T Last<T>(  )
    {
      return Invoker.InvokeSync<T>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", new Object[] { typeof( T ).Name }, true );
    }

    public void Last<T>( AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", new Object[] { typeof( T ).Name }, true, callback );
    }
    #endregion
    #region Get Object Count
    public int GetObjectCount<T>()
    {
      return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count",
                                                                 new Object[] { GetTypeName( typeof( T ) ) }, true );
    }

    public int GetObjectCount<T>( DataQueryBuilder dataQueryBuilder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count",
                                                                 new Object[] { GetTypeName( typeof( T ) ), dataQuery }, true );
    }
    public void GetObjectCount<T>( AsyncCallback<int> callback )
    {
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count",
                                                                 new Object[] { GetTypeName( typeof( T ) ) }, true, responder );
    }

    public void GetObjectCount<T>( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> callback )
    {
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count",
                                                                 new Object[] { GetTypeName( typeof( T ) ), dataQuery }, true, responder );
    }
    #endregion
    #region VIEWS
    public IList<Dictionary<String, Object>> GetView( String viewName, BackendlessDataQuery dataQuery )
    {
      CheckPageSizeAndOffset( dataQuery );

      Object[] args = new Object[] { viewName, dataQuery };
      return Invoker.InvokeSync<IList<Dictionary<String, Object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "callStoredView", args );
    }

    public void GetView( String viewName, BackendlessDataQuery query, AsyncCallback<Dictionary<String, Object>> responder )
    {
      CheckPageSizeAndOffset( query );

      Object[] args = new Object[] { viewName, query };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "callStoredView", args, responder );
    }
    #endregion
    #region STORED PROCEDURES
    public IList<Dictionary<Object, Object>> CallStoredProcedure( String spName, Dictionary<String, Object> arguments )
    {
      Object[] args = new Object[] { spName, arguments };

      return Invoker.InvokeSync<IList<Dictionary<Object, Object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "callStoredProcedure", args );
    }

    public void CallStoredProcedure( String procedureName, Dictionary<String, Object> arguments, AsyncCallback<IList<Dictionary<Object, Object>>> responder )
    {
      Object[] args = new Object[] { procedureName, arguments };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "callStoredProcedure", args, responder );
    }
    #endregion
    #region ADD RELATION
    public void AddRelation<T>( String parentTableName, T parent, String columnName, Object[] children )
    {
      AddRelation<T>( parentTableName, parent, columnName, children, null );
    }

    public void AddRelation<T>( String parentTableName, T parent, String columnName, Object[] children, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );

      IList<String> childObjectIds = new List<String>();
      foreach( Object child in children )
      {
        String childObjectId = GetEntityId<Object>( child );
        childObjectIds.Add( childObjectId );
      }

      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, childObjectIds };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "addRelation", args, true, callback );
      else 
        Invoker.InvokeSync<Object>( PERSISTENCE_MANAGER_SERVER_ALIAS, "addRelation", args, true );
    }

    public int AddRelation<T>( String parentTableName, T parent, String columnName, String whereClause )
    {
      return AddRelation<T>( parentTableName, parent, columnName, whereClause, null );
    }

    public int AddRelation<T>( String parentTableName, T parent, String columnName, String whereClause, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );
      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, whereClause };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "addRelation", args, true, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "addRelation", args, true );

      // needed for the async call - not used.
      return 0;
    }
    #endregion
    #region SET RELATION
    public int SetRelation<T>( String parentTableName, T parent, String columnName, Object[] children )
    {
      return SetRelation<T>( parentTableName, parent, columnName, children, null );
    }

    public int SetRelation<T>( String parentTableName, T parent, String columnName, Object[] children, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );

      IList<String> childObjectIds = new List<String>();
      foreach( Object child in children )
      {
        String childObjectId = GetEntityId<Object>( child );
        childObjectIds.Add( childObjectId );
      }

      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, childObjectIds };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "setRelation", args, true, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "setRelation", args, true );

      // needed for the async call - is not used
      return 0;
    }

    public int SetRelation<T>( String parentTableName, T parent, String columnName, String whereClause )
    {
      return SetRelation<T>( parentTableName, parent, columnName, whereClause, null );
    }

    public int SetRelation<T>( String parentTableName, T parent, String columnName, String whereClause, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );
      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, whereClause };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "setRelation", args, true, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "setRelation", args, true );

      // needed for the async call - not used.
      return 0;
    }

    #endregion
    #region DELETE RELATION
    public int DeleteRelation<T>( String parentTableName, T parent, String columnName, Object[] children )
    {
      return DeleteRelation<T>( parentTableName, parent, columnName, children );
    }

    public void DeleteRelation<T>( String parentTableName, T parent, String columnName, Object[] children, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );

      IList<String> childObjectIds = new List<String>();
      foreach( Object child in children )
      {
        if( child.GetType().Equals( typeof( String ) ) )
          childObjectIds.Add( (String) child );
        else
          childObjectIds.Add( GetEntityId<Object>( child ) );
      }

      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, childObjectIds };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deleteRelation", args, true, callback );
      else
        Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deleteRelation", args, true );
    }

    public int DeleteRelation<T>( String parentTableName, T parent, String columnName, String whereClause )
    {
      return DeleteRelation<T>( parentTableName, parent, columnName, whereClause, null );
    }

    public int DeleteRelation<T>( String parentTableName, T parent, String columnName, String whereClause, AsyncCallback<int> callback )
    {
      String parentObjectId = GetEntityId<T>( parent );
      Object[] args = new Object[] { parentTableName, columnName, parentObjectId, whereClause };

      if( callback != null )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deleteRelation", args, true, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deleteRelation", args, true );

      // needed for the async call - not used.
      return 0;
    }

    #endregion

    public IDataStore<T> Of<T>()
    {
      return DataStoreFactory.CreateDataStore<T>();
    }

    public IDataStore<Dictionary<String, Object>> Of( String tableName )
    {
     // if( tableName.ToLower().Equals( "users" ) )
     //   throw new System.Exception( "Table 'Users' is not accessible through this signature. Use Backendless.Data.Of( typeof( BackendlessUser ) ) instead" );

      if (!dataStores.ContainsKey( tableName ) )
        dataStores[tableName] = new DictionaryDrivenDataStore( tableName );
      
      return dataStores[ tableName ];
    }

    public void MapTableToType( String tableName, Type type )
    {
      Weborb.Types.Types.AddClientClassMapping( tableName, type );
    }

    public static String GetEntityId<T>( T entity )
    {
      if( entity is Dictionary<String,Object> )
      {
        Object untypedDictionary = entity;
        Dictionary<String, Object> dictionary = (Dictionary<String, Object>) untypedDictionary;
        return (String) dictionary[ DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE ];
      }

      try
      {
        Type entityType = entity.GetType();

        if( entityType == typeof( BackendlessUser ) )
        {
          Object entityObject = entity;
          return ( (BackendlessUser) entityObject ).ObjectId;
        }

        PropertyInfo objectIdProp = entityType.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );

        if( objectIdProp == null )
          objectIdProp = entityType.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

        if( objectIdProp != null )
          return objectIdProp.GetValue( entity, null ) as String;

        FieldInfo objectIdField = entityType.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

        if( objectIdField == null )
          objectIdField = entityType.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );

        if( objectIdField != null )
          return objectIdField.GetValue( entity ) as String;

        IDictionary<String, Object> underflow = UnderflowStore.GetObjectUnderflow( entity );

        if( underflow != null && underflow.ContainsKey( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE ) )
          return (String) underflow[ DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE ];
      }
      catch( System.Exception )
      {
      }

      return null;
    }

    private static void CheckEntityStructure<T>()
    {
      Type entityClass = typeof( T );

      if( entityClass.IsArray || entityClass.IsAssignableFrom( typeof( IEnumerable ) ) )
        throw new ArgumentException( ExceptionMessage.WRONG_ENTITY_TYPE );

      try
      {
        entityClass.GetConstructor( new Type[ 0 ] );
      }
      catch( System.Exception )
      {
        throw new ArgumentException( ExceptionMessage.ENTITY_MISSING_DEFAULT_CONSTRUCTOR );
      }

      /*
      // OBJECTID
      PropertyInfo objectIdProp = entityClass.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );
      Type objectIdType = null;

      if( objectIdProp == null )
        objectIdProp = entityClass.GetProperty( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

      if( objectIdProp != null )
      {
        objectIdType = objectIdProp.PropertyType;
      }
      else
      {
        FieldInfo objectIdField = entityClass.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE );

        if( objectIdField == null )
          objectIdField = entityClass.GetField( DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );

        if( objectIdField != null )
          objectIdType = objectIdField.FieldType;
      }

      if( objectIdType != null && objectIdType != typeof( String ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_OBJECT_ID_FIELD_TYPE );

      // CREATED FIELD/PROPERTY
      Type createdType = null;
      PropertyInfo createdProp = entityClass.GetProperty( DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE );

      if( createdProp == null )
        createdProp = entityClass.GetProperty( DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE );

      if( createdProp != null )
      {
        createdType = createdProp.PropertyType;
      }
      else
      {
        FieldInfo createdField = entityClass.GetField( DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE, BindingFlags.Public | BindingFlags.Instance );

        if( createdField == null )
          createdField = entityClass.GetField( DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE, BindingFlags.Instance | BindingFlags.Public );

        if( createdField != null )
          createdType = createdField.FieldType;
      }

      if( createdType != null && createdType != typeof( DateTime ) && createdType != typeof( DateTime? ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_CREATED_FIELD_TYPE );


      // UPDATED FIELD/PROPERTY
      Type updatedType = null;
      PropertyInfo updatedProp = entityClass.GetProperty( DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE );

      if( updatedProp == null )
        updatedProp = entityClass.GetProperty( DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE );

      if( updatedProp != null )
      {
        updatedType = updatedProp.PropertyType;
      }
      else
      {
        FieldInfo updatedField = entityClass.GetField( DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE, BindingFlags.Public | BindingFlags.Instance );

        if( updatedField == null )
          updatedField = entityClass.GetField( DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE, BindingFlags.Instance | BindingFlags.Public );

        if( updatedField != null )
          updatedType = updatedField.FieldType;
      }

      if( updatedType != null && updatedType != typeof( DateTime ) && updatedType != typeof( DateTime? ) )
        throw new ArgumentException( ExceptionMessage.ENTITY_WRONG_UPDATED_FIELD_TYPE );
       */ 
    }

    private static void AddWeborbPropertyMapping<T>()
    {
      var entityType = typeof( T );
      var entityProperties = entityType.GetProperties();

      foreach( var entityProperty in entityProperties )
        switch( entityProperty.Name )
        {
          case DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE:
            if(
              String.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_OBJECT_ID_PROPERTY_NAME_DOTNET_STYLE,
                                                DEFAULT_OBJECT_ID_PROPERTY_NAME_JAVA_STYLE );
            break;

          case DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE:
            if(
              String.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_UPDATED_FIELD_NAME_DOTNET_STYLE,
                                                DEFAULT_UPDATED_FIELD_NAME_JAVA_STYLE );
            break;

          case DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE:
            if(
              String.IsNullOrEmpty( PropertyRenaming.GetRenamingRule( entityType,
                                                                      DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE ) ) )
              PropertyRenaming.AddRenamingRule( entityType, DEFAULT_CREATED_FIELD_NAME_DOTNET_STYLE,
                                                DEFAULT_CREATED_FIELD_NAME_JAVA_STYLE );
            break;
        }
    }

    public static void CheckPageSizeAndOffset( IBackendlessQuery dataQuery )
    {
      if( dataQuery == null )
        return;

      if( dataQuery.Offset < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_OFFSET );

      if( dataQuery.PageSize < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_PAGE_SIZE );
    }

    public static String GetTypeName( Type type )
    {
      if( type.Equals( typeof( BackendlessUser ) ) )
        return "Users";
      else
      {
        String mappedName = Weborb.Types.Types.getClientClassForServerType( type.Name );

        if( mappedName != null )
          return mappedName;
        else
          return type.Name;
      }
    }

    #region CREATE_ARGS  
    public Object[] CreateArgs<T>( DataQueryBuilder qb )
    {
      return SubArgsCreator<T>( qb.GetRelated(), qb.GetRelationsDepth(), null );
    }

    public Object[] CreateArgs<T>( String id, IList<String> relations, int? relationsDepth )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      return SubArgsCreator<T>( relations, relationsDepth, id );
    }

    public Object[] CreateArgs<T>( T entity, IList<String> relations, int? relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      return SubArgsCreator<T>( relations, relationsDepth, entity );
    }

    private Object[] SubArgsCreator<T>( IList<String> relations, int? Depth, Object obj )
    {
      if( relations == null )
        relations = new List<String>();

      AddWeborbPropertyMapping<T>();
      List<Object> args = new List<Object> { GetTypeName( typeof( T ) ) };

      if( obj != null )
        args.Add( obj );

      args.Add( relations );

      if( Depth != null )
        args.Add( Depth );

      return args.ToArray();
    }
    #endregion
  }
}