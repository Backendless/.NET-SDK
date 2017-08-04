using System;
using System.Collections.Generic;
using System.Globalization;
#if WINDOWS_PHONE8
using Windows.Phone.System.Analytics;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using Weborb.Types;
using BackendlessAPI.Messaging;
using Subscription = BackendlessAPI.Messaging.Subscription;

namespace BackendlessAPI.Service
{
  public class MessagingService
  {
    private static string MESSAGING_MANAGER_SERVER_ALIAS = "com.backendless.services.messaging.MessagingService";
    private static string EMAIL_MANAGER_SERVER_ALIAS = "com.backendless.services.mail.CustomersEmailService";
    private static string DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS = "com.backendless.services.messaging.DeviceRegistrationService";

    private static string DEFAULT_CHANNEL_NAME = "default";
    
    private static int CHANNEL_NAME_MAX_LENGTH = 46;
    
    private static Messaging.DeviceRegistration _deviceRegistrationDto;

#if UNITY
    private static AsyncCallback<string> _deviceRegisterCallback = null;
    private static AsyncCallback<bool> _deviceUnregisterCallback = null;
    public delegate void UnityRegisterDevice( string GCMSenderID, List<String> channels, DateTime? expiration );
    public delegate void UnityUnregisterDevice();
    private UnityRegisterDevice _unityRegisterDevice;
    private UnityUnregisterDevice _unityUnregisterDevice;
#endif

    public MessagingService()
    {
      Types.AddClientClassMapping( "com.backendless.management.DeviceRegistrationDto", typeof( Messaging.DeviceRegistration ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.MessageStatus", typeof( Messaging.MessageStatus ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.PublishOptions", typeof( Messaging.PublishOptions ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.DeliveryOptions", typeof( Messaging.DeliveryOptions ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.PublishStatusEnum", typeof( Messaging.PublishStatusEnum ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.Message", typeof( Messaging.Message ) );

#if WINDOWS_PHONE8
      object deviceId;
      if (!Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out deviceId))
      {
          deviceId = HostInformation.PublisherHostId;

          if( deviceId == null )
            throw new BackendlessException(new BackendlessFault(ExceptionMessage.NO_DEVICEID_CAPABILITY));
      }

      _deviceRegistrationDto = new DeviceRegistration
        {
          Os = "WP",
          DeviceId = BitConverter.ToString( (byte[]) deviceId ).Replace( "-", "" ),
          OsVersion = System.Environment.OSVersion.Version.Major.ToString( CultureInfo.InvariantCulture )
        };
#elif UNITY
      _deviceRegistrationDto = new DeviceRegistration();
#endif
    }

#if UNITY
    public void SetUnityRegisterDevice( UnityRegisterDevice unityRegisterDevice, UnityUnregisterDevice unityUnregisterDevice )
    {
      _unityRegisterDevice = unityRegisterDevice;
      _unityUnregisterDevice = unityUnregisterDevice;
    }
    public void RegisterDevice( string GCMSenderID )
    {
      RegisterDevice( GCMSenderID, (AsyncCallback<string>) null );
    }

    public void RegisterDevice( string GCMSenderID, AsyncCallback<string> callback )
    {
      RegisterDevice( GCMSenderID, DEFAULT_CHANNEL_NAME, callback );
    }

    public void RegisterDevice( string GCMSenderID, string channel )
    {
      RegisterDevice( GCMSenderID, channel, (AsyncCallback<string>) null );
    }

    public void RegisterDevice( string GCMSenderID, string channel, AsyncCallback<string> callback )
    {
      if( string.IsNullOrEmpty( channel ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );

      RegisterDevice( GCMSenderID, new List<string> { channel }, callback );
    }

    public void RegisterDevice( string GCMSenderID, List<string> channels )
    {
      RegisterDevice( GCMSenderID, channels, (AsyncCallback<string>) null );
    }

    public void RegisterDevice( string GCMSenderID, List<string> channels, AsyncCallback<string> callback )
    {
      RegisterDevice( GCMSenderID, channels, null, callback );
    }

    public void RegisterDevice( string GCMSenderID, DateTime expiration )
    {
      RegisterDevice( GCMSenderID, expiration, (AsyncCallback<string>) null );
    }

    public void RegisterDevice( string GCMSenderID, DateTime expiration, AsyncCallback<string> callback )
    {
      RegisterDevice( GCMSenderID, null, expiration, callback );
    }

    public void RegisterDevice( string GCMSenderID, List<string> channels, DateTime? expiration )
    {
      RegisterDevice( GCMSenderID, channels, expiration, (AsyncCallback<string>) null );
    }

    public void RegisterDevice( string GCMSenderID, List<string> channels, DateTime? expiration, AsyncCallback<string> callback )
    {
      if( channels == null )
        channels = new List<string>();

      if( channels.Count == 0 )
       channels.Add( DEFAULT_CHANNEL_NAME );

      foreach ( String channel in channels ) {
		if ( channel.Length > CHANNEL_NAME_MAX_LENGTH )
			throw new ArgumentException( ExceptionMessage.CHANNEL_NAME_TOO_LONG );
			
        _deviceRegistrationDto.AddChannel(channel);
      }

      _deviceRegisterCallback = callback;
		if (_unityRegisterDevice != null)
			_unityRegisterDevice(GCMSenderID, channels, expiration);
		else
			RegisterDeviceOnServer(); // Skip Unity
    }

    public void RegisterDeviceOnServer()
    {
      AsyncCallback<string> callback = _deviceRegisterCallback;

      var responder = new AsyncCallback<string>( r =>
      {
        _deviceRegistrationDto.RegistrationId = r;
        callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );

      Invoker.InvokeAsync( DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "registerDevice",
                           new object[] { _deviceRegistrationDto }, responder );
    }

    public void UnregisterDevice( AsyncCallback<bool> callback )
    {
      _deviceUnregisterCallback = callback;
		if (_unityUnregisterDevice != null)
			_unityUnregisterDevice();
		else
			UnregisterDeviceOnServer(); // Skip Unity
    }

    public void UnregisterDeviceOnServer()
    {
      AsyncCallback<bool> callback = _deviceUnregisterCallback;

      var responder = new AsyncCallback<bool>( r =>
      {
        if( callback != null )
        {
          _deviceRegistrationDto.ClearRegistration();
          callback.ResponseHandler.Invoke( r );
        }
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );

      Invoker.InvokeAsync( DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "unregisterDevice",
                           new Object[] { _deviceRegistrationDto.DeviceId },
                           responder );
    }

    public DeviceRegistration DeviceRegistration
    {
      get { return _deviceRegistrationDto; }
    }
#endif

#if WINDOWS_PHONE || WINDOWS_PHONE8
    public DeviceRegistration DeviceRegistration
    {
      get { return _deviceRegistrationDto; }
    }

    public void RegisterDevice( BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding = null,
                                AsyncCallback<string> callback = null )
    {
      RegisterDevice( DEFAULT_CHANNEL_NAME, pushNotificationsBinding, callback );
    }

    public void RegisterDevice(string channel, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding = null,
                                AsyncCallback<string> callback = null )
    {
      if( string.IsNullOrEmpty( channel ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );

      RegisterDevice( new List<string> {channel}, pushNotificationsBinding, callback );
    }

    public void RegisterDevice(List<string> channels, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding = null,
                                AsyncCallback<string> callback = null )
    {
      RegisterDevice( channels, null, pushNotificationsBinding, callback );
    }

    public void RegisterDevice(DateTime expiration, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding = null,
                                AsyncCallback<string> callback = null )
    {
      RegisterDevice( null, expiration, pushNotificationsBinding, callback );
    }

    public void RegisterDevice( List<string> channels, DateTime? expiration,
                                BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding = null,
                                AsyncCallback<string> callback = null )
    {
      if( channels == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );

      if( channels.Count == 0 )
        _deviceRegistrationDto.AddChannel( DEFAULT_CHANNEL_NAME );

      foreach( string channel in channels )
      {
        checkChannelName( channel );
        _deviceRegistrationDto.AddChannel( channel );
      }

      if( expiration != null )
        _deviceRegistrationDto.Expiration = expiration;

      BackendlessAPI.Push.Registrar.RegisterDevice( _deviceRegistrationDto.DeviceId, pushNotificationsBinding, callback );
    }

    internal void RegisterDeviceOnServer( string deviceToken, AsyncCallback<string> callback )
    {
      _deviceRegistrationDto.DeviceToken = deviceToken;

      var responder = new AsyncCallback<string>( r =>
        {
          _deviceRegistrationDto.RegistrationId = r;
          callback.ResponseHandler.Invoke( r );
        }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );

      Invoker.InvokeAsync( DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "registerDevice",
                           new object[] {Backendless.AppId, Backendless.VersionNum, _deviceRegistrationDto}, responder );
    }

    public void GetRegistrations( AsyncCallback<List<DeviceRegistration>> callback )
    {
      Invoker.InvokeAsync( DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "getDeviceRegistrationByDeviceId",
                           new object[] {Backendless.AppId, Backendless.VersionNum, _deviceRegistrationDto.DeviceId},
                           callback );
    }

    public void UnregisterDevice( AsyncCallback<bool> callback )
    {
      if( _deviceRegistrationDto.RegistrationId == null )
        return;

      BackendlessAPI.Push.Registrar.UnregisterDevice( callback );
    }

    internal void UnregisterDeviceOnServer( AsyncCallback<bool> callback )
    {
      var responder = new AsyncCallback<bool>( r =>
        {
          if( callback != null )
          {
            _deviceRegistrationDto.ClearRegistration();
            callback.ResponseHandler.Invoke( r );
          }
        }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );

      Invoker.InvokeAsync( DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS, "unregisterDevice",
                           new Object[] {Backendless.AppId, Backendless.VersionNum, _deviceRegistrationDto.DeviceId},
                           responder );
    }


#endif

    #region PUBLISH SYNC (DEFAULT CHANNEL)

    public Messaging.MessageStatus Publish( object message )
    {
      return Publish( message, DEFAULT_CHANNEL_NAME );
    }

    public Messaging.MessageStatus Publish( object message, Messaging.PublishOptions publishOptions )
    {
      return Publish( message, DEFAULT_CHANNEL_NAME, publishOptions );
    }

    public Messaging.MessageStatus Publish( object message, Messaging.DeliveryOptions deliveryOptions )
    {
      return Publish( message, DEFAULT_CHANNEL_NAME, null, deliveryOptions );
    }

    public Messaging.MessageStatus Publish( object message, Messaging.PublishOptions publishOptions,
                                     Messaging.DeliveryOptions deliveryOptions )
    {
      return Publish( message, DEFAULT_CHANNEL_NAME, publishOptions, deliveryOptions );
    }

    #endregion

    #region PUBLISH ASYNC (DEFAULT CHANNEL)

    public void Publish( object message, AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, callback );
    }

    public void Publish( object message, Messaging.PublishOptions publishOptions, AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, publishOptions, callback );
    }

    public void Publish( object message, Messaging.DeliveryOptions deliveryOptions, AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, null, deliveryOptions, callback );
    }

    public void Publish( object message, Messaging.PublishOptions publishOptions, Messaging.DeliveryOptions deliveryOptions,
                         AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, publishOptions, deliveryOptions, callback );
    }

    #endregion

    #region PUBLISH SYNC

    public Messaging.MessageStatus Publish( object message, string channelName )
    {
      return PublishSync( message, channelName, null, null );
    }

    public Messaging.MessageStatus Publish( object message, string channelName, Messaging.PublishOptions publishOptions )
    {
      return PublishSync( message, channelName, publishOptions, null );
    }

    public Messaging.MessageStatus Publish( object message, string channelName, Messaging.DeliveryOptions deliveryOptions )
    {
      return PublishSync( message, channelName, null, deliveryOptions );
    }

    public Messaging.MessageStatus Publish( object message, string channelName, Messaging.PublishOptions publishOptions,
                                     Messaging.DeliveryOptions deliveryOptions )
    {
      return PublishSync( message, channelName, publishOptions, deliveryOptions );
    }

    private Messaging.MessageStatus PublishSync( object message, string channelName, Messaging.PublishOptions publishOptions,
                                          Messaging.DeliveryOptions deliveryOptions )
    {
      checkChannelName( channelName );

      if( message == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE );

      return Invoker.InvokeSync<Messaging.MessageStatus>( MESSAGING_MANAGER_SERVER_ALIAS, "publish",
                                                   new[]
                                                     { channelName, message, publishOptions, deliveryOptions } );
    }

    #endregion

    #region PUBLISH ASYNC

    public void Publish( object message, string channelName, AsyncCallback<MessageStatus> callback )
    {
      Publish( message, channelName, null, null, callback );
    }

    public void Publish( object message, string channelName, Messaging.PublishOptions publishOptions,
                         AsyncCallback<MessageStatus> callback )
    {
      Publish( message, channelName, publishOptions, null, callback );
    }

    public void Publish( object message, string channelName, Messaging.DeliveryOptions deliveryOptions,
                         AsyncCallback<MessageStatus> callback )
    {
      Publish( message, channelName, null, deliveryOptions, callback );
    }

    public void Publish( object message, string channelName, Messaging.PublishOptions publishOptions,
                         Messaging.DeliveryOptions deliveryOptions, AsyncCallback<MessageStatus> callback )
    {
      checkChannelName( channelName );

      if( message == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE );

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "publish",
                           new[]
                             { channelName, message, publishOptions, deliveryOptions }, callback );
    }

    #endregion

    public bool Cancel( string messageId )
    {
      if( string.IsNullOrEmpty( messageId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE_ID );

      return Invoker.InvokeSync<bool>( MESSAGING_MANAGER_SERVER_ALIAS, "cancel",
                                       new Object[] { messageId } );
    }

    public void Cancel( string messageId, AsyncCallback<bool> callback )
    {
      if( string.IsNullOrEmpty( messageId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE_ID );

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "cancel",
                           new Object[] { messageId }, callback );
    }

    #region SUBSCRIBE SYNC (DEFAULT CHANNEL)

    public Messaging.Subscription Subscribe( AsyncCallback<List<Message>> callback )
    {
      return Subscribe( DEFAULT_CHANNEL_NAME, callback );
    }

    public Messaging.Subscription Subscribe( int pollingInterval, AsyncCallback<List<Message>> callback )
    {
      return Subscribe( DEFAULT_CHANNEL_NAME, pollingInterval, callback );
    }

    public Messaging.Subscription Subscribe( AsyncCallback<List<Message>> callback,
                                      Messaging.SubscriptionOptions subscriptionOptions )
    {
      return Subscribe( DEFAULT_CHANNEL_NAME, callback, subscriptionOptions );
    }

    public Messaging.Subscription Subscribe( int pollingInterval, AsyncCallback<List<Message>> callback,
                                      Messaging.SubscriptionOptions subscriptionOptions )
    {
      return Subscribe( DEFAULT_CHANNEL_NAME, pollingInterval, callback, subscriptionOptions );
    }

    #endregion

    #region SUBSCRIBE SYNC

    public Messaging.Subscription Subscribe( string channelName, AsyncCallback<List<Message>> callback )
    {
      return Subscribe( channelName, 0, callback, new Messaging.SubscriptionOptions() );
    }

    public Messaging.Subscription Subscribe( string channelName, int pollingInterval, AsyncCallback<List<Message>> callback )
    {
      return Subscribe( channelName, pollingInterval, callback, new Messaging.SubscriptionOptions() );
    }

    public Messaging.Subscription Subscribe( string channelName, AsyncCallback<List<Message>> callback,
                                      Messaging.SubscriptionOptions subscriptionOptions )
    {
      return Subscribe( channelName, 0, callback, subscriptionOptions );
    }

    public Messaging.Subscription Subscribe( string channelName, int pollingInterval, AsyncCallback<List<Message>> callback,
                                      Messaging.SubscriptionOptions subscriptionOptions )
    {
      checkChannelName( channelName );

      if( pollingInterval < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_POLLING_INTERVAL );

      string subscriptionId = subscribeForPollingAccess( channelName, subscriptionOptions );
      Messaging.Subscription subscription = new Messaging.Subscription();

      subscription.ChannelName = channelName;
      subscription.SubscriptionId = subscriptionId;

      if( pollingInterval != 0 )
        subscription.PollingInterval = pollingInterval;

      subscription.OnSubscribe( callback );

      return subscription;
    }

    #endregion

    #region SUBSCRIBE ASYNC (DEFAULT CHANNEL)

    public void Subscribe( int pollingInterval, AsyncCallback<List<Message>> callback,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( DEFAULT_CHANNEL_NAME, pollingInterval, callback, null, subscriptionCallback );
    }

    public void Subscribe( AsyncCallback<List<Message>> callback, AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( DEFAULT_CHANNEL_NAME, 0, callback, null, subscriptionCallback );
    }

    public void Subscribe( AsyncCallback<List<Message>> callback, Messaging.SubscriptionOptions subscriptionOptions,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( DEFAULT_CHANNEL_NAME, 0, callback, subscriptionOptions, subscriptionCallback );
    }

    public void Subscribe( int pollingInterval, AsyncCallback<List<Message>> callback,
                           Messaging.SubscriptionOptions subscriptionOptions,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( DEFAULT_CHANNEL_NAME, pollingInterval, callback, subscriptionOptions, subscriptionCallback );
    }

    #endregion

    #region SUBSCRIBE ASYNC

    public void Subscribe( string channelName, AsyncCallback<List<Message>> callback,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( channelName, 0, callback, null, subscriptionCallback );
    }

    public void Subscribe( string channelName, int pollingInterval, AsyncCallback<List<Message>> callback,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( channelName, pollingInterval, callback, null, subscriptionCallback );
    }

    public void Subscribe( string channelName, AsyncCallback<List<Message>> callback,
                           Messaging.SubscriptionOptions subscriptionOptions,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      Subscribe( channelName, 0, callback, subscriptionOptions, subscriptionCallback );
    }

    public void Subscribe( string channelName, int pollingInterval, AsyncCallback<List<Message>> callback,
                           Messaging.SubscriptionOptions subscriptionOptions,
                           AsyncCallback<Subscription> subscriptionCallback )
    {
      checkChannelName( channelName );
      if( pollingInterval < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_POLLING_INTERVAL );

      var responder = new AsyncCallback<string>( r =>
        {
          Messaging.Subscription subscription = new Messaging.Subscription();
          subscription.ChannelName = channelName;
          subscription.SubscriptionId = r;

          if( pollingInterval != 0 )
            subscription.PollingInterval = pollingInterval;

          subscription.OnSubscribe( callback );

          if( subscriptionCallback != null )
            subscriptionCallback.ResponseHandler.Invoke( subscription );
        }, f =>
          {
            if( subscriptionCallback != null )
              subscriptionCallback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );
      subscribeForPollingAccess( channelName, subscriptionOptions, responder );
    }

    #endregion

    public List<Message> PollMessages( string channelName, string subscriptionId )
    {
      checkChannelName( channelName );

      if( string.IsNullOrEmpty( subscriptionId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBSCRIPTION_ID );

      return Invoker.InvokeSync<List<Message>>( MESSAGING_MANAGER_SERVER_ALIAS, "pollMessages",
                                                   new object[] { channelName, subscriptionId } );
    }

    public void PollMessages( string channelName, string subscriptionId, AsyncCallback<List<Message>> callback )
    {
      checkChannelName( channelName );

      if( string.IsNullOrEmpty( subscriptionId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBSCRIPTION_ID );

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "pollMessages",
                           new object[] { channelName, subscriptionId },
                           callback );
    }

    #region SEND EMAIL
    public void SendTextEmail( String subject, String messageBody, List<String> recipients )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), recipients, new List<String>() );
    }

    public void SendTextEmail( String subject, String messageBody, String recipient )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), new List<String>() { recipient }, new List<String>() );
    }

    public void SendHTMLEmail( String subject, String messageBody, List<String> recipients )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), recipients, new List<String>() );
    }

    public void SendHTMLEmail( String subject, String messageBody, String recipient )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), new List<String>() { recipient }, new List<String>() );
    }

    public void SendEmail( String subject, BodyParts bodyParts, String recipient, List<String> attachments )
    {
      SendEmail( subject, bodyParts, new List<String>() { recipient }, attachments );
    }

    public void SendEmail( String subject, BodyParts bodyParts, String recipient )
    {
      SendEmail( subject, bodyParts, new List<String>() { recipient }, new List<String>() );
    }

    public void SendEmail( String subject, BodyParts bodyParts, List<String> recipients, List<String> attachments )
    {
      if( subject == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBJECT );

      if( bodyParts == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_BODYPARTS );

      if( recipients == null || recipients.Count == 0 )
        throw new ArgumentNullException( ExceptionMessage.NULL_RECIPIENTS );

      if( attachments == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ATTACHMENTS );

      Invoker.InvokeSync<object>( EMAIL_MANAGER_SERVER_ALIAS, "send", new Object[] { subject, bodyParts, recipients, attachments } );
    }

    public void SendTextEmail( String subject, String messageBody, List<String> recipients, AsyncCallback<object> responder )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), recipients, new List<String>(), responder );
    }

    public void SendTextEmail( String subject, String messageBody, String recipient, AsyncCallback<object> responder )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), new List<String>() { recipient }, new List<String>(), responder );
    }

    public void SendHTMLEmail( String subject, String messageBody, List<String> recipients, AsyncCallback<object> responder )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), recipients, new List<String>(), responder );
    }

    public void SendHTMLEmail( String subject, String messageBody, String recipient, AsyncCallback<object> responder )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), new List<String>() { recipient }, new List<String>(), responder );
    }

    public void SendEmail( String subject, BodyParts bodyParts, String recipient, List<String> attachments, AsyncCallback<object> responder )
    {
      SendEmail( subject, bodyParts, new List<String>() { recipient }, attachments, responder );
    }

    public void SendEmail( String subject, BodyParts bodyParts, String recipient, AsyncCallback<object> responder )
    {
      SendEmail( subject, bodyParts, new List<String>() { recipient }, new List<String>(), responder );
    }

    public void SendEmail( String subject, BodyParts bodyParts, List<String> recipients, List<String> attachments,
                           AsyncCallback<object> responder )
    {
      if( subject == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBJECT );

      if( bodyParts == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_BODYPARTS );

      if( recipients == null || recipients.Count == 0 )
        throw new ArgumentNullException( ExceptionMessage.NULL_RECIPIENTS );

      if( attachments == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ATTACHMENTS );

      Invoker.InvokeAsync( EMAIL_MANAGER_SERVER_ALIAS, "send", new Object[] { subject, bodyParts, recipients, attachments }, responder );
    }
    #endregion

    private string subscribeForPollingAccess( string channelName, Messaging.SubscriptionOptions subscriptionOptions )
    {
      checkChannelName( channelName );

      if( subscriptionOptions == null )
        subscriptionOptions = new Messaging.SubscriptionOptions();

      return Invoker.InvokeSync<string>( MESSAGING_MANAGER_SERVER_ALIAS, "subscribeForPollingAccess",
                                         new Object[] { channelName, subscriptionOptions } );
    }

    private void subscribeForPollingAccess( string channelName, Messaging.SubscriptionOptions subscriptionOptions,
                                            AsyncCallback<string> callback )
    {
      checkChannelName( channelName );

      if( subscriptionOptions == null )
        subscriptionOptions = new Messaging.SubscriptionOptions();

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "subscribeForPollingAccess",
                           new Object[] { channelName, subscriptionOptions },
                           callback );
    }

    private void checkChannelName( string channelName )
    {
      if( string.IsNullOrEmpty( channelName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );
    }
  }
}