using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI.Async;

namespace BackendlessAPI.Counters
{
  class AtomicImpl<T> : IAtomic<T>
  {
    private String counterName;

    public AtomicImpl( String counterName )
    {
      this.counterName = counterName;
    }

    public void Reset()
    {
      CounterService.GetInstance().Reset( counterName );
    }

    public void Reset( AsyncCallback<Object> callback )
    {
      CounterService.GetInstance().Reset( counterName, callback );
    }

    public T Get()
    {
      return CounterService.GetInstance().Get<T>( counterName );
    }

    public void Get( Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().Get( counterName, callback );
    }

    public T GetAndIncrement()
    {
      return CounterService.GetInstance().GetAndIncrement<T>( counterName );
    }

    public void GetAndIncrement( Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().GetAndIncrement( counterName, callback );
    }

    public T IncrementAndGet()
    {
      return CounterService.GetInstance().IncrementAndGet<T>( counterName );
    }

    public void IncrementAndGet( Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().IncrementAndGet( counterName, callback );
    }

    public T GetAndDecrement()
    {
      return CounterService.GetInstance().GetAndDecrement<T>( counterName );
    }

    public void GetAndDecrement( Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().GetAndDecrement( counterName, callback );
    }

    public T DecrementAndGet()
    {
      return CounterService.GetInstance().DecrementAndGet<T>( counterName );
    }

    public void DecrementAndGet( Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().DecrementAndGet( counterName, callback );
    }

    public T AddAndGet( Int64 value )
    {
      return CounterService.GetInstance().AddAndGet<T>( counterName, value );
    }

    public void AddAndGet( Int64 value, Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().AddAndGet<T>( counterName, value, callback );
    }

    public T GetAndAdd( Int64 value )
    {
      return CounterService.GetInstance().GetAndAdd<T>( counterName, value );
    }

    public void GetAndAdd( Int64 value, Async.AsyncCallback<T> callback )
    {
      CounterService.GetInstance().GetAndAdd( counterName, value, callback );
    }

    public bool CompareAndSet( Int64 expected, Int64 updated )
    {
      return CounterService.GetInstance().CompareAndSet( counterName, expected, updated );
    }

    public void CompareAndSet( Int64 expected, Int64 updated, Async.AsyncCallback<bool> callback )
    {
      CounterService.GetInstance().CompareAndSet( counterName, expected, updated, callback );
    }
  }
}
