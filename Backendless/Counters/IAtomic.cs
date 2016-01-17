using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI.Async;

namespace BackendlessAPI.Counters
{
  public interface IAtomic<T>
  {
    void Reset();

    void Reset( AsyncCallback<Object> responder );

    T Get();

    void Get( AsyncCallback<T> responder );

    T GetAndIncrement();

    void GetAndIncrement( AsyncCallback<T> responder );

    T IncrementAndGet();

    void IncrementAndGet( AsyncCallback<T> responder );

    T GetAndDecrement();

    void GetAndDecrement( AsyncCallback<T> responder );

    T DecrementAndGet();

    void DecrementAndGet( AsyncCallback<T> responder );

    T AddAndGet( Int64 value );

    void AddAndGet( Int64 value, AsyncCallback<T> responder );

    T GetAndAdd( Int64 value );

    void GetAndAdd( Int64 value, AsyncCallback<T> responder );

    bool CompareAndSet( Int64 expected, Int64 updated );

    void CompareAndSet( Int64 expected, Int64 updated, AsyncCallback<Boolean> responder );
  }
}
