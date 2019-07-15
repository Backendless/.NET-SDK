using System;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif

namespace BackendlessAPI.Counters
{
  public class CounterService
  {
    private const String COUNTERS_SERVER_ALIAS = "com.backendless.services.redis.AtomicOperationService";

    private static readonly CounterService instance = new CounterService();

    public static CounterService GetInstance()
    {
      return instance;
    }

    private CounterService()
    {
    }

    public IAtomic<T> Of<T>( String counterName )
    {
      return new AtomicImpl<T>( counterName );
    }

    #region RESET
    #if !(NET_35 || NET_40)
      public async Task ResetAsync( String counterName )
      {
        await Task.Run( () => Reset( counterName ) ).ConfigureAwait( false );
      }
    #endif  
    public void Reset( String counterName )
    {
      Invoker.InvokeSync<Object>( COUNTERS_SERVER_ALIAS, "reset", new object[] { counterName } );
    }

    public void Reset( String counterName, AsyncCallback<Object> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "reset", new object[] { counterName }, callback );
    }
    #endregion

    #region GET
    #if !(NET_35 || NET_40)
      public async Task<int> GetAsync( String counterName )
      {
        return await Task.Run( () => Get( counterName ) ).ConfigureAwait( false );
      }

      public async Task<T> GetAsync<T>( String counterName )
      {
        return await Task.Run( () => Get<T>( counterName ) ).ConfigureAwait( false );
      }
    #endif 
    public int Get( String counterName )
    {
      return RunGetOperation<int>( "get", counterName );
    }

    public T Get<T>( String counterName )
    {
      return RunGetOperation<T>( "get", counterName );
    }

    public void Get<T>( String counterName, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "get", new object[] { counterName }, callback );
    }
    #endregion

    #region GET_AND_INC
    #if !(NET_35 || NET_40)
      public async Task<int> GetAndIncrementAsync( String counterName )
      {
        return await Task.Run( () => GetAndIncrement( counterName ) ).ConfigureAwait( false );
      }

      public async Task<T> GetAndIncrementAsync<T>( String counterName )
      {
        return await Task.Run( () => GetAndIncrement<T>( counterName ) ).ConfigureAwait( false );
      }
    #endif  
    public int GetAndIncrement( String counterName )
    {
      return GetAndIncrement<int>( counterName );
    }

    public T GetAndIncrement<T>( String counterName )
    {
      return RunGetOperation<T>( "getAndIncrement", counterName );
    }

    public void GetAndIncrement<T>( String counterName, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "getAndIncrement", new object[] { counterName }, callback );
    }
    #endregion

    #region INC_AND_GET
    #if !(NET_35 || NET_40)
      public async Task<int> IncrementAndGetAsync( String counterName )
      {
        return await Task.Run( () => IncrementAndGet( counterName ) ).ConfigureAwait( false );
      }

      public async Task<T> IncrementAndGetAsync<T>( String counterName )
      {
        return await Task.Run( () => IncrementAndGet<T>( counterName ) ).ConfigureAwait( false );
      }
    #endif  
    public int IncrementAndGet( String counterName )
    {
      return IncrementAndGet<int>( counterName );
    }

    public T IncrementAndGet<T>( String counterName )
    {
      return RunGetOperation<T>( "incrementAndGet", counterName );
    }

    public void IncrementAndGet<T>( String counterName, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "incrementAndGet", new object[] { counterName }, callback );
    }
    #endregion

    #region GET_AND_DEC
    #if !(NET_35 || NET_40)
      public async Task<int> GetAndDecrementAsync( String counterName )
      {
        return await Task.Run( () => GetAndDecrement( counterName ) ).ConfigureAwait( false );
      }

      public async Task<T> GetAndDecrementAsync<T>( String counterName )
      {
        return await Task.Run( () => GetAndDecrement<T>( counterName ) ).ConfigureAwait( false );
      }
    #endif  
    public int GetAndDecrement( String counterName )
    {
      return GetAndDecrement<int>( counterName );
    }

    public T GetAndDecrement<T>( String counterName )
    {
      return RunGetOperation<T>( "getAndDecrement", counterName );
    }

    public void GetAndDecrement<T>( String counterName, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "getAndDecrement", new object[] {counterName }, callback );
    }
    #endregion

    #region DEC_AND_GET
    #if !(NET_35 || NET_40)
      public async Task<int> DecrementAndGetAsync( String counterName )
      {
        return await Task.Run( () => DecrementAndGet( counterName ) ).ConfigureAwait( false );
      }

      public async Task<T> DecrementAndGetAsync<T>( String counterName )
      {
        return await Task.Run( () => DecrementAndGet<T>( counterName ) ).ConfigureAwait( false );
      }
    #endif
    public int DecrementAndGet( String counterName )
    {
      return DecrementAndGet<int>( counterName );
    }

    public T DecrementAndGet<T>( String counterName )
    {
      return RunGetOperation<T>( "decrementAndGet", counterName );
    }

    public void DecrementAndGet<T>( String counterName, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "decrementAndGet", new object[] { counterName }, callback );
    }
    #endregion

    #region ADD_AND_GET
    #if !(NET_35 || NET_40)
      public async Task<int> AddAndGetAsync( String counterName, Int64 value )
      {
        return await Task.Run( () => AddAndGet( counterName, value ) ).ConfigureAwait( false );
      }

      public async Task<T> AddAndGetAsync<T>( String counterName, Int64 value )
      {
        return await Task.Run( () => AddAndGet<T>( counterName, value ) ).ConfigureAwait( false );
      }
    #endif  
    public int AddAndGet( String counterName, Int64 value )
    {
      return AddAndGet<int>( counterName, value );
    }

    public T AddAndGet<T>( String counterName, Int64 value )
    {
      return RunGetOperation<T>( "addAndGet", counterName, value );
    }

    public void AddAndGet<T>( String counterName, Int64 value, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "addAndGet", new object[] { counterName, value }, callback );
    }
    #endregion

    #region GET_AND_ADD
    #if !(NET_35 || NET_40)
      public async Task<int> GetAndAddAsync( String counterName, Int64 value )
      {
        return await Task.Run( () => GetAndAdd( counterName, value ) ).ConfigureAwait( false );
      }

      public async Task<T> GetAndAddAsync<T>( String counterName, Int64 value )
      {
        return await Task.Run( () => GetAndAdd<T>( counterName, value ) ).ConfigureAwait( false );
      }
    #endif  
    public int GetAndAdd( String counterName, Int64 value )
    {
      return GetAndAdd<int>( counterName, value );
    }

    public T GetAndAdd<T>( String counterName, Int64 value )
    {
      return RunGetOperation<T>( "getAndAdd", counterName, value );
    }

    public void GetAndAdd<T>( String counterName, Int64 value, AsyncCallback<T> callback )
    {
      Invoker.InvokeAsync( COUNTERS_SERVER_ALIAS, "getAndAdd", new object[] { counterName, value }, callback );
    }
    #endregion

    #region COMPARE
    #if !(NET_35 || NET_40)
      public async Task<bool> CompareAndSetAsync( String counterName, Int64 expected, Int64 updated )
      {
        return await Task.Run( () => CompareAndSet( counterName, expected, updated ) ).ConfigureAwait( false );
      }
    #endif  
    public bool CompareAndSet( String counterName, Int64 expected, Int64 updated )
    {
      return Invoker.InvokeSync<bool>( COUNTERS_SERVER_ALIAS, "compareAndSet", new object[] { counterName, expected, updated } );
    }

    public void CompareAndSet( String counterName, Int64 expected, Int64 updated, AsyncCallback<bool> callback )
    {
      Invoker.InvokeAsync<bool>( COUNTERS_SERVER_ALIAS, "compareAndSet", new object[] { counterName, expected, updated }, callback );
    }
    #endregion

    private T RunGetOperation<T>( String operationName, String counterName )
    {
      return Invoker.InvokeSync<T>( COUNTERS_SERVER_ALIAS, operationName, new object[] { counterName } );
    }

    private T RunGetOperation<T>( String operationName, String counterName, Int64 value )
    {
      return Invoker.InvokeSync<T>( COUNTERS_SERVER_ALIAS, operationName, new object[] { counterName, value } );
    }
  }
}
