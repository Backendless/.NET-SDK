using System;
namespace BackendlessAPI.Async
{
  public delegate void ResultHandler<T>( T result );
  public delegate void Handle<T>( T result );

  public abstract class Result<T>
  {
    public Handle<T> Handle;
  }
}
