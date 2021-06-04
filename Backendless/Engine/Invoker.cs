using System;
using System.Threading;
using System.Collections;
#if !(NET_35 || NET_40)
using System.Threading.Tasks;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Utils;
using Weborb.Client;
using Weborb.Exceptions;

namespace BackendlessAPI.Engine
{
  internal static class Invoker
  {
    private static readonly string URL_ENDING = "/binary";
    private static readonly string DESTINATION = "GenericDestination";

    private static readonly WeborbClient client =
      new WeborbClient( Backendless.InitAppData.FULL_QUERY_URL + URL_ENDING );

    public static int Timeout
    {
      get => client.Timeout;

      set => client.Timeout = value;
    }

    public static T InvokeSync<T>( string className, string methodName, object[] args )
    {
      return InvokeSync<T>( className, methodName, args, false );
    }

    public static T InvokeSync<T>( string className, string methodName, object[] args, bool enableUnderFlowInspection )
    {
      T result = default( T );
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
          responseConfig = SetupUnderFlowListener;

        client.Invoke( className, methodName, args, null, HeadersManager.GetInstance().Headers, responder,
                          responseConfig );
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
    #if !(NET_35 || NET_40)
    public static async Task<T> InvokeAsync<T>( string className, string methodName, object[] args )
    {
      return await InvokeAsync<T>( className, methodName, args, false );
    }
    
    public static async Task<T> InvokeAsync<T>( string className, string methodName, object[] args, bool enableUnderFlowInspection )
    {
      try
      {
        ResponseThreadConfigurator responseConfig = null;

        if( enableUnderFlowInspection )
          responseConfig = SetupUnderFlowListener;

        return await client.Invoke<T>( className, methodName, args, null, HeadersManager.GetInstance().Headers, responseConfig );
      }
      catch( System.Exception ex )
      {
        if( ex is WebORBException exception )
          throw new BackendlessException( new BackendlessFault( exception.Fault ) );

        throw new BackendlessException( ex.Message );
      }
    }
    #endif
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

    public static void InvokeAsync<T>( string className, string methodName, object[] args,
                                       bool enableUnderFlowInspection, AsyncCallback<T> callback )
    {
      var responder = new Responder<T>( response => { callback?.ResponseHandler( response ); }, fault =>
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
          responseConfig = SetupUnderFlowListener;

        client.Invoke<T>( className, methodName, args, null, HeadersManager.GetInstance().Headers, responder,
                          responseConfig );
      }
      catch( System.Exception ex )
      {
        var backendlessFault = new BackendlessFault( ex.Message );
        callback?.ErrorHandler( backendlessFault );
        throw new BackendlessException( backendlessFault );
      }
    }
  }
}