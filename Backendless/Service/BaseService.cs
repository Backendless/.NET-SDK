using System;
using System.Collections.Generic;
using System.Text;

using BackendlessAPI.Engine;
using BackendlessAPI.Async;

namespace BackendlessAPI.Service
{
  public class BaseService
  {
    private static string SERVICE = "com.backendless.services.servercode.CustomServiceHandler";

    public T InvokeSync<T>( String serviceName, String serviceVersion, String method, Object[] args )
    {
      List<Object> methodArgs = new List<Object>();
      methodArgs.Add( serviceName );
      methodArgs.Add( serviceVersion );
      methodArgs.Add( method );
      methodArgs.AddRange( args );

      return Invoker.InvokeSync<T>( SERVICE, "dispatchService", methodArgs.ToArray() );
    }

    public void InvokeAsync<T>( String serviceName, String serviceVersion, String method, Object[] args, AsyncCallback<T> callback )
    {
      List<Object> methodArgs = new List<Object>();
      methodArgs.Add( serviceName );
      methodArgs.Add( serviceVersion );
      methodArgs.Add( method );
      methodArgs.AddRange( args );

      Invoker.InvokeAsync<T>( SERVICE, "dispatchService", methodArgs.ToArray(), callback );
    }
  }
}
