using System;
using System.Collections.Generic;
#if !NET_35
using System.Collections.Concurrent;
#endif  
using BackendlessAPI.Exception;
using BackendlessAPI.RT;
using BackendlessAPI.RT.Messaging;
using BackendlessAPI.Async;
using BackendlessAPI.RT.Users;
using BackendlessAPI.Utils;
using Weborb.Util.Logging;
using Weborb.Types;

namespace BackendlessAPI.RT.Command
{
  public abstract class CommandListener<T> where T : RTSubscription
  {
    private readonly IRTClient rtClient = RTClientFactory.Get();
    #if NET_35
    private Queue<RTMethodRequest> commandsToSend = new Queue<RTMethodRequest>();
    #else
    private ConcurrentQueue<RTMethodRequest> commandsToSend = new ConcurrentQueue<RTMethodRequest>();
    #endif

    protected abstract List<T> GetSubscriptionHolder();

    protected abstract T CreateSubscription( IRTCallback rtCallback );

    protected abstract CommandRequest CreateCommandRequest( IRTCallback rtCallback );

    protected abstract Boolean IsConnected();

    public void Connected()
    {
#if NET_35
      var methodRequest = commandsToSend.Dequeue();
#else
      commandsToSend.TryDequeue( out var methodRequest );
      #endif

      while (methodRequest != null)
      {
        rtClient.Invoke(methodRequest);

#if NET_35
        methodRequest = commandsToSend.Dequeue();
#else
        commandsToSend.TryDequeue( out methodRequest );
  #endif
      }
    }

    public void AddCommandListener<TR>( CommandReceived<TR> callback, IChannel channel )
    {
      IRTCallback rtCallback = new RTCallback<TR>( callback, result =>
        {
          try
          {
            Command<TR> command = new Command<TR>();

            UserInfo userInfo = new UserInfo();

            command.UserInfo = userInfo;

            userInfo.ConnectionId = WeborbSerializationHelper.AsString( result, "connectionId" );
            userInfo.UserId = WeborbSerializationHelper.AsString( result, "userId" );

            command.Type = WeborbSerializationHelper.AsString( result, "type" );

            IAdaptingType data = WeborbSerializationHelper.AsAdaptingType( result, "data" );

            command.Data = (TR) data.adapt( typeof( TR ) );
            callback( command );
          }
          catch( System.Exception e )
          {
            channel.ErrorHandler?.Invoke( RTErrorType.COMMAND, new BackendlessFault( e ) );
          }
        }, fault =>
        {
          channel.ErrorHandler?.Invoke( RTErrorType.COMMAND, fault );
        } );

      AddCommandListener( rtCallback );
    }

    public void SendCommand( String type, Object data, AsyncCallback<Object> callback )
    {
      Log.log( Backendless.BACKENDLESSLOG, String.Format( "Send command with type {0}", type ) );
      CommandRequest rtMethodRequest = CreateCommandRequest( new RTCallback<Object>( callback, result =>
      {

        Log.log( Backendless.BACKENDLESSLOG, "command sent" );

        if( callback != null )
          callback.ResponseHandler( null );
      }, fault =>
      {

        Log.log( Backendless.BACKENDLESSLOG, String.Format( "error when sending command {0}", fault ) );
        if( callback != null )
          callback.ErrorHandler( fault );
      } ) );

      rtMethodRequest.SetData( data ).SetType( type );

      if( IsConnected() )
        rtClient.Invoke( rtMethodRequest );
      else
        commandsToSend.Enqueue( rtMethodRequest );
    }

    public void removeCommandListener( CommandReceived<T> callback )
    {
      List<T> subscriptionHolder = GetSubscriptionHolder();

      for( int i = subscriptionHolder.Count - 1; i >= 0; i-- )
      {
        T messagingSubscription = subscriptionHolder[ i ];

        if( messagingSubscription.Callback.UsersCallback.Equals( callback ) )
        {
          subscriptionHolder.RemoveAt( i );

          if( IsConnected() )
            rtClient.Unsubscribe( messagingSubscription.Id );
        }
      }
    }

    private void AddCommandListener( IRTCallback rtCallback )
    {
      Log.log( Backendless.BACKENDLESSLOG, "try to add command listener" );
      T subscription = CreateSubscription( rtCallback );
      Log.log( Backendless.BACKENDLESSLOG, String.Format( "subscription object {0}", subscription ) );
      GetSubscriptionHolder().Add( subscription );

      if( IsConnected() )
      {
        Log.log( Backendless.BACKENDLESSLOG, "subscription is connected try to subscribe" );
        rtClient.Subscribe( subscription );
      }
    }
  }
}
