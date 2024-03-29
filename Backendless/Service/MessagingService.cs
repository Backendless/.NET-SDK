using System;
using System.Collections.Generic;
#if MOBILE
using BackendlessAPI.Push;
#endif
#if WINDOWS_PHONE8
using Windows.Phone.System.Analytics;
#endif
using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using Weborb.Types;
using BackendlessAPI.Messaging;
#if WITHRT
using BackendlessAPI.RT.Messaging;
#endif
#if !( NET_35 || NET_40 )
using System.Threading.Tasks;
#endif
namespace BackendlessAPI.Service
{
  public class MessagingService
  {
    private static string MESSAGING_MANAGER_SERVER_ALIAS = "com.backendless.services.messaging.MessagingService";

    internal static string DEVICE_REGISTRATION_MANAGER_SERVER_ALIAS = "com.backendless.services.messaging.DeviceRegistrationService";
    private static string EMAIL_MANAGER_SERVER_ALIAS = "com.backendless.services.mail.CustomersEmailService";
    private static string EMAIL_TEMPLATE_SENDER_SERVER_ALIAS = "com.backendless.services.mail.EmailTemplateSender";
    private static string DEFAULT_CHANNEL_NAME = "default";
    private static string deviceId;
    private static int CHANNEL_NAME_MAX_LENGTH = 46;
#if UNITY
    private static Messaging.DeviceRegistration _deviceRegistrationDto;
    private static AsyncCallback<string> _deviceRegisterCallback = null;
    private static AsyncCallback<bool> _deviceUnregisterCallback = null;
    public delegate void UnityRegisterDevice( string GCMSenderID, List<String> channels, DateTime? expiration );
    public delegate void UnityUnregisterDevice();
    private UnityRegisterDevice _unityRegisterDevice;
    private UnityUnregisterDevice _unityUnregisterDevice;
  #endif

    public MessagingService()
    {
#if MOBILE
      Types.AddClientClassMapping( "com.backendless.management.DeviceRegistrationDto",
                                   typeof( Messaging.DeviceRegistration ) );
#endif
      Types.AddClientClassMapping( "com.backendless.services.messaging.MessageStatus",
                                   typeof( Messaging.MessageStatus ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.PublishOptions",
                                   typeof( Messaging.PublishOptions ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.DeliveryOptions",
                                   typeof( Messaging.DeliveryOptions ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.PublishStatusEnum",
                                   typeof( Messaging.PublishStatusEnum ) );
      Types.AddClientClassMapping( "com.backendless.services.messaging.Message", typeof( Messaging.Message ) );
      deviceId = Guid.NewGuid().ToString();
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

#if MOBILE
    public void UnregisterDevice()
    {
      Registrar.UnregisterDevice();
    }

    public async Task UnregisterDeviceAsync()
    {
      await Task.Run( () => { Registrar.UnregisterDevice(); } );
    }

    public async Task UnregisterDeviceAsync( List<String> channels )
    {
      await Task.Run( () => { Registrar.UnregisterDevice( channels ); } );
    }

    public void UnregisterDevice( List<String> channels )
    {
      Registrar.UnregisterDevice( channels );
    }

    public void RegisterDevice( String token )
    {
      RegisterDevice( token, DEFAULT_CHANNEL_NAME );
    }


    public void RegisterDevice( String token, String channel )
    {
      RegisterDevice( token, new List<String> { channel } );
    }
    

    public void RegisterDevice( String token, List<String> channels )
    {
      RegisterDevice( token, channels, null );
    }

    public void RegisterDevice( String token, List<String> channels, DateTime? expiration )
    {
      RegisterDeviceLogic( token, channels, expiration );
    }

#if !( NET_35 || NET_40 )
    public async Task RegisterDeviceAsync( String token )
    {
      await Task.Run( () => { RegisterDevice( token, DEFAULT_CHANNEL_NAME ); } );
    }

    public async Task RegisterDeviceAsync( String token, String channel )
    {
      await Task.Run( () => { RegisterDevice( token, new List<String> { channel } ); } );
    }

    public async Task RegisterDeviceAsync( String token, List<String> channels )
    {
      await Task.Run( () => { RegisterDevice( token, channels, null ); } );
    }

    public async Task RegisterDeviceAsync( String token, List<String> channels, DateTime? expiration)
    {
      await Task.Run( () => { RegisterDeviceLogic( token, channels, expiration ); } );
    }
#endif

    private void RegisterDeviceLogic( String token, List<String> channels, DateTime? expiration )
    {
      if( channels == null || channels.Count == 0 || ( channels.Count == 1 && String.IsNullOrEmpty(channels[0]) ))
        channels = new List<String> { DEFAULT_CHANNEL_NAME };

      foreach( String channel in channels )
        checkChannelName( channel );

      Registrar.RegisterDevice( token, channels, expiration ); 
    }
#endif

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

    public void RegisterDevice(string channel, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding =
 null,
                                AsyncCallback<string> callback = null )
    {
      if( string.IsNullOrEmpty( channel ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );

      RegisterDevice( new List<string> {channel}, pushNotificationsBinding, callback );
    }

    public void RegisterDevice(List<string> channels, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding
 = null,
                                AsyncCallback<string> callback = null )
    {
      RegisterDevice( channels, null, pushNotificationsBinding, callback );
    }

    public void RegisterDevice(DateTime expiration, BackendlessAPI.Push.PushNotificationsBinding pushNotificationsBinding
 = null,
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
    public string DeviceID
    {
      get { return deviceId; }
    }
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

#if !( NET_35 || NET_40 )
    public async Task<MessageStatus> PublishAsync( object message )
    {
      return await PublishAsync( message, DEFAULT_CHANNEL_NAME );
    }

    public async Task<MessageStatus> PublishAsync( object message, PublishOptions publishOptions )
    {
      return await PublishAsync( message, DEFAULT_CHANNEL_NAME, publishOptions );
    }

    public async Task<MessageStatus> PublishAsync( object message, DeliveryOptions deliveryOptions )
    {
      return await PublishAsync( message, DEFAULT_CHANNEL_NAME, null, deliveryOptions );
    }

    public async Task<MessageStatus> PublishAsync( object message,
                                                   PublishOptions publishOptions,
                                                   DeliveryOptions deliveryOptions,
                                                   AsyncCallback<MessageStatus> callback )
    {
      return await PublishAsync( message, DEFAULT_CHANNEL_NAME, publishOptions, deliveryOptions );
    }
#endif
    public void Publish( object message, AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, callback );
    }

    public void Publish( object message, Messaging.PublishOptions publishOptions,
                         AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, publishOptions, callback );
    }

    public void Publish( object message, Messaging.DeliveryOptions deliveryOptions,
                         AsyncCallback<MessageStatus> callback )
    {
      Publish( message, DEFAULT_CHANNEL_NAME, null, deliveryOptions, callback );
    }

    public void Publish( object message, Messaging.PublishOptions publishOptions,
                         Messaging.DeliveryOptions deliveryOptions,
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

    public Messaging.MessageStatus Publish( object message, string channelName,
                                            Messaging.PublishOptions publishOptions )
    {
      return PublishSync( message, channelName, publishOptions, null );
    }

    public Messaging.MessageStatus Publish( object message, string channelName,
                                            Messaging.DeliveryOptions deliveryOptions )
    {
      return PublishSync( message, channelName, null, deliveryOptions );
    }

    public Messaging.MessageStatus Publish( object message, string channelName, Messaging.PublishOptions publishOptions,
                                            Messaging.DeliveryOptions deliveryOptions )
    {
      return PublishSync( message, channelName, publishOptions, deliveryOptions );
    }

    private Messaging.MessageStatus PublishSync( object message, string channelName,
                                                 Messaging.PublishOptions publishOptions,
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

#if !( NET_35 || NET_40 )
    public async Task<MessageStatus> PublishAsync( object message, string channelName )
    {
      return await PublishAsync( message, channelName, null, null );
    }

    public async Task<MessageStatus> PublishAsync( object message, string channelName, PublishOptions publishOptions )
    {
      return await PublishAsync( message, channelName, publishOptions, null );
    }

    public async Task<MessageStatus> PublishAsync( object message, string channelName, DeliveryOptions deliveryOptions )
    {
      return await PublishAsync( message, channelName, null, deliveryOptions );
    }

    public async Task<MessageStatus> PublishAsync( object message, string channelName, PublishOptions publishOptions,
                                                   DeliveryOptions deliveryOptions )
    {
      return await Task.Run( () => Publish( message, channelName, publishOptions, deliveryOptions ) )
                       .ConfigureAwait( false );
    }
#endif

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

#region MESSAGE STATUS

#if !( NET_35 || NET_40 )
    public async Task<MessageStatus> GetMessageStatusAsync( string messageId )
    {
      return await Task.Run( () => GetMessageStatus( messageId ) ).ConfigureAwait( false );
    }

#endif
    public MessageStatus GetMessageStatus( string messageId )
    {
      if( messageId == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE_ID );
      MessageStatus messageStatus =
        Invoker.InvokeSync<MessageStatus>( MESSAGING_MANAGER_SERVER_ALIAS, "getMessageStatus",
                                           new object[] { messageId } );

      return messageStatus;
    }

    public void GetMessageStatus( string messageId, AsyncCallback<MessageStatus> callback )
    {
      if( messageId == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_MESSAGE_ID );

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "getMessageStatus", new object[] { messageId }, callback );
    }

#endregion

#region CANCEL MESSAGE

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

#if !( NET_35 || NET_40 )
    public async Task<bool> CancelAsync( string messageId )
    {
      return await Task.Run( () => Cancel( messageId ) ).ConfigureAwait( false );
    }
#endif

#endregion

#region SUBSCRIBE

#if WITHRT
    public IChannel Subscribe()
    {
      return Subscribe( DEFAULT_CHANNEL_NAME );
    }

    public IChannel Subscribe( string channelName )
    {
      return new ChannelImpl( channelName );
    }
#endif

#endregion

#region PUSHTEMPLATE

    public MessageStatus PushWithTemplate( String templateName )
    {
      return PushWithTemplate( templateName, (Dictionary<String, Object>) null );
    }

    public MessageStatus PushWithTemplate( String templateName, Dictionary<String, Object> templateValues )
    {
      if( String.IsNullOrEmpty( templateName ) )
        throw new ArgumentException( ExceptionMessage.NULL_EMPTY_TEMPLATE_NAME );

      return Invoker.InvokeSync<MessageStatus>( MESSAGING_MANAGER_SERVER_ALIAS, "pushWithTemplate", 
                                                                                new Object[] { templateName, templateValues } );
    }

    public void PushWithTemplate( String templateName, AsyncCallback<MessageStatus> callback )
    {
      PushWithTemplate( templateName, null, callback );
    }

    public void PushWithTemplate( String templateName, Dictionary<String, Object> templateValues, AsyncCallback<MessageStatus> callback )
    {
      if( String.IsNullOrEmpty( templateName ) )
        throw new ArgumentException( ExceptionMessage.NULL_EMPTY_TEMPLATE_NAME );

      Invoker.InvokeAsync( MESSAGING_MANAGER_SERVER_ALIAS, "pushWithTemplate", new Object[] { templateName, templateValues }, callback );
    }

#if !( NET_35 || NET_40 )
    public async Task<MessageStatus> PushWithTemplateAsync( String templateName )
    {
      return await Task.Run( () => PushWithTemplate( templateName ) ).ConfigureAwait( false );
    }

    public async Task<MessageStatus> PushWithTemplateAsync( String templateName, Dictionary<String, Object> templateValues )
    {
      return await Task.Run( () => PushWithTemplate( templateName, templateValues ) ).ConfigureAwait( false );
    }
#endif

#endregion

#region SEND EMAIL FROM TEMPLATE
#if !( NET_35 || NET_40 )
    public async Task<MessageStatus> SendEmailFromTemplateAsync( string templateName, EmailEnvelope envelope )
    {
      return await Task.Run(() => SendEmailFromTemplate(templateName, envelope)).ConfigureAwait(false);
    }

    public async Task<MessageStatus> SendEmailFromTemplateAsync( string templateName, EmailEnvelope envelope, Dictionary<string, string> templateValues )
    {
      return await Task.Run( () => SendEmailFromTemplate( templateName, envelope, templateValues ) ).ConfigureAwait( false );
    }

    public async Task<MessageStatus> SendEmailFromTemplateAsync(String templateName, EmailEnvelope envelope, List<String> attachments)
    {
      return await Task.Run(() => SendEmailFromTemplate(templateName, envelope, attachments)).ConfigureAwait(false);
    }

    public async Task<MessageStatus> SendEmailFromTemplateAsync(String templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues, List<String> attachments)
    {
      return await Task.Run(() => SendEmailFromTemplate(templateName, envelope, templateValues, attachments)).ConfigureAwait(false);
    }

#endif
    public MessageStatus SendEmailFromTemplate( String templateName, EmailEnvelope envelope )
    {
      return SendEmailFromTemplateInternal( templateName, envelope, null, null );
    }
    public MessageStatus SendEmailFromTemplate( String templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues )
    {

      return SendEmailFromTemplateInternal(templateName, envelope, templateValues, null);
    }

    public MessageStatus SendEmailFromTemplate(String templateName, EmailEnvelope envelope, List<String> attachments)
    {
      return SendEmailFromTemplateInternal(templateName, envelope, null, attachments);
    }

    public MessageStatus SendEmailFromTemplate(string templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues, List<String> attachments)
    {
      return SendEmailFromTemplateInternal(templateName, envelope, templateValues, attachments);
    }

    private MessageStatus SendEmailFromTemplateInternal(string templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues, List<String> attachments)
    {
      if (string.IsNullOrEmpty(templateName))
        throw new ArgumentNullException(ExceptionMessage.NULL_EMPTY_TEMPLATE_NAME);

      if (envelope == null)
        throw new ArgumentException(ExceptionMessage.NULL_EMAIL_ENVELOPE);

      return Invoker.InvokeSync<MessageStatus>(EMAIL_TEMPLATE_SENDER_SERVER_ALIAS, "sendEmails", new Object[] { templateName, envelope, templateValues, attachments});
    }

    public void SendEmailFromTemplate( string templateName, EmailEnvelope envelope, AsyncCallback<MessageStatus> responder )
    {
      SendEmailFromTemplateInternal( templateName, envelope, null, null, responder );
    }

    public void SendEmailFromTemplate( string templateName, EmailEnvelope envelope, Dictionary<string, string> templateValues, AsyncCallback<MessageStatus> responder )
    {
      SendEmailFromTemplateInternal(templateName, envelope, templateValues, null, responder);
    }

    public void SendEmailFromTemplate(String templateName, EmailEnvelope envelope, List<String> attachments, AsyncCallback<MessageStatus> responder)
    {
      SendEmailFromTemplateInternal(templateName, envelope, null, attachments, responder);
    }

    public void SendEmailFromTemplate(string templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues, List<String> attachments, AsyncCallback<MessageStatus> responder)
    {
      SendEmailFromTemplateInternal(templateName, envelope, templateValues, attachments, responder);
    }

    private void SendEmailFromTemplateInternal(string templateName, EmailEnvelope envelope, Dictionary<String, String> templateValues, List<String> attachments, AsyncCallback<MessageStatus> responder)
    {
      if (string.IsNullOrEmpty(templateName))
        throw new ArgumentNullException(ExceptionMessage.NULL_EMPTY_TEMPLATE_NAME);

      if (envelope == null)
        throw new ArgumentException(ExceptionMessage.NULL_EMAIL_ENVELOPE);

      Invoker.InvokeAsync(EMAIL_TEMPLATE_SENDER_SERVER_ALIAS, "sendEmails", new Object[] { templateName, envelope, templateValues, attachments }, responder);
    }
    #endregion
    #region SEND EMAIL

#if !(NET_35 || NET_40)
    public async Task<MessageStatus> SendTextEmailAsync( string subject, string messageBody, List<string> recipients )
    {
      return await SendEmailAsync( subject, new BodyParts( messageBody, null ), recipients, new List<string>() );
    }

    public async Task<MessageStatus> SendTextEmailAsync( string subject, string messageBody, string recipient )
    {
      return await SendEmailAsync( subject, new BodyParts( messageBody, null ), new List<string>() { recipient },
                            new List<string>() );
    }

    public async Task<MessageStatus> SendHTMLEmailAsync( string subject, string messageBody, List<string> recipients )
    {
      return await SendEmailAsync( subject, new BodyParts( null, messageBody ), recipients, new List<string>() );
    }

    public async Task<MessageStatus> SendHTMLEmailAsync( string subject, string messageBody, string recipient )
    {
      return await SendEmailAsync( subject, new BodyParts( null, messageBody ), new List<string>() { recipient },
                            new List<string>() );
    }

    public async Task<MessageStatus> SendEmailAsync( string subject, BodyParts bodyParts, string recipient,
                                                     List<string> attachments )
    {
      return await SendEmailAsync( subject, bodyParts, new List<string>() { recipient }, attachments );
    }

    public async Task<MessageStatus> SendEmailAsync( string subject, BodyParts bodyParts, string recipient )
    {
      return await SendEmailAsync( subject, bodyParts, new List<string>() { recipient }, new List<string>() );
    }

    public async Task<MessageStatus> SendEmailAsync( string subject, BodyParts bodyParts, List<string> recipients,
                                                     List<string> attachments )
    {
      return await Task.Run( () => SendEmail( subject, bodyParts, recipients, attachments ) ).ConfigureAwait( false );
    }
#endif

    public MessageStatus SendTextEmail( string subject, string messageBody, List<string> recipients )
    {
      return SendEmail( subject, new BodyParts( messageBody, null ), recipients, new List<string>() );
    }

    public MessageStatus SendTextEmail( string subject, string messageBody, string recipient )
    {
      return SendEmail( subject, new BodyParts( messageBody, null ), new List<string>() { recipient },
                        new List<string>() );
    }

    public MessageStatus SendHTMLEmail( string subject, string messageBody, List<string> recipients )
    {
      return SendEmail( subject, new BodyParts( null, messageBody ), recipients, new List<string>() );
    }

    public MessageStatus SendHTMLEmail( string subject, string messageBody, string recipient )
    {
      return SendEmail( subject, new BodyParts( null, messageBody ), new List<string>() { recipient },
                        new List<string>() );
    }

    public MessageStatus SendEmail( string subject, BodyParts bodyParts, string recipient, List<string> attachments )
    {
      return SendEmail( subject, bodyParts, new List<string>() { recipient }, attachments );
    }

    public MessageStatus SendEmail( string subject, BodyParts bodyParts, string recipient )
    {
      return SendEmail( subject, bodyParts, new List<string>() { recipient }, new List<string>() );
    }

    public MessageStatus SendEmail( string subject, BodyParts bodyParts, List<string> recipients,
                                    List<string> attachments )
    {
      if( subject == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBJECT );

      if( bodyParts == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_BODYPARTS );

      if( recipients == null || recipients.Count == 0 )
        throw new ArgumentNullException( ExceptionMessage.NULL_RECIPIENTS );

      if( attachments == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ATTACHMENTS );

      return Invoker.InvokeSync<MessageStatus>( EMAIL_MANAGER_SERVER_ALIAS, "send",
                                                new Object[] { subject, bodyParts, recipients, attachments } );
    }

    public void SendTextEmail( string subject, string messageBody, List<string> recipients,
                               AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), recipients, new List<string>(), responder );
    }

    public void SendTextEmail( string subject, string messageBody, string recipient,
                               AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, new BodyParts( messageBody, null ), new List<string>() { recipient }, new List<string>(),
                 responder );
    }

    public void SendHTMLEmail( string subject, string messageBody, List<string> recipients,
                               AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), recipients, new List<string>(), responder );
    }

    public void SendHTMLEmail( string subject, string messageBody, string recipient,
                               AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, new BodyParts( null, messageBody ), new List<string>() { recipient }, new List<string>(),
                 responder );
    }

    public void SendEmail( string subject, BodyParts bodyParts, string recipient, List<string> attachments,
                           AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, bodyParts, new List<string>() { recipient }, attachments, responder );
    }

    public void SendEmail( string subject, BodyParts bodyParts, string recipient,
                           AsyncCallback<MessageStatus> responder )
    {
      SendEmail( subject, bodyParts, new List<string>() { recipient }, new List<string>(), responder );
    }

    public void SendEmail( string subject, BodyParts bodyParts, List<string> recipients, List<string> attachments,
                           AsyncCallback<MessageStatus> responder )
    {
      if( subject == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_SUBJECT );

      if( bodyParts == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_BODYPARTS );

      if( recipients == null || recipients.Count == 0 )
        throw new ArgumentNullException( ExceptionMessage.NULL_RECIPIENTS );

      if( attachments == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_ATTACHMENTS );

      Invoker.InvokeAsync( EMAIL_MANAGER_SERVER_ALIAS, "send",
                           new Object[] { subject, bodyParts, recipients, attachments }, responder );
    }

#endregion

    private void checkChannelName( String channelName )
    {
      if( String.IsNullOrEmpty( channelName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CHANNEL_NAME );

      if( channelName.Length > CHANNEL_NAME_MAX_LENGTH )
        throw new ArgumentException( ExceptionMessage.CHANNEL_NAME_TOO_LONG );
    }
  }
}