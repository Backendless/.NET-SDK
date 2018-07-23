using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.RT;
using BackendlessAPI.RT.Command;
using BackendlessAPI.RT.Users;
using BackendlessAPI.Utils;
using BackendlessAPI.Messaging;
using Weborb.Types;

namespace BackendlessAPI.RT.Messaging
{
  public class ChannelImpl : RTListenerImpl, IChannel
  {
    private String channelName;
    private IRTClient rtClient = RTClientFactory.Get();
    private List<MessagingSubscription> messagingCallbacks = new List<MessagingSubscription>();
    private CommandListener<MessagingSubscription> commandListener;
    private ConnectListener<MessagingSubscription> connectListener;

    internal ChannelImpl( String channelName )
    {
      this.channelName = channelName;
      this.commandListener = new ChannelImpl.ChannelCommandListener( this );
      this.connectListener = new ChannelImpl.ChannelConnectListener( this, channelName );
      Join();
    }

    public HandleMessagingError ErrorHandler
    {
      get;
      set;
    }

    public void AddCommandListener<T>( CommandReceived<Command<T>> callback )
    {
      commandListener.AddCommandListener( callback, this );
    }

    public void AddJoinListener( AsyncCallback<Object> callback )
    {
      connectListener.AddConnectListener( callback );
    }

    public void AddMessageListener<T>( MessageReceived<T> callback )
    {
      AddMessageListener<T>( null, callback );
    }

    public void AddMessageListener<T>( string selector, MessageReceived<T> callback )
    {
      IRTCallback rtCallback = new RTCallback<T>( (Delegate) callback, result =>
      {
        try
        {
          IAdaptingType message;

          if( !typeof( T ).Equals( typeof( Message ) ) )
            message = WeborbSerializationHelper.AsAdaptingType( result, "message" );
          else
            message = result;

          T adaptedResponse = (T) message.adapt( typeof( T ) );

          callback( adaptedResponse );
        }
        catch( System.Exception ex )
        {
          ErrorHandler?.Invoke( RTErrorType.MESSAGELISTENER, new Exception.BackendlessFault( ex ) );
        }
      }, fault =>
      {
        ErrorHandler?.Invoke( RTErrorType.MESSAGELISTENER, fault );
      } );

      AddMessageListener( selector, rtCallback );
    }

    public void AddUserStatusListener( UserStatusChanged callback )
    {
      if( callback == null )
        throw new ArgumentNullException( "callback", "the callback argument cannot be null" );

      IRTCallback rtCallback = new RTCallback<UserStatusResponse>( (Delegate) callback, result =>
      {
        try
        {
          UserStatusResponse userStatusResponse = (UserStatusResponse) result.adapt( typeof( UserStatusResponse ) );
          callback?.Invoke( userStatusResponse );
        }
        catch( System.Exception ex )
        {
          ErrorHandler?.Invoke( RTErrorType.USERSTATUSLISTENER, new Exception.BackendlessFault( ex ) );
        }
      }, fault =>
      {
        ErrorHandler?.Invoke( RTErrorType.USERSTATUSLISTENER, fault );
      } );

      AddUserListener( rtCallback );
    }

    public bool IsJoined()
    {
      return connectListener.IsConnected();
    }

    public void Join()
    {
      connectListener.Connect();
    }

    public void Leave()
    {
      connectListener.Disconnect();
    }

    public void RemoveAllMessageListeners()
    {
      foreach( MessagingSubscription messagingSubscription in messagingCallbacks )
        if( IsJoined() )
          rtClient.Unsubscribe( messagingSubscription.Id );

      messagingCallbacks.Clear();
    }

    public void RemoveCommandListener<T>( CommandReceived<Command<T>> callback )
    {
      RemoveMessageListener( callback );
    }

    public void RemoveJoinListener( AsyncCallback<Object> callback )
    {
      connectListener.RemoveConnectListener( callback );
    }

    public void RemoveMessageListener<T>( MessageReceived<T> callback )
    {
      RemoveMessageListener( callback );
    }

    public void RemoveMessageListeners( string selector )
    {
      for( int i = messagingCallbacks.Count - 1; i >= 0; i-- )
      {
        MessagingSubscription messagingSubscription = messagingCallbacks[ i ];
        object callbackObject = messagingSubscription.Callback.UsersCallback;

        if( messagingSubscription.Selector != null && messagingSubscription.Selector.Equals( selector ) )
          RemoveSubscription( messagingSubscription, i );
      }
    }

    public void RemoveMessageListeners<T>( string selector, MessageReceived<T> callback )
    {
      for( int i = messagingCallbacks.Count - 1; i >= 0; i-- )
      {
        MessagingSubscription messagingSubscription = messagingCallbacks[ i ];
        object callbackObject = messagingSubscription.Callback.UsersCallback;

        if( (messagingSubscription.Selector == null || messagingSubscription.Selector.Equals( selector )) &&
           (callback == null || callbackObject.Equals( callback )) )
          RemoveSubscription( messagingSubscription, i );
      }
    }

    public void RemoveUserStatusListener( UserStatusChanged callback )
    {
      RemoveMessageListener( callback );
    }

    public void RemoveUserStatusListeners()
    {
      for( int i = messagingCallbacks.Count - 1; i >= 0; i-- )
      {
        MessagingSubscription messagingSubscription = messagingCallbacks[ i ];

        if( messagingSubscription.SubscriptionName == SubscriptionNames.PUB_SUB_USERS )
          RemoveSubscription( messagingSubscription, i );
      }
    }

    public void SendCommand( string type, object data )
    {
      SendCommand( type, data, null );
    }

    public void SendCommand( string type, object data, AsyncCallback<object> callback )
    {
      commandListener.SendCommand( type, data, callback );
    }

    private void RemoveMessageListener( Delegate @delegate )
    {
      for( int i = messagingCallbacks.Count - 1; i >= 0; i-- )
      {
        MessagingSubscription messagingSubscription = messagingCallbacks[ i ];
        object callbackObject = messagingSubscription.Callback.UsersCallback;

        if( callbackObject is Delegate && ((Delegate) callbackObject) == @delegate )
          RemoveSubscription( messagingSubscription, i );
      }
    }

    private void RemoveSubscription( MessagingSubscription messagingSubscription, int index )
    {
      messagingCallbacks.RemoveAt( index );

      if( IsJoined() )
        rtClient.Unsubscribe( messagingSubscription.Id );
    }

    private void AddMessageListener( String selector, IRTCallback rtCallback )
    {
      MessagingSubscription subscription;

      if( selector == null )
        subscription = MessagingSubscription.Subscribe( channelName, rtCallback );
      else
        subscription = MessagingSubscription.Subscribe( channelName, selector, rtCallback );

      messagingCallbacks.Add( subscription );

      if( IsJoined() )
        rtClient.Subscribe( subscription );
    }

    private void AddUserListener( IRTCallback rtCallback )
    {
      MessagingSubscription subscription = MessagingSubscription.UserStatus( channelName, rtCallback );

      messagingCallbacks.Add( subscription );

      if( IsJoined() )
        rtClient.Subscribe( subscription );
    }

    private class ChannelConnectListener : ConnectListener<MessagingSubscription>
    {
      private ChannelImpl parent;

      internal ChannelConnectListener( ChannelImpl parent, String channelName ) : base( channelName )
      {
        this.parent = parent;
      }

      public override void Connected()
      {
        foreach( MessagingSubscription messagingCallback in parent.messagingCallbacks )
          parent.rtClient.Subscribe( messagingCallback );

        parent.commandListener.Connected();
      }

      public override MessagingSubscription CreateSubscription( IRTCallback callback )
      {
        return MessagingSubscription.Connect( base.subject, callback );
      }
    }

    private class ChannelCommandListener : CommandListener<MessagingSubscription>
    {
      private ChannelImpl parent;

      internal ChannelCommandListener( ChannelImpl parent )
      {
        this.parent = parent;
      }

      protected override CommandRequest CreateCommandRequest( IRTCallback rtCallback )
      {
        return new MessagingCommandRequest( parent.channelName, rtCallback );
      }

      protected override MessagingSubscription CreateSubscription( IRTCallback rtCallback )
      {
        return MessagingSubscription.Command( parent.channelName, rtCallback );
      }

      protected override List<MessagingSubscription> GetSubscriptionHolder()
      {
        return parent.messagingCallbacks;
      }

      protected override bool IsConnected()
      {
        return parent.connectListener.IsConnected();
      }
    }
  }
}
