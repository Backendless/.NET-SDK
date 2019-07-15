using System;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif

namespace BackendlessAPI.Caching
{
public class Cache
{
  private const String CACHE_SERVER_ALIAS = "com.backendless.services.redis.CacheService";

  private static readonly Cache instance = new Cache();

  public static Cache GetInstance()
  {
    return instance;
  }

  private Cache()
  {
  }

  public ICache<T> With<T>( String key, T type )
  {
    return new CacheService<T>( type, key );
  }

  #region PUT
  #if !(NET_35 || NET_40)
    public async Task PutAsync( String key, Object obj, int expire )
    {
      await Task.Run( () => Put( key, obj, expire ) ).ConfigureAwait( false );
    }

    public async Task PutAsync( String key, Object obj )
    {
      await Task.Run( () => Put( key, obj ) ).ConfigureAwait( false );
    }
  #endif
  public void Put( String key, Object obj, int expire, AsyncCallback<Object> callback )
  {
    byte[] bytes = serialize( obj );
    Invoker.InvokeAsync(CACHE_SERVER_ALIAS, "putBytes", new object[] { key, bytes, expire }, callback );
  }

  public void Put( String key, Object obj, AsyncCallback<Object> callback )
  {
    Put( key, obj, 0, callback );
  }

  public void Put( String key, Object obj )
  {
    Put( key, obj, 0 );
  }

  public void Put( String key, Object obj, int expire )
  {
    byte[] bytes = serialize( obj );
    Invoker.InvokeSync<object>(CACHE_SERVER_ALIAS, "putBytes", new object[] { key, bytes, expire } );
  }
  #endregion
    
  #region GET
  #if !(NET_35 || NET_40)
    public async Task<T> GetAsync<T>( String key )
    {
      return await Task.Run( () => Get<T>( key ) ).ConfigureAwait( false ); 
    }
  #endif

  public T Get<T>( String key )
  {
    byte[] bytes = Invoker.InvokeSync<byte[]>( CACHE_SERVER_ALIAS, "getBytes", new Object[] { key } );

    if( bytes == null )
      return default(T);

    return (T) deserialize<T>( bytes );
  }

  public void Get<T>( String key, AsyncCallback<T> callback )
  {
    AsyncCallback<byte[]> interimCallback = new AsyncCallback<byte[]>(
      result =>
      {
        callback.ResponseHandler( (T) deserialize<T>( result ) );
      },
      fault =>
      {
        callback.ErrorHandler( fault );
      } );

    Invoker.InvokeAsync( CACHE_SERVER_ALIAS, "getBytes", new Object[] { key }, interimCallback );
  }
  #endregion
    
  #region CONTAINS
  #if !(NET_35 || NET_40)
    public async Task<Boolean> ContainsAsync( String key )
    {
      return await Task.Run( () => Contains( key ) ).ConfigureAwait( false ); 
    }
  #endif  
  public Boolean Contains( String key )
  {
    return Invoker.InvokeSync<Boolean>( CACHE_SERVER_ALIAS, "containsKey", new Object[] { key } );
  }

  public void Contains( String key, AsyncCallback<Boolean> callback )
  {
    Invoker.InvokeAsync( CACHE_SERVER_ALIAS, "containsKey", new Object[] { key }, callback );
  }
  #endregion
    
  #region EXPIREIN
  #if !(NET_35 || NET_40)
    public async Task ExpireInAsync( String key, int seconds )
    {
      await Task.Run( () => ExpireIn( key, seconds ) ).ConfigureAwait( false ); 
    }
  #endif  
  public void ExpireIn( String key, int seconds )
  {
    Invoker.InvokeSync<object>( CACHE_SERVER_ALIAS, "extendLife", new Object[] { key, seconds } );
  }

  public void ExpireIn( String key, int seconds, AsyncCallback<Object> callback )
  {
    Invoker.InvokeAsync( CACHE_SERVER_ALIAS, "extendLife", new Object[] { key, seconds }, callback );
  }
  #endregion
    
  #region EXPIREAT
  #if !(NET_35 || NET_40)
    public async Task ExpireAtAsync( String key, int timestamp )
    {
      await Task.Run( () => ExpireAt( key, timestamp ) ).ConfigureAwait( false ); 
    }
  #endif  
  public void ExpireAt( String key, int timestamp )
  {
    Invoker.InvokeSync<object>( CACHE_SERVER_ALIAS, "expireAt", new Object[] { key, timestamp } );
  }

  public void ExpireAt( String key, int timestamp, AsyncCallback<Object> callback )
  {
    Invoker.InvokeAsync( CACHE_SERVER_ALIAS, "expireAt", new Object[] { key, timestamp }, callback );
  }
  #endregion
    
  #region DELETE
  #if !(NET_35 || NET_40)
    public async Task DeleteAsync( String key )
    {
      await Task.Run( () => Delete( key ) ).ConfigureAwait( false ); 
    }
  #endif  
  public void Delete( String key )
  {
    Invoker.InvokeSync<object>( CACHE_SERVER_ALIAS, "delete", new Object[] { key } );
  }

  public void Delete( String key, AsyncCallback<Object> callback )
  {
    Invoker.InvokeAsync( CACHE_SERVER_ALIAS, "delete", new Object[] { key }, callback );
  }
  #endregion

  private static T deserialize<T>( byte[] bytes )
  {
    try
    {
      Weborb.Types.IAdaptingType adpatingType = (Weborb.Types.IAdaptingType) Weborb.Util.IO.Serializer.FromBytes( bytes, Weborb.Util.IO.Serializer.AMF3, true );
      Type adaptToType = typeof( T );
      return (T) ( (Weborb.Types.IAdaptingType) adpatingType ).adapt( adaptToType );
    }
    catch( System.Exception e )
    {
      throw new BackendlessException( e.Message );
    }
  }

  private static byte[] serialize( Object obj )
  {
    byte[] bytes;
    try
    {
      bytes = Weborb.Util.IO.Serializer.ToBytes( obj, Weborb.Util.IO.Serializer.AMF3 );
    }
    catch( System.Exception e )
    {
      throw new BackendlessException( e.Message );
    }
    return bytes;
  }
}
}
