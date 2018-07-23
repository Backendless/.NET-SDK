using System;
using BackendlessAPI.Async;
using BackendlessAPI.RT;
using BackendlessAPI.RT.Command;
using BackendlessAPI.RT.Users;

namespace BackendlessAPI.RT.Messaging
{
  public delegate void MessageReceived<T>( T message );
  public delegate void CommandReceived<T>( Command<T> command );
  public delegate void UserStatusChanged( UserStatusResponse userStatus );
  public delegate void HandleMessagingError( RTErrorType errorType, Exception.BackendlessFault backendlessFault );

  public interface IChannel
  {
    HandleMessagingError ErrorHandler
    {
      get;
      set;
    }

    void Join();

    void Leave();

    Boolean IsJoined();

    void AddJoinListener( AsyncCallback<Object> callback );

    void RemoveJoinListener( AsyncCallback<Object> callback );

    //----------------------------------

    void AddMessageListener<T>( MessageReceived<T> callback );

    void AddMessageListener<T>( String selector, MessageReceived<T> callback );

    void RemoveMessageListeners( String selector );

    void RemoveMessageListener<T>( MessageReceived<T> callback );

    void RemoveMessageListeners<T>( String selector, MessageReceived<T> callback );

    void RemoveAllMessageListeners();

    //----------------------------------

    void AddCommandListener<T>( CommandReceived<Command<T>> callback );

    void SendCommand( String type, Object data );

    void SendCommand( String type, Object data, AsyncCallback<Object> callback );

    void RemoveCommandListener<T>( CommandReceived<Command<T>> callback );

    //----------------------------------

    void AddUserStatusListener( UserStatusChanged callback );

    void RemoveUserStatusListeners();

    void RemoveUserStatusListener( UserStatusChanged callback );
  }
}
