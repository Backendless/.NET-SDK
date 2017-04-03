using System;
using System.Collections;
using System.Collections.Generic;

using BackendlessAPI.Engine;
using BackendlessAPI.Async;

namespace BackendlessAPI
{
  public class Events
  {
    private const String EVENTS_MANAGER_SERVER_ALIAS = "com.backendless.services.servercode.EventHandler";

    private static readonly Events instance = new Events();

    public static Events GetInstance()
    {
      return instance;
    }

    // synchronous method
    public IDictionary Dispatch( String eventName, IDictionary eventArgs )
    {
      return Invoker.InvokeSync<IDictionary>( EVENTS_MANAGER_SERVER_ALIAS, "dispatchEvent", new object[] { eventName, eventArgs } );
    }

    // asynchronous method
    public void Dispatch( String eventName, IDictionary eventArgs, AsyncCallback<IDictionary> callback )
    {
      Invoker.InvokeAsync( EVENTS_MANAGER_SERVER_ALIAS, "dispatchEvent", new object[] { eventName, eventArgs }, callback );
    }
  }
}
