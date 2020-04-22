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
    private const string PERSISTENCE_MANAGER_SERVER_ALIAS = "com.backendless.services.persistence.PersistenceService";
    //private EventHandlerFactory<Dictionary<string, object>> eventHandlerFactory = new EventHandlerFactory<Dictionary<string, object>>();

    private readonly string tableName;

    public DictionaryDrivenDataStore( string tableName )
    {
      this.tableName = tableName;
    #if WITHRT
      eventHandler = EventHandlerFactory.Of( tableName );
    #endif
    }
    
  #region RT
  #if WITHRT
    private readonly IEventHandler<Dictionary<string, object>> eventHandler;
    
    public IEventHandler<Dictionary<string, object>> RT()
    {
      return this.eventHandler;
    }
  #endif
  #endregion
    
  #region Bulk Update

    public int Update( string whereClause, Dictionary<string, object> changes )
    {
      return Update( whereClause, changes, null, false );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<int> UpdateAsync( string whereClause, Dictionary<string, object> changes )
    {
      return await Task.Run( () => Update( whereClause, changes ) ).ConfigureAwait( false );
    }
  #endif

    public void Update( string whereClause, Dictionary<string, object> changes, AsyncCallback<int> callback )
    {
      Update( whereClause, changes, callback, true );
    }

    private int Update( string whereClause, Dictionary<string, object> changes, AsyncCallback<int> callback,
                        bool async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( string.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      if( changes == null || changes.Count == 0 )
        throw new ArgumentNullException( string.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object with changes" ) );

      object[] args = { tableName, whereClause, changes };

      if( async )
        Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "updateBulk", args );

      // not used
      return -1;
    }

  #endregion

  #region Bulk Create

    public IList<string> Create( IList<Dictionary<string, object>> objects )
    {
      if( objects == null )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object collection" ) );

      if( objects.Count == 0 )
        return new List<string>();

      object[] args = new object[] { tableName, objects };
      return Invoker.InvokeSync<IList<string>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk", args );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<IList<string>> CreateAsync( IList<Dictionary<string, object>> objects )
    {
      return await Task.Run( () => Create( objects ) ).ConfigureAwait( false );
    }
  #endif

    public void Create( IList<Dictionary<string, object>> objects, AsyncCallback<IList<string>> callback )
    {
      if( objects == null )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE,
                                                        "Object collection" ) );

      if( objects.Count == 0 )
        callback.ResponseHandler( new List<string>() );

      object[] args = new object[] { tableName, objects };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "createBulk", args, callback );
    }

  #endregion

  #region Bulk Delete

    public int Remove( string whereClause )
    {
      return Remove( whereClause, null, false );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<int> RemoveAsync( string whereClause )
    {
      return await Task.Run( () => Remove( whereClause ) ).ConfigureAwait( false );
    }
  #endif

    public void Remove( string whereClause, AsyncCallback<int> callback )
    {
      Remove( whereClause, callback, true );
    }

    private int Remove( string whereClause, AsyncCallback<int> callback, bool async )
    {
      if( whereClause == null || whereClause.Trim().Length == 0 )
        throw new ArgumentNullException( String.Format( ExceptionMessage.NULL_OR_EMPTY_TEMPLATE, "Where clause" ) );

      object[] args = new object[] { tableName, whereClause };

      if( async )
        Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args, callback );
      else
        return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "removeBulk", args );

      // not used
      return -1;
    }

  #endregion

  #region Save

    public Dictionary<string, object> Save( Dictionary<string, object> entity )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { tableName, entity };
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Dictionary<string, object>> SaveAsync( Dictionary<string, object> entity )
    {
      return await Task.Run( () => Save( entity ) ).ConfigureAwait( false );
    }
  #endif

    public void Save( Dictionary<string, object> entity, AsyncCallback<Dictionary<string, object>> callback )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      object[] args = new object[] { tableName, entity };
      Invoker.InvokeAsync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "save", args, callback );
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
    
  #if !(NET_35 || NET_40)
    public async Task<long> RemoveAsync( Dictionary<string, object> entity )
    {
      return await Task.Run( () => Remove( entity ) ).ConfigureAwait( false );
    }
  #endif

    public void Remove( Dictionary<string, object> entity, AsyncCallback<long> responder )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      Object[] args = new Object[] { tableName, entity };
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
      return Invoker.InvokeSync<Dictionary<String, Object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", CreateArgs( queryBuilder ) );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Dictionary<string, object>> FindFirstAsync()
    {
      return await Task.Run( () => FindFirst() ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindFirstAsync( DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => FindFirst( queryBuilder ) ).ConfigureAwait( false );
    } 
  #endif

    public void FindFirst( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindFirst( DataQueryBuilder.Create(), responder );
    }

    public void FindFirst( DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<string, object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "first", CreateArgs( queryBuilder ), responder );
    }

  #endregion

  #region Last

    public Dictionary<string, object> FindLast()
    {
      return FindLast( DataQueryBuilder.Create() );
    }

    public Dictionary<string, object> FindLast( DataQueryBuilder queryBuilder )
    {
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", CreateArgs( queryBuilder ) );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Dictionary<string, object>> FindLastAsync()
    {
      return await Task.Run( () => FindLast() ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindLastAsync( DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => FindLast( queryBuilder ) ).ConfigureAwait( false );
    } 
  #endif

    public void FindLast( AsyncCallback<Dictionary<string, object>> responder )
    {
      FindLast( DataQueryBuilder.Create(), responder );
    }

    public void FindLast( DataQueryBuilder queryBuilder, AsyncCallback<Dictionary<string, object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "last", CreateArgs( queryBuilder ), responder );
    }

  #endregion

  #region Find

    public IList<Dictionary<string, object>> Find()
    {
      return Find( (DataQueryBuilder) null );
    }

    public IList<Dictionary<string, object>> Find( DataQueryBuilder dataQueryBuilder )
    {
      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();

      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      PersistenceService.CheckPageSizeAndOffset( dataQuery );
      
      object[] args = { tableName, dataQuery };
      return Invoker.InvokeSync<IList<Dictionary<string, object>>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<IList<Dictionary<string, object>>> FindAsync()
    {
      return await Task.Run( () => Find() ).ConfigureAwait( false );
    }
    
    public async Task<IList<Dictionary<string, object>>> FindAsync( DataQueryBuilder queryBuilder )
    {
      return await Task.Run( () => Find( queryBuilder ) ).ConfigureAwait( false );
    } 
  #endif

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

      if( dataQueryBuilder == null )
        dataQueryBuilder = DataQueryBuilder.Create();

      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();

      object[] args = { tableName, dataQuery };
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "find", args, responder );
    }

  #endregion

  #region Find By Id

    public Dictionary<string, object> FindById( string id )
    {
      return FindById( id, 0 );
    }

    public Dictionary<string, object> FindById( string id, int? relationsDepth )
    {
      return FindById( id, null, relationsDepth );
    }

    public Dictionary<string, object> FindById( string id, IList<string> relations )
    {
      return FindById( id, relations, 0 );
    }

    public Dictionary<string, object> FindById( string id, IList<string> relations, int? relationsDepth )
    {
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( id, relations, relationsDepth ) );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity )
    {
      return FindById( entity, null, 0 );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, int? relationsDepth )
    {
      return FindById( entity, null, relationsDepth );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, IList<string> relations )
    {
      return FindById( entity, relations, 0 );
    }

    public Dictionary<string, object> FindById( Dictionary<string, object> entity, IList<string> relations, int? relationsDepth )
    {
      return Invoker.InvokeSync<Dictionary<string, object>>( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById",
                                                                                 CreateArgs( entity, relations, relationsDepth ) );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<Dictionary<string, object>> FindByIdAsync( string id )
    {
      return await Task.Run( () => FindById( id ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindByIdAsync( string id, int? relationsDepth )
    {
      return await Task.Run( () => FindById( id, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindByIdAsync( string id, IList<string> relations )
    {
      return await Task.Run( () => FindById( id, relations ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<string, object>> FindByIdAsync( string id, IList<string> relations,
                                                                 int? relationsDepth )
    {
      return await Task.Run( () => FindById( id, relations, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindByIdAsync( Dictionary<string, object> entity )
    {
      return await Task.Run( () => FindById( entity ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindByIdAsync( Dictionary<string, object> entity, int? relationsDepth )
    {
      return await Task.Run( () => FindById( entity, relationsDepth ) ).ConfigureAwait( false );
    }
    
    public async Task<Dictionary<string, object>> FindByIdAsync( Dictionary<string, object> entity, IList<string> relations )
    {
      return await Task.Run( () => FindById( entity, relations ) ).ConfigureAwait( false );
    }

    public async Task<Dictionary<string, object>> FindByIdAsync( Dictionary<string, object> entity, IList<string> relations,
                                                                 int? relationsDepth )
    {
      return await Task.Run( () => FindById( entity, relations, relationsDepth ) ).ConfigureAwait( false );
    }
  #endif

    public void FindById( string id, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, null, 0, responder );
    }

    public void FindById( string id, int? relationsDepth, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, null, relationsDepth, responder );
    }

    public void FindById( string id, IList<string> relations, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( id, relations, 0, responder );
    }

    public void FindById( string id, IList<string> relations, int? relationsDepth,
                                                         AsyncCallback<Dictionary<string, object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( id, relations, relationsDepth ), responder );
    }

    public void FindById( Dictionary<string, object> entity, AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, null, 0, responder );
    }

    public void FindById( Dictionary<string, object> entity, int? relationsDepth,
                          AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, null, relationsDepth, responder );
    }

    public void FindById( Dictionary<string, object> entity, IList<string> relations,
                          AsyncCallback<Dictionary<string, object>> responder )
    {
      FindById( entity, relations, 0, responder );
    }

    public void FindById( Dictionary<string, object> entity, IList<string> relations, int? relationsDepth,
                          AsyncCallback<Dictionary<string, object>> responder )
    {
      Invoker.InvokeAsync( PERSISTENCE_MANAGER_SERVER_ALIAS, "findById", CreateArgs( entity, relations, relationsDepth ), responder );
    }

  #endregion

  #region Load Relations

    public IList<M> LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder )
    {
      return Backendless.Persistence.LoadRelations<M>( tableName, objectId, queryBuilder );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<IList<M>> LoadRelationsAsync<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder )
    {
      return await Task.Run( () => LoadRelations( objectId, queryBuilder ) ).ConfigureAwait( false );
    }
  #endif

    public void LoadRelations<M>( string objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                                  AsyncCallback<IList<M>> responder )
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
      return Invoker.InvokeSync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName, dataQuery },
                                      true );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<int> GetObjectCountAsync()
    {
      return await Task.Run( () => GetObjectCount() ).ConfigureAwait( false );
    }
    
    public async Task<int> GetObjectCountAsync( DataQueryBuilder dataQueryBuilder )
    {
      return await Task.Run( () => GetObjectCount( dataQueryBuilder ) ).ConfigureAwait( false );
    }
  #endif

    public void GetObjectCount( AsyncCallback<int> responder )
    {
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName }, true,
                                responder );
    }

    public void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<int> responder )
    {
      BackendlessDataQuery dataQuery = dataQueryBuilder.Build();
      Invoker.InvokeAsync<int>( PERSISTENCE_MANAGER_SERVER_ALIAS, "count", new object[] { tableName, dataQuery }, true,
                                responder );
    }

  #endregion

  #region ADD RELATION

    public void AddRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }

    public int AddRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }
    
  #if !(NET_35 || NET_40)
    public async Task AddRelationAsync( Dictionary<string, object> parent, string columnName, object[] children )
    {
      await Task.Run( () => AddRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<int> AddRelationAsync( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return await Task.Run( () => AddRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
  #endif

    public void AddRelation( Dictionary<string, object> parent, string columnName, string whereClause,
                             AsyncCallback<int> callback )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause, callback );
    }
    
    public void AddRelation( Dictionary<string, object> parent, string columnName, object[] children,
                             AsyncCallback<int> callback )
    {
      Backendless.Data.AddRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }
  #endregion

  #region SET RELATION

    public int SetRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      return Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }

    public int SetRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }
    
  #if !(NET_35 || NET_40)
    public async Task<int> SetRelationAsync( Dictionary<string, object> parent, string columnName, object[] children )
    {
      return await Task.Run( () => SetRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<int> SetRelationAsync( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return await Task.Run( () => SetRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
  #endif

    public void SetRelation( Dictionary<string, object> parent, string columnName, string whereClause,
                             AsyncCallback<int> callback )
    {
      Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause, callback );
    }
    
    public void SetRelation( Dictionary<string, object> parent, string columnName, object[] children,
                             AsyncCallback<int> callback )
    {
      Backendless.Data.SetRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }

  #endregion

  #region DELETE RELATION

    public int DeleteRelation( Dictionary<string, object> parent, string columnName, object[] children )
    {
      return Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, children );
    }
    
    public int DeleteRelation( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause );
    }

  #if !(NET_35 || NET_40)
    public async Task<int> DeleteRelationAsync( Dictionary<string, object> parent, string columnName, object[] children )
    {
      return await Task.Run( () => DeleteRelation( parent, columnName, children) ).ConfigureAwait( false );
    }
    
    public async Task<int> DeleteRelationAsync( Dictionary<string, object> parent, string columnName, string whereClause )
    {
      return await Task.Run( () => DeleteRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
    }
  #endif

    public void DeleteRelation( Dictionary<string, object> parent, string columnName, string whereClause,
                                AsyncCallback<int> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, whereClause,
                                                                   callback );
    }
    
    public void DeleteRelation( Dictionary<string, object> parent, string columnName, object[] children,
                                AsyncCallback<int> callback )
    {
      Backendless.Data.DeleteRelation<Dictionary<string, object>>( tableName, parent, columnName, children, callback );
    }

    #endregion

  #region CREATE_ARGS
    private Object[] CreateArgs( DataQueryBuilder qb )
    {
      return SubArgsCreator<Object>( qb.GetRelated(), qb.GetRelationsDepth() );
    }

    private Object[] CreateArgs( String id, IList<String> relations, int? relationsDepth )
    {
      if( id == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ID );

      return SubArgsCreator<String>( relations, relationsDepth, id );
    }

    private Object[] CreateArgs( Dictionary<String, Object> entity, IList<String> relations, int? relationsDepth )
    {
      if( entity == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ENTITY );

      return SubArgsCreator<Dictionary<String, Object>>( relations, relationsDepth, entity );
    }

    private Object[] SubArgsCreator<T>( IList<String> relations, int? Depth, T obj = null ) where T : class
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