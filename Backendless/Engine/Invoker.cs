using System.Threading;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using Weborb.Client;

namespace BackendlessAPI.Engine
{
  internal static class Invoker
  {
    private static string URL_ENDING = "/binary";
    private static string DESTINATION = "GenericDestination";

    private static WeborbClient client = new WeborbClient( Backendless.URL + "/" + Backendless.VersionNum + URL_ENDING,
                                                           DESTINATION );

    public static T InvokeSync<T>( string className, string methodName, object[] args )
    {
      T result = default(T);
      BackendlessFault backendlessFault = null;
      AutoResetEvent waiter = new AutoResetEvent( false );

      var responder = new Responder<T>( r =>
        {
          result = r;
          waiter.Set();
        }, f =>
          {
            backendlessFault = new BackendlessFault( f );
            waiter.Set();
          } );
      try
      {
        client.Invoke<T>( className, methodName, args, HeadersManager.GetInstance().Headers, responder );
        waiter.WaitOne( Timeout.Infinite );
      }
      catch( System.Exception ex )
      {
        throw new BackendlessException( ex.Message );
      }

      if( backendlessFault != null )
        throw new BackendlessException( backendlessFault );

      return result;
    }

    public static void InvokeAsync<T>( string className, string methodName, object[] args, AsyncCallback<T> callback )
    {
      var responder = new Responder<T>( response =>
        {
          if( callback != null )
            callback.ResponseHandler.Invoke( response );
        }, fault =>
          {
            var backendlessFault = new BackendlessFault( fault );
            if( callback != null )
              callback.ErrorHandler.Invoke( backendlessFault );
            else
              throw new BackendlessException( backendlessFault );
          } );

      try
      {
        client.Invoke<T>( className, methodName, args, HeadersManager.GetInstance().Headers, responder );
      }
      catch( System.Exception ex )
      {
        var backendlessFault = new BackendlessFault( ex.Message );
        if( callback != null )
          callback.ErrorHandler.Invoke( backendlessFault );
        throw new BackendlessException( backendlessFault );
      }
    }
  }
}