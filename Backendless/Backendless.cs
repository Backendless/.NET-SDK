using System;
using System.Collections;
using System.Collections.Generic;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Service;
using Weborb.Config;
using Weborb.Types;
using Weborb.Util;
using Weborb.V3Types;

namespace BackendlessAPI
{
  public static class Backendless
  {
    public static string URL = "https://api.backendless.com";
    
    public static PersistenceService Persistence;
    public static GeoService Geo;
    public static MessagingService Messaging;
    public static FileService Files;
    public static UserService UserService;

    public static string AppId { get; private set; }

    public static string SecretKey { get; private set; }

    public static string VersionNum { get; private set; }

    static Backendless() 
    {
      Types.AddAbstractTypeMapping( typeof( IList<> ), typeof( List<> ) );
      Types.AddClientClassMapping( "flex.messaging.messages.AcknowledgeMessage", typeof( AckMessage ) );
      Types.AddClientClassMapping( "flex.messaging.messages.AsyncMessage", typeof( AsyncMessage ) );
      Types.AddClientClassMapping( "flex.messaging.messages.RemotingMessage", typeof( ReqMessage ) );
      Types.AddClientClassMapping( "flex.messaging.messages.CommandMessage", typeof( CommandMessage ) );
      Types.AddClientClassMapping( "flex.messaging.messages.ErrorMessage", typeof( ErrMessage ) );
      Types.AddClientClassMapping( "flex.messaging.io.ArrayCollection", typeof( ObjectProxy ) );
      ORBConfig.GetInstance()
               .getObjectFactories()
               .AddArgumentObjectFactory( "Weborb.V3Types.BodyHolder", new BodyHolderFactory() );
      Types.AddAbstractTypeMapping( typeof( IDictionary ), typeof( Dictionary<object, object> ) );
    }

    public static void InitApp( string applicationId, string secretKey, string version )
    {
      if( string.IsNullOrEmpty( applicationId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_APPLICATION_ID );

      if( string.IsNullOrEmpty( secretKey ) )
        throw new ArgumentNullException(ExceptionMessage.NULL_SECRET_KEY);

      if( string.IsNullOrEmpty( version ) )
        throw new ArgumentNullException(ExceptionMessage.NULL_VERSION);

      AppId = applicationId;
      SecretKey = secretKey;
      VersionNum = version;

      Persistence = new PersistenceService();
      Geo = new GeoService();
      Messaging = new MessagingService();
      Files = new FileService();
      UserService = new UserService();

      HeadersManager.CleanHeaders();
    }
  }
}