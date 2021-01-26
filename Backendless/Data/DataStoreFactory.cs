using System;
using System.Collections.Generic;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using BackendlessAPI.Service;
#if WITHRT
using BackendlessAPI.RT.Data;

#endif

namespace BackendlessAPI.Data
{
  internal static class DataStoreFactory
  {
    private static readonly Dictionary<Type, Object> dataStores = new Dictionary<Type, Object>();

    internal static IDataStore<T> CreateDataStore<T>()
    {
      if( !dataStores.ContainsKey( typeof( T ) ) )
        dataStores[ typeof( T ) ] = new DataStoreImpl<T>();

      return (IDataStore<T>) dataStores[ typeof( T ) ];
    }

    private class DataStoreImpl<T> : IDataStore<T>
    {
    #region RT

    #if WITHRT
      private readonly IEventHandler<T> eventHandler = EventHandlerFactory.Of<T>();
      public IEventHandler<T> RT()
      {
        return this.eventHandler;
      }
    #endif

    #endregion

    #region Bulk Create

      public IList<String> Create( IList<T> objects )
      {
        return Backendless.Persistence.Create( objects );
      }
    #if !(NET_35 || NET_40)
      public async Task<IList<String>> CreateAsync( IList<T> objects )
      {
        return await Task.Run( () => Create( objects ) ).ConfigureAwait( false );
      }
    #endif
      public void Create( IList<T> objects, AsyncCallback<IList<String>> responder )
      {
        Backendless.Persistence.Create( objects, responder );
      }

    #endregion

    #region Bulk Update

      public Int32 Update( String whereClause, Dictionary<String, Object> changes )
      {
        return Backendless.Persistence.Update( PersistenceService.GetTypeName( typeof( T ) ), whereClause, changes );
      }

    #if !(NET_35 || NET_40)
      public async Task<Int32> UpdateAsync( String whereClause, Dictionary<String, Object> changes )
      {
        return await Task.Run( () => Update( whereClause, changes ) ).ConfigureAwait( false );
      }
    #endif

      public void Update( String whereClause, Dictionary<String, Object> changes, AsyncCallback<Int32> callback )
      {
        Backendless.Persistence.Update( PersistenceService.GetTypeName( typeof( T ) ), whereClause, changes, callback );
      }

    #endregion

    #region Bulk Delete

      public Int32 Remove( String whereClause )
      {
        return Backendless.Persistence.Remove( PersistenceService.GetTypeName( typeof( T ) ), whereClause );
      }

    #if !(NET_35 || NET_40)
      public async Task<Int32> RemoveAsync( String whereClause )
      {
        return await Task.Run( () => Remove( whereClause ) ).ConfigureAwait( false );
      }
    #endif

      public void Remove( String whereClause, AsyncCallback<Int32> callback )
      {
        Backendless.Persistence.Remove( PersistenceService.GetTypeName( typeof( T ) ), whereClause, callback );
      }

      #endregion

      #region Deep Save

      public T DeepSave( T entity )
      {
        return Backendless.Persistence.DeepSave( entity );
      }

#if !(NET_35 || NET_40 )
      public async Task<T> DeepSaveAsync( T entity )
      {
        return await Task.Run( () => DeepSave( entity ) ).ConfigureAwait( false );
      }
#endif

      public void DeepSave( T entity, AsyncCallback<T> callback )
      {
        Backendless.Persistence.DeepSave( entity, callback );
      }

      #endregion

      #region Save

      public T Save( T entity )
      {
        return Backendless.Persistence.Save( entity );
      }

    #if !(NET_35 || NET_40)
      public async Task<T> SaveAsync( T entity )
      {
        return await Task.Run( () => Save( entity ) ).ConfigureAwait( false );
      }
    #endif

      public void Save( T entity, AsyncCallback<T> responder )
      {
        Backendless.Persistence.Save( entity, responder );
      }

    #endregion

    #region Remove by Object

      public long Remove( T entity )
      {
        return Backendless.Persistence.Remove( entity );
      }

    #if !(NET_35 || NET_40)
      public async Task<long> RemoveAsync( T entity )
      {
        return await Task.Run( () => Remove( entity ) ).ConfigureAwait( false );
      }
    #endif

      public void Remove( T entity, AsyncCallback<long> responder )
      {
        Backendless.Persistence.Remove( entity, responder );
      }

    #endregion

    #region FindFirst

      public T FindFirst()
      {
        return Backendless.Persistence.First<T>();
      }

    #if !(NET_35 || NET_40)
      public async Task<T> FindFirstAsync()
      {
        return await Task.Run( () => FindFirst() ).ConfigureAwait( false );
      }

    #endif

      public void FindFirst( AsyncCallback<T> responder )
      {
        Backendless.Persistence.First( responder );
      }

    #endregion

    #region FindLast

      public T FindLast()
      {
        return Backendless.Persistence.Last<T>();
      }

    #if !(NET_35 || NET_40)
      public async Task<T> FindLastAsync()
      {
        return await Task.Run( () => FindLast() ).ConfigureAwait( false );
      }

    #endif

      public void FindLast( AsyncCallback<T> responder )
      {
        Backendless.Persistence.Last( responder );
      }


    #endregion

    #region Find

      public IList<T> Find()
      {
        return Backendless.Persistence.Find<T>( (DataQueryBuilder) null );
      }

      public IList<T> Find( DataQueryBuilder dataQueryBuilder )
      {
        return Backendless.Persistence.Find<T>( dataQueryBuilder );
      }

    #if !(NET_35 || NET_40)
      public async Task<IList<T>> FindAsync()
      {
        return await Task.Run( () => Find() ).ConfigureAwait( false );
      }

      public async Task<IList<T>> FindAsync( DataQueryBuilder queryBuilder )
      {
        return await Task.Run( () => Find( queryBuilder ) ).ConfigureAwait( false );
      }
    #endif
      public void Find( AsyncCallback<IList<T>> responder )
      {
        Backendless.Persistence.Find( (DataQueryBuilder) null, responder );
      }

      public void Find( DataQueryBuilder dataQueryBuilder, AsyncCallback<IList<T>> responder )
      {
        Backendless.Persistence.Find( dataQueryBuilder, responder );
      }

    #endregion

    #region FindById with ID

      public T FindById( String id )
      {
        return Backendless.Persistence.FindById<T>( id, null );
      }

      public T FindById( String id, Int32? relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( id, null, relationsDepth );
      }

      public T FindById( String id, IList<String> relations )
      {
        return Backendless.Persistence.FindById<T>( id, relations );
      }

      public T FindById( String id, IList<String> relations, Int32? relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( id, relations, relationsDepth );
      }

    #if !(NET_35 || NET_40)
      public async Task<T> FindByIdAsync( String id )
      {
        return await Task.Run( () => FindById( id ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( String id, Int32? relationsDepth )
      {
        return await Task.Run( () => FindById( id, relationsDepth ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( String id, IList<String> relations )
      {
        return await Task.Run( () => FindById( id, relations ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( String id, IList<String> relations, Int32? relationsDepth )
      {
        return await Task.Run( () => FindById( id, relations, relationsDepth ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( String id, DataQueryBuilder queryBuilder )
      {
        return await Task.Run( () => FindById( id, queryBuilder ) ).ConfigureAwait( false );
      }
    #endif
      public T FindById( String id, DataQueryBuilder queryBuilder )
      {
        return Backendless.Persistence.FindByIdViaDataQueryBuilder<T>( id, queryBuilder );
      }

      public void FindById( String id, DataQueryBuilder queryBuilder, AsyncCallback<T> callback )
      {
        Backendless.Persistence.FindByIdViaDataQueryBuilder( id, queryBuilder, callback );
      }

      public void FindById( String id, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, null, responder );
      }

      public void FindById( String id, Int32? relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, null, relationsDepth, responder );
      }

      public void FindById( String id, IList<String> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, relations, responder );
      }

      public void FindById( String id, IList<String> relations, Int32? relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById( id, relations, relationsDepth, responder );
      }

    #endregion

    #region FindById with Object

      public T FindById( T entity )
      {
        return Backendless.Persistence.FindById<T>( entity );
      }

      public void FindById( T entity, DataQueryBuilder queryBuilder, AsyncCallback<T> callback )
      {
        Backendless.Persistence.FindByPrimaryKey( entity, queryBuilder, callback );
      }

      public T FindById( T entity, DataQueryBuilder queryBuilder )
      {
        return Backendless.Persistence.FindByPrimaryKey( entity, queryBuilder );
      }

      public T FindById( T entity, Int32? relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( entity, relationsDepth );
      }

      public T FindById( T entity, IList<String> relations )
      {
        return Backendless.Persistence.FindById<T>( entity, relations );
      }

      public T FindById( T entity, IList<String> relations, Int32? relationsDepth )
      {
        return Backendless.Persistence.FindById<T>( entity, relations, relationsDepth );
      }

    #if !(NET_35 || NET_40)
      public async Task<T> FindByIdAsync( T entity )
      {
        return await Task.Run( () => FindById( entity ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( T entity, DataQueryBuilder queryBuilder )
      {
        return await Task.Run( () => FindById( entity, queryBuilder ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( T entity, Int32? relationsDepth )
      {
        return await Task.Run( () => FindById( entity, relationsDepth ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( T entity, IList<String> relations )
      {
        return await Task.Run( () => FindById( entity, relations ) ).ConfigureAwait( false );
      }

      public async Task<T> FindByIdAsync( T entity, IList<String> relations, Int32? relationsDepth )
      {
        return await Task.Run( () => FindById( entity, relations, relationsDepth ) ).ConfigureAwait( false );
      }
    #endif

      public void FindById( T entity, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, responder );
      }

      public void FindById( T entity, Int32? relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relationsDepth, responder );
      }

      public void FindById( T entity, IList<String> relations, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relations, responder );
      }

      public void FindById( T entity, IList<String> relations, Int32? relationsDepth, AsyncCallback<T> responder )
      {
        Backendless.Persistence.FindById<T>( entity, relations, relationsDepth, responder );
      }

    #endregion

    #region LOAD RELATIONS

      public IList<M> LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder )
      {
        return Backendless.Persistence.LoadRelations( PersistenceService.GetTypeName( typeof( T ) ), objectId,
                                                      queryBuilder );
      }

    #if !(NET_35 || NET_40)
      public async Task<IList<M>> LoadRelationsAsync<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder )
      {
        return await Task.Run( () => LoadRelations( objectId, queryBuilder ) ).ConfigureAwait( false );
      }
    #endif

      public void LoadRelations<M>( String objectId, LoadRelationsQueryBuilder<M> queryBuilder,
                                    AsyncCallback<IList<M>> responder )
      {
        Backendless.Persistence.LoadRelations( PersistenceService.GetTypeName( typeof( T ) ), objectId, queryBuilder,
                                               responder );
      }

    #endregion

    #region Get Object Count

      public Int32 GetObjectCount()
      {
        return Backendless.Persistence.GetObjectCount<T>();
      }

      public Int32 GetObjectCount( DataQueryBuilder dataQueryBuilder )
      {
        return Backendless.Persistence.GetObjectCount<T>( dataQueryBuilder );
      }

    #if !(NET_35 || NET_40)
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
        Backendless.Persistence.GetObjectCount<T>( responder );
      }

      public void GetObjectCount( DataQueryBuilder dataQueryBuilder, AsyncCallback<Int32> responder )
      {
        Backendless.Persistence.GetObjectCount<T>( dataQueryBuilder, responder );
      }

    #endregion

    #region ADD RELATION

      public void AddRelation( T parent, String columnName, Object[] children )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                         children );
      }

      public Int32 AddRelation( T parent, String columnName, String whereClause )
      {
        return Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                                whereClause );
      }

    #if !(NET_35 || NET_40)
      public async Task AddRelationAsync( T parent, String columnName, Object[] children )
      {
        await Task.Run( () => AddRelation( parent, columnName, children ) ).ConfigureAwait( false );
      }

      public async Task<Int32> AddRelationAsync( T parent, String columnName, String whereClause )
      {
        return await Task.Run( () => AddRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
      }
    #endif

      public void AddRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                         whereClause, callback );
      }

      public void AddRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback )
      {
        Backendless.Data.AddRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                         children, callback );
      }

    #endregion

    #region SET RELATION

      public Int32 SetRelation( T parent, String columnName, Object[] children )
      {
        return Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                                children );
      }

      public Int32 SetRelation( T parent, String columnName, String whereClause )
      {
        return Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                                whereClause );
      }

    #if !(NET_35 || NET_40)
      public async Task<Int32> SetRelationAsync( T parent, String columnName, Object[] children )
      {
        return await Task.Run( () => SetRelation( parent, columnName, children ) ).ConfigureAwait( false );
      }

      public async Task<Int32> SetRelationAsync( T parent, String columnName, String whereClause )
      {
        return await Task.Run( () => SetRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
      }
    #endif

      public void SetRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback )
      {
        Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                         whereClause, callback );
      }

      public void SetRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback )
      {
        Backendless.Data.SetRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                         children, callback );
      }

    #endregion

    #region DELETE RELATION

      public Int32 DeleteRelation( T parent, String columnName, Object[] children )
      {
        return Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent,
                                                   columnName, children );
      }

      public Int32 DeleteRelation( T parent, String columnName, String whereClause )
      {
        return Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent,
                                                   columnName, whereClause );
      }

    #if !(NET_35 || NET_40)
      public async Task<Int32> DeleteRelationAsync( T parent, String columnName, Object[] children )
      {
        return await Task.Run( () => DeleteRelation( parent, columnName, children ) ).ConfigureAwait( false );
      }

      public async Task<Int32> DeleteRelationAsync( T parent, String columnName, String whereClause )
      {
        return await Task.Run( () => DeleteRelation( parent, columnName, whereClause ) ).ConfigureAwait( false );
      }
    #endif

      public void DeleteRelation( T parent, String columnName, String whereClause, AsyncCallback<Int32> callback )
      {
        Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                            whereClause );
      }

      public void DeleteRelation( T parent, String columnName, Object[] children, AsyncCallback<Int32> callback )
      {
        Backendless.Data.DeleteRelation<T>( PersistenceService.GetTypeName( parent.GetType() ), parent, columnName,
                                            children, callback );
      }

    #endregion
    }
  }
}