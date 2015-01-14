using BackendlessAPI.Exception;

namespace BackendlessAPI.Async
{
  public delegate void ErrorHandler( BackendlessFault _backendlessFault );
  public delegate void ResponseHandler<T>( T response );

  public class AsyncCallback<T>
  {
    internal ErrorHandler ErrorHandler;
    internal ResponseHandler<T> ResponseHandler;

    public AsyncCallback( ResponseHandler<T> responseHandler, ErrorHandler errorHandler )
    {
      this.ResponseHandler = responseHandler;
      this.ErrorHandler = errorHandler;
    }
  }
}
