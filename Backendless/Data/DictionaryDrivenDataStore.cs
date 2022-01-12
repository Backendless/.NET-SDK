using System;
using System.Collections.Generic;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif
using BackendlessAPI.Exception;
using BackendlessAPI.Engine;
using BackendlessAPI.Service;
using BackendlessAPI.Persistence;
using BackendlessAPI.Async;
#if WITHRT
using BackendlessAPI.RT.Data;

#endif

namespace BackendlessAPI.Data
{
  class DictionaryDrivenDataStore : IDataStore<Dictionary<String, Object>>
  {
    private const String PERSISTENCE_MANAGER_SERVER_ALIAS = "com.backendless.services.persistence.PersistenceService";
    //private EventHandlerFactory<Dictionary<String, Object>> eventHandlerFactory = new EventHandlerFactory<Dictionary<String, Object>>();

    private readonly String tableName;

    public DictionaryDrivenDataStore( String tableName )
    {
      this.tableName = tableName;
    #if WITHRT
      eventHandler = EventHandlerFactory.Of( tableName );
    #endif
    }
    
  #region RT
  #if WITHRT
    private readonly IEventHandler<Dictionary<String, Object>> eventHandler;
    
    public IEventHandler<Dictionary<String, Object>> RT()
    {
      return this.eventHandler;
    }
  #endif
  #endregion
    
  #region Bulk Update

    public Int32 Update( String whereClause, Dictionary<String, Object> changes )
    {
      return Update( whereClause, changes, null, false );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Int32> UpdateAsync( String whereClause, Dictionary<String, Object> changes )
    {
      return await Task.Run( () => Update( whereClause, changes ) ).ConfigureAwait( false );
    }
  #endif

    public void Update( String whereClause, Dictionary<String, Object> changes, AsyncCallback<Int32> callback )
    {
      Update( whereClause, changes, callback, true );
    }

    private Int32 Update( String whereClause, Dictionary<String, Object> changes, AsyncCallback<Int32> callback,
                        Boolean async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      if( changes == null || changes.Count == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object with changes" ) );

      Object[] args = { tableName, whereClause, changes };

      if( async )
        Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args, callback );
      else
        return Invoker.InvokeSync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args );

      // not used
      return -1;
    }

  #endregion

  #region Bulk Create

    public IList<String> Create( IList<Dictionary<String, Object>> objects )
    {
      if( objects == null )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object collection" ) );

      if( objects.Count == 0 )
        return new List<String>();

      Object[] args = new Object[] { tableName, objects };
      return Invoker.InvokeSync<IList<String>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk", args );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<IList<String>> CreateAsync( IList<Dictionary<String, Object>> objects )
    {
      return await Task.Run( () => Create( objects ) ).ConfigureAwait( false );
    }
  #endif

    public void Create( IList<Dictionary<String, Object>> objects, AsyncCallback<IList<String>> callback )
    {
      if( objects == null )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object collection" ) );

      if( objects.Count == 0 )
        callback.ResponseHandler( new List<String>() );

      Object[] args = new Object[] { tableName, objects };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk", args, callback );
    }

  #endregion

  #region Bulk Delete

    public Int32 Remove( String whereClause )
    {
      return Remove( whereClause, null, false );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Int32> RemoveAsync( String whereClause )
    {
      return await Task.Run( () => Remove( whereClause ) ).ConfigureAwait( false );
    }
  #endif

    public void Remove( String whereClause, AsyncCallback<Int32> callback )
    {
      Remove( whereClause, callback, true );
    }

    private Int32 Remove( String whereClause, AsyncCallback<Int32> callback, Boolean async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      Object[] args = new Object[] { tableName, whereClause };

      if( async )
        Invoker.InvokeAsync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args, callback );
      else
        return Invoker.InvokeSync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args );

      // not used
      return -1;
    }

    #endregion

    #region Save

    public Dictionary<String, Object> Save( Dictionary<String, Object> entity, Boolean isUpsert = false )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      String operation = "save";

      if( isUpsert )
        operation = "upsert";

      Object[] args = new Object[] { tableName, entity };
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, operation, args );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Dictionary<String, Object>> SaveAsync( Dictionary<String, Object> entity, Boolean isUpsert = false )
    {
      return await Task.Run( () => Save( entity, isUpsert ) ).ConfigureAwait( false );
    }
#endif

    public void Save( Dictionary<String, Object> entity, AsyncCallback<Dictionary<String, Object>> callback, Boolean isUpsert = false )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      String operation = "save";

      if( isUpsert )
        operation = "upsert";

      Object[] args = new Object[] { tableName, entity };
      Invoker.InvokeAsync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, operation, args, callback );
    }

    #endregion

    #region Deep Save
    public Dictionary<String, Object> DeepSave( Dictionary<String, Object> map )
    {
      if( map == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deepSave", new Object[] { tableName, map } );
    }

#if !(NET_35 || NET_40)
    public async Task<Dictionary<String, Object>> DeepSaveAsync( Dictionary<String, Object> map )
    {
      return await Task.Run( () => DeepSave( map ) ).ConfigureAwait( false );
    }
#endif
    public void DeepSave( Dictionary<String, Object> map, AsyncCallback<Dictionary<String, Object>> callback )
    {
      if( map == null )
        throw new ArgumentException( ExceptionMessage.NULL_MAP );

      Invoker.InvokeAsync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "deepSave",
                                                       new Object[] { tableName, map }, true, callback );
    }
    #endregion
    #region Remove

    public long Remove( Dictionary<String, Object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { tableName, entity };
      return Invoker.InvokeSync<long>( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<long> RemoveAsync( Dictionary<String, Object> entity )
    {
      return await Task.Run( () => Remove( entity ) ).ConfigureAwait( false );
    }
#endif

    public void Remove( Dictionary<String, Object> entity, AsyncCallback<long> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { tableName, entity };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "remove", args, responder );
    }

#endregion

#region First

    public Dictionary<String, Object> FindFirst()
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", new Object[] { tableName } );
    }
    
#if !( NET_35 || NET_40 )
    
    public async Task<Dictionary<String, Object>> FindFirstAsync()
    {
      return await Task.Run( () => FindFirst() ).ConfigureAwait( false );
    } 
#endif

    public void FindFirst( AsyncCallback<Dictionary<String, Object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", new Object[] { tableName }, responder );
    }

#endregion

#region Last

    public Dictionary<String, Object> FindLast()
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", new Object[] { tableName } );
    }
    
#if !( NET_35 || NET_40 )
    
    public async Task<Dictionary<String, Object>> FindLastAsync()
    {
      return await Task.Run( () => FindLast() ).ConfigureAwait( false );
    } 
#endif

    public void FindLast( AsyncCallback<Dictionary<String, Object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", new Object[] { tableName }, responder );
    }

#endregion

#region Find

    public IList<Dictionary<String, Object>> Find()
    {
      return Find( (DataQueryBuilder) null );
    }

    public IList<Dictionary<String, Object>> Find( DataQueryBuilder dataQueryBuilder )
    {
      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();

      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      PersistenceService.CheckPageSizeAndOffset( dataQuery );
      
      Object[] args = { tableName, dataQuery };
      return Invoker.InvokeSync<IList<Dictionary<String, Object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<IList<Dictionary<String, Object>>> FindAsync()
    {
      return await Task.Run( () => Find() ).ConfigureAwait( false );
    }
    
    public async Task<IList<Dictionary<String, Object>>> FindAsync( DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => Find( queryBuilder ) ).ConfigureAwait( false );
    } 
#endif

    public void Find( AsyncCallback<IList<Dictionary<String, Object>>> responder )
    {
      Find( (DataQueryBuilder) null, responder );
    }

    public void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<Dictionary<String, Object>>> callback )
    {
      var responder = new AsyncCallback<IList<Dictionary<String, Object>>>(
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

      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();

      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();

      Object[] args = { tableName, dataQuery };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args, responder );
    }

#endregion

#region Find By Id

    public Dictionary<String, Object> FindById( String id )
    {
      return FindById( id, 0 );
    }

    public Dictionary<String, Object> FindById( String id, DataQueryBuilder queryBuilder )
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { tableName, id, queryBuilder.Build() } );
    }

    public Dictionary<String, Object> FindById( Dictionary<String, Object> entity, DataQueryBuilder queryBuilder )
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { tableName, entity, queryBuilder.Build() } );
    }

    public void FindById( String id, DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<String, Object>> callback )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { tableName, id, queryBuilder.Build() }, callback );
    }

    public void FindById( Dictionary<String, Object> entity, DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<String, Object>> callback )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", new Object[] { tableName, entity, queryBuilder.Build() }, callback );
    }

    public Dictionary<String, Object> FindById( String id, Int32? relationsDepth )
    {
      return FindById( id, null, relationsDepth );
    }

    public Dictionary<String, Object> FindById( String id, IList<String> relations )
    {
      return FindById( id, relations, 0 );
    }

    public Dictionary<String, Object> FindById( String id, IList<String> relations, Int32? relationsDepth )
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( id, relations, relationsDepth ) );
    }

    public Dictionary<String, Object> FindById( Dictionary<String, Object> entity )
    {
      return FindById( entity, null, 0 );
    }

    public Dictionary<String, Object> FindById( Dictionary<String, Object> entity, Int32? relationsDepth )
    {
      return FindById( entity, null, relationsDepth );
    }

    public Dictionary<String, Object> FindById( Dictionary<String, Object> entity, IList<String> relations )
    {
      return FindById( entity, relations, 0 );
    }

    public Dictionary<String, Object> FindById( Dictionary<String, Object> entity, IList<String> relations, Int32? relationsDepth )
    {
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                                                                                 CreateArgs( entity, relations, relationsDepth ) );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<Dictionary<String, Object>> FindByIdAsync( String id )
    {
      return await Task.Run( () => FindById( id ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<String, Object>> FindByIdAsync( String id, DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => FindById( id, queryBuilder ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<String, Object>> FindByIdAsync( Dictionary<String, Object> entity, DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => FindById( entity, queryBuilder ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<String, Object>> FindByIdAsync( String id, Int32? relationsDepth )
    {
      return await Task.Run( () => FindById( id, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<String, Object>> FindByIdAsync( String id, IList<String> relations )
    {
      return await Task.Run( () => FindById( id, relations ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<String, Object>> FindByIdAsync( String id, IList<String> relations,
                                                                 Int32? relationsDepth )
    {
      return await Task.Run( () => FindById( id, relations, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<String, Object>> FindByIdAsync( Dictionary<String, Object> entity )
    {
      return await Task.Run( () => FindById( entity ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<String, Object>> FindByIdAsync( Dictionary<String, Object> entity, Int32? relationsDepth )
    {
      return await Task.Run( () => FindById( entity, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<String, Object>> FindByIdAsync( Dictionary<String, Object> entity, IList<String> relations )
    {
      return await Task.Run( () => FindById( entity, relations ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<String, Object>> FindByIdAsync( Dictionary<String, Object> entity, IList<String> relations,
                                                                 Int32? relationsDepth )
    {
      return await Task.Run( () => FindById( entity, relations, relationsDepth ) ).ConfigureAwait( false );
    }
#endif

    public void FindById( String id, AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( id, null, 0, responder );
    }

    public void FindById( String id, Int32? relationsDepth, AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( id, null, relationsDepth, responder );
    }

    public void FindById( String id, IList<String> relations, AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( id, relations, 0, responder );
    }

    public void FindById( String id, IList<String> relations, Int32? relationsDepth,
                                                         AsyncCallback<Dictionary<String, Object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( id, relations, relationsDepth ), responder );
    }

    public void FindById( Dictionary<String, Object> entity, AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( entity, null, 0, responder );
    }

    public void FindById( Dictionary<String, Object> entity, Int32? relationsDepth,
                          AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( entity, null, relationsDepth, responder );
    }

    public void FindById( Dictionary<String, Object> entity, IList<String> relations,
                          AsyncCallback<Dictionary<String, Object>> responder )
    {
      FindById( entity, relations, 0, responder );
    }

    public void FindById( Dictionary<String, Object> entity, IList<String> relations, Int32? relationsDepth,
                          AsyncCallback<Dictionary<String, Object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( entity, relations, relationsDepth ), responder );
    }

#endregion

#region Load Relations

    public IList<M> LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder )
    {
      return Backendless.Persistence.LoadRelations<M>( tableName, objectId, queryBuilder );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<IList<M>> LoadRelationsAsync<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder )
    {
      return await Task.Run( () => LoadRelations( objectId, queryBuilder ) ).ConfigureAwait( false );
    }
#endif

    public void LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                                  AsyncCallback<IList<M>> responder )
    {
      Backendless.Persistence.LoadRelations<M>( tableName, objectId, queryBuilder, responder );
    }

#endregion

#region Get Object Count

    public Int32 GetObjectCount()
    {
      return Invoker.InvokeSync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new Object[] { tableName }, true );
    }

    public Int32 GetObjectCount( DataQueryBuilder dataQueryBuilder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      return Invoker.InvokeSync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new Object[] { tableName, dataQuery },
                                      true );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<Int32> GetObjectCountAsync()
    {
      return await Task.Run( () => GetObjectCount() ).ConfigureAwait( false );
    }
    
    public async Task<Int32> GetObjectCountAsync( DataQueryBuilder dataQueryBuilder )
    {
      return await Task.Run( () => GetObjectCount( dataQueryBuilder ) ).ConfigureAwait( false );
    }
#endif

    public void GetObjectCount( AsyncCallback<Int32> responder )
    {
      Invoker.InvokeAsync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new Object[] { tableName }, true,
                                responder );
    }

    public void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<Int32> responder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      Invoker.InvokeAsync<Int32>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new Object[] { tableName, dataQuery }, true,
                                responder );
    }

#endregion

#region ADD RELATION

    public void AddRelation( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      Backendless.Data.AddRelation<Dictionary<String, Object>>( tableName, parent, columnName, children );
    }

    public Int32 AddRelation( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return Backendless.Data.AddRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause );
    }
    
#if !( NET_35 || NET_40 )
    public async Task AddRelationAsync( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      await Task.Run( () => AddRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<Int32> AddRelationAsync( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return await Task.Run( () => AddRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
#endif

    public void AddRelation( Dictionary<String, Object> parent, String columnName, String whereClause,
                             AsyncCallback<Int32> callback )
    {
      Backendless.Data.AddRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause, callback );
    }
    
    public void AddRelation( Dictionary<String, Object> parent, String columnName, Object[] children,
                             AsyncCallback<Int32> callback )
    {
      Backendless.Data.AddRelation<Dictionary<String, Object>>( tableName, parent, columnName, children, callback );
    }
#endregion

#region SET RELATION

    public Int32 SetRelation( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      return Backendless.Data.SetRelation<Dictionary<String, Object>>( tableName, parent, columnName, children );
    }

    public Int32 SetRelation( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return Backendless.Data.SetRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause );
    }
    
#if !( NET_35 || NET_40 )
    public async Task<Int32> SetRelationAsync( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      return await Task.Run( () => SetRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<Int32> SetRelationAsync( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return await Task.Run( () => SetRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
#endif

    public void SetRelation( Dictionary<String, Object> parent, String columnName, String whereClause,
                             AsyncCallback<Int32> callback )
    {
      Backendless.Data.SetRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause, callback );
    }
    
    public void SetRelation( Dictionary<String, Object> parent, String columnName, Object[] children,
                             AsyncCallback<Int32> callback )
    {
      Backendless.Data.SetRelation<Dictionary<String, Object>>( tableName, parent, columnName, children, callback );
    }

#endregion

#region DELETE RELATION

    public Int32 DeleteRelation( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      return Backendless.Data.DeleteRelation<Dictionary<String, Object>>( tableName, parent, columnName, children );
    }
    
    public Int32 DeleteRelation( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return Backendless.Data.DeleteRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause );
    }

#if !( NET_35 || NET_40 )
    public async Task<Int32> DeleteRelationAsync( Dictionary<String, Object> parent, String columnName, Object[] children )
    {
      return await Task.Run( () => DeleteRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<Int32> DeleteRelationAsync( Dictionary<String, Object> parent, String columnName, String whereClause )
    {
      return await Task.Run( () => DeleteRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
#endif

    public void DeleteRelation( Dictionary<String, Object> parent, String columnName, String whereClause,
                                AsyncCallback<Int32> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<String, Object>>( tableName, parent, columnName, whereClause,
                                                                   callback );
    }
    
    public void DeleteRelation( Dictionary<String, Object> parent, String columnName, Object[] children,
                                AsyncCallback<Int32> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<String, Object>>( tableName, parent, columnName, children, callback );
    }

#endregion

#region CREATE_ARGS
    private Object[] CreateArgs( DataQueryBuilder qb )
    {
      return SubArgsCreator( qb.GetRelated(), qb.GetRelationsDepth() );
    }

    private Object[] CreateArgs( String id, IList<String> relations, Int32? relationsDepth )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      return SubArgsCreator( relations, relationsDepth, id );
    }

    private Object[] CreateArgs( Dictionary<String, Object> entity, IList<String> relations, Int32? relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );
      
      return SubArgsCreator( relations, relationsDepth, entity );
    }

    private Object[] SubArgsCreator( IList<String> relations, Int32? Depth, Object obj = null )
    {   
      if( relations == null )
        relations = new List<String>();
      
      List<Object> args = new List<Object> { tableName };

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