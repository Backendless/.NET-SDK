using System;
using Weborb.Types;
using BackendlessAPI.Async;

namespace BackendlessAPI.RT
{
  public class RTCallback<T> : AsyncCallback<IAdaptingType>, IRTCallback
  {
    private Delegate usersDelegateCallback;
    private AsyncCallback<T> usersCallback;

    public RTCallback( ResponseHandler<IAdaptingType> responseHandler, ErrorHandler errorHandler )
  : base( responseHandler, errorHandler )
    {

    }

    public RTCallback( AsyncCallback<T> usersCallback, ResponseHandler<IAdaptingType> responseHandler, ErrorHandler errorHandler )
      : base( responseHandler, errorHandler )
    {
      this.usersCallback = usersCallback;
    }

    public RTCallback( Delegate usersCallback, ResponseHandler<IAdaptingType> responseHandler, ErrorHandler errorHandler )
  : base( responseHandler, errorHandler )
    {
      this.usersDelegateCallback = usersCallback;
    }

    public Type Type
    {
      get { return typeof( T ); }
    }

    public Object UsersCallback
    {
      get { return usersCallback != null ? UsersCallback : usersDelegateCallback; }
    }

    public ResponseHandler<IAdaptingType> responseHandler
    {
      get
      {
        return ResponseHandler;
      }
    }

    public ErrorHandler errorHandler
    {
      get
      {
        return ErrorHandler;
      }
    }
  }

  public interface IRTCallback
  {
    Type Type { get; }
    Object UsersCallback { get; }
    ResponseHandler<IAdaptingType> responseHandler { get; }
    ErrorHandler errorHandler { get; }
  }
}
