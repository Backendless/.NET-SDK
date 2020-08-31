using System;
using System.Collections.Generic;
using BackendlessAPI.Messaging;
using BackendlessAPI.Service;
using BackendlessAPI.Engine;

#if UNITY_ANDROID
namespace BackendlessAPI.Push
{
  internal static class Registrar
  {
    internal static String RegisterDevice( String token, List<String> channels, DateTime? expiration )
    {
      DeviceRegistration deviceRegistration = new DeviceRegistration();
      deviceRegistration.Channels = channels;
      deviceRegistration.Expiration = expiration;
      deviceRegistration.DeviceToken = token;
      deviceRegistration.DeviceId = Backendless.Messaging.DeviceID;
      deviceRegistration.Os = "ANDROID";
      deviceRegistration.Channels = channels;

      return Invoker.InvokeSync<String>( MessagingService.DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "registerDevice", new Object[] { deviceRegistration } );  
    }

    internal static Boolean UnregisterDevice()
    {
      return Invoker.InvokeSync<Boolean>( MessagingService.DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "unregisterDevice", new object[] { Backendless.Messaging.DeviceID } );
    }

    internal static Int32? UnregisterDevice( IList<String> channels )
    {
      return Invoker.InvokeSync<Int32?>( MessagingService.DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "unregisterDevice", new Object[] { Backendless.Messaging.DeviceID, channels } );
    }
  }
}
#endif
/*
private static RegistrationDecorator _currentRegistration;

private static EventHandler<NotificationChannelConnectionEventArgs> _onConnectionStatusChangedHandler;
private static EventHandler<NotificationChannelUriEventArgs> _channelUriUpdateddHandler;

internal static void RegisterDevice( String channel, PushNotificationsBinding pushNotificationsBinding,
                                     AsyncCallback<String> callback )
{
  if( String.IsNullOrEmpty( channel ) )
    throw new ArgumentNullException( "Push channel cannot be null" );

  if( _currentRegistration == null || !_currentRegistration.IsRegistered() )
    MakeInternalRegistration( channel, pushNotificationsBinding, callback );
  else
    callback.ResponseHandler.Invoke( _currentRegistration.GetRegistrationId() );
}

private static void MakeInternalRegistration( String channel, PushNotificationsBinding pushNotificationsBinding, AsyncCallback<String> callback )
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
MY COMMENT
*/

///////COMMENTED
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
//COMENTED      
/*
 * MY COMMENT
      httpNotificationChannel.ErrorOccurred +=
        ( sender, args ) => callback.ErrorHandler.Invoke( new BackendlessFault( args.Message ) );
    }

    static void channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
    {

    }

    private static void ProceedRegistration( HttpNotificationChannel httpNotificationChannel,
                                             AsyncCallback<String> callback )
    {
      Backendless.Messaging.RegisterDeviceOnServer( httpNotificationChannel.ChannelUri.ToString(),
                                                    new AsyncCallback<String>( response =>
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
*/