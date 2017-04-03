using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Engine;

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
