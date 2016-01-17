using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI.Async;

namespace BackendlessAPI.Caching
{
  public interface ICache<T>
  {
    void Put( T value, AsyncCallback<Object> callback );

    void Put( T value, int expire, AsyncCallback<Object> callback );

    void Put( T value );

    void Put( T value, int expire );

    void Get( AsyncCallback<T> callback );

    T Get();

    void Contains( AsyncCallback<Boolean> callback );

    Boolean Contains();

    void ExpireIn( int seconds, AsyncCallback<Object> callback );

    void ExpireIn( int seconds );

    void ExpireAt( int seconds, AsyncCallback<Object> callback );

    void ExpireAt( int seconds );

    void Delete( AsyncCallback<Object> callback );

    void Delete();
  }
}
