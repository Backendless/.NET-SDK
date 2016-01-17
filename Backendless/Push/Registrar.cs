using System;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using Microsoft.Phone.Notification;

namespace BackendlessAPI.Push
{
  internal static class Registrar
  {
    private static RegistrationDecorator _currentRegistration;

    private static EventHandler<NotificationChannelConnectionEventArgs> _onConnectionStatusChangedHandler;
    private static EventHandler<NotificationChannelUriEventArgs> _channelUriUpdateddHandler;

    internal static void RegisterDevice( string channel, PushNotificationsBinding pushNotificationsBinding,
                                         AsyncCallback<string> callback )
    {
      if( string.IsNullOrEmpty( channel ) )
        throw new ArgumentNullException( "Push channel cannot be null" );

      if( _currentRegistration == null || !_currentRegistration.IsRegistered() )
        MakeInternalRegistration( channel, pushNotificationsBinding, callback );
      else
        callback.ResponseHandler.Invoke( _currentRegistration.GetRegistrationId() );
    }

    private static void MakeInternalRegistration( string channel, PushNotificationsBinding pushNotificationsBinding, AsyncCallback<string> callback )
    {
      var httpNotificationChannel = HttpNotificationChannel.Find( channel );

      if( httpNotificationChannel == null )
      {
          httpNotificationChannel = new HttpNotificationChannel( channel, Backendless.URL );
          httpNotificationChannel.ChannelUriUpdated += ( sender, args ) => ProceedRegistration( httpNotificationChannel, callback );
          httpNotificationChannel.Open();
      }
      else
      {
          httpNotificationChannel.ChannelUriUpdated += ( sender, args ) => ProceedRegistration( httpNotificationChannel, callback );
      }

         if(pushNotificationsBinding != null)
            pushNotificationsBinding.ApplyTo( httpNotificationChannel );
                                 

/*
      if(httpNotificationChannel.ConnectionStatus.Equals( ChannelConnectionStatus.Connected ))
        ProceedRegistration( httpNotificationChannel, callback );
      else
      {
        httpNotificationChannel.ConnectionStatusChanged +=
          _onConnectionStatusChangedHandler = delegate( object sender, NotificationChannelConnectionEventArgs args )
            {
              if( args.ConnectionStatus.Equals( ChannelConnectionStatus.Connected ) )
              {
                ProceedRegistration( httpNotificationChannel, callback );
                httpNotificationChannel.ConnectionStatusChanged -= _onConnectionStatusChangedHandler;
              }
            };
        httpNotificationChannel.Open();
      }

      if(pushNotificationsBinding != null)
        pushNotificationsBinding.ApplyTo( httpNotificationChannel );
 * */
      
      httpNotificationChannel.ErrorOccurred +=
        ( sender, args ) => callback.ErrorHandler.Invoke( new BackendlessFault( args.Message ) );
    }

    static void channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
    {

    }

    private static void ProceedRegistration( HttpNotificationChannel httpNotificationChannel,
                                             AsyncCallback<string> callback )
    {
      Backendless.Messaging.RegisterDeviceOnServer( httpNotificationChannel.ChannelUri.ToString(),
                                                    new AsyncCallback<string>( response =>
                                                      {
                                                        _currentRegistration =
                                                          new RegistrationDecorator( httpNotificationChannel, response,
                                                                                     DateTime.Now.AddDays( 2 ) );
                                                        callback.ResponseHandler.Invoke( response );
                                                      }, fault => callback.ErrorHandler.Invoke( fault ) ) );
    }

    internal static void UnregisterDevice( AsyncCallback<bool> callback )
    {
      if( _currentRegistration != null &&
          _currentRegistration.GetRegistrationInfo()
                              .Channel.ConnectionStatus.Equals( ChannelConnectionStatus.Connected ) )
      {
        HttpNotificationChannel httpNotificationChannel = _currentRegistration.GetRegistrationInfo().Channel;
        httpNotificationChannel.ConnectionStatusChanged +=
          _onConnectionStatusChangedHandler = delegate( object sender, NotificationChannelConnectionEventArgs args )
            {
              if( args.ConnectionStatus.Equals( ChannelConnectionStatus.Disconnected ) )
              {
                ProceedUnregistration( callback );
                httpNotificationChannel.ConnectionStatusChanged -= _onConnectionStatusChangedHandler;
              }
            };
        _currentRegistration.GetRegistrationInfo().Channel.Close();
      }
      else
      {
        _currentRegistration = null;
        callback.ResponseHandler.Invoke(true); 
      }
    }

    private static void ProceedUnregistration( AsyncCallback<bool> callback )
    {
      Backendless.Messaging.UnregisterDeviceOnServer( new AsyncCallback<bool>( response =>
        {
          _currentRegistration = null;
          callback.ResponseHandler.Invoke( response );
        }, fault => callback.ErrorHandler.Invoke( fault ) ) );
    }
  }
}