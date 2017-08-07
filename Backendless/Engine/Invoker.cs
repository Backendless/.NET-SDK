using System.Threading;
using System.Collections;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Utils;
using Weborb.Client;

namespace BackendlessAPI.Engine
{
  internal static class Invoker
  {
    private static string URL_ENDING = "/binary";
    private static string DESTINATION = "GenericDestination";

    private static WeborbClient client = new WeborbClient( Backendless.URL + "/" + Backendless.AppId + "/" + Backendless.APIKey + URL_ENDING,
                                                           DESTINATION );

    public static int Timeout
    {
      get
      {
        return client.Timeout;
      }

      set
      {
        client.Timeout = value;
      }
    }

    public static T InvokeSync<T>( string className, string methodName, object[] args )
    {
      return InvokeSync<T>( className, methodName, args, false );
    }
 
    public static T InvokeSync<T>( string className, string methodName, object[] args, bool enableUnderFlowInspection )
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
        ResponseThreadConfigurator responseConfig = null;

        if( enableUnderFlowInspection )
          responseConfig = new ResponseThreadConfigurator( SetupUnderFlowListener );

        client.Invoke<T>( className, methodName, args, null, HeadersManager.GetInstance().Headers, responder, responseConfig );
        waiter.WaitOne( System.Threading.Timeout.Infinite );
      }
      catch( System.Exception ex )
      {
        throw new BackendlessException( ex.Message );
      }

      if( backendlessFault != null )
        throw new BackendlessException( backendlessFault );

      return result;
    }

    public static void SetupUnderFlowListener()
    {
      IDictionary props = Weborb.Util.ThreadContext.getProperties();
      
      if( !props.Contains( "UnderFlowReceiver" ) )
        props.Add( "UnderFlowReceiver", new Weborb.Reader.ObjectUnderflow( UnderflowStore.ReportObjectUnderFlow ) );
    }

    public static void InvokeAsync<T>( string className, string methodName, object[] args, AsyncCallback<T> callback )
    {
      InvokeAsync<T>( className, methodName, args, false, callback );
    }

    public static void InvokeAsync<T>( string className, string methodName, object[] args, bool enableUnderFlowInspection, AsyncCallback<T> callback )
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
        ResponseThreadConfigurator responseConfig = null;

        if( enableUnderFlowInspection )
          responseConfig = new ResponseThreadConfigurator( SetupUnderFlowListener );

        client.Invoke<T>( className, methodName, args, null, HeadersManager.GetInstance().Headers, responder, responseConfig );
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