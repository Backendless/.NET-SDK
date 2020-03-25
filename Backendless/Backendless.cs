using System;
using System.Collections;
using System.Collections.Generic;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Service;
using Weborb.Config;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.V3Types;
using Weborb.Writer;
using BackendlessAPI.IO;
using BackendlessAPI.Utils;
using BackendlessAPI.Caching;
using BackendlessAPI.Counters;
using BackendlessAPI.Logging;
using BackendlessAPI.Geo;

#if WITHRT
using BackendlessAPI.RT;
#endif
  
namespace BackendlessAPI
{
  public static class Backendless
  {
    public static long BACKENDLESSLOG = Weborb.Util.Logging.Log.getCode( "BACKENDLESS LOG" );
    public static string URL = "https://api.backendless.com";

    public static PersistenceService Persistence;
    public static PersistenceService Data;
    public static GeoService Geo;
    public static MessagingService Messaging;
    public static FileService Files;
    public static UserService UserService;
    public static Events Events;
    public static Cache Cache;
    public static CounterService Counters;
    public static LoggingService Logging;
    public static CustomService CustomService;
    #if WITHRT
    public static IRTService RT;
    #endif
    public static string AppId { get; private set; }

    public static string APIKey { get; private set; }

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

    public static void InitApp( string applicationId, string apiKey )
    {
      if( string.IsNullOrEmpty( applicationId ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_APPLICATION_ID );

      if( string.IsNullOrEmpty( apiKey ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_SECRET_KEY );

      Log.addLogger( Log.DEFAULTLOGGER, new ConsoleLogger() );
      Log.startLogging( BACKENDLESSLOG );
      #if WITHRT
      Quobject.EngineIoClientDotNet.Modules.LogManager.Enabled = true;
      #endif
      AppId = applicationId;
      APIKey = apiKey;

      Persistence = new PersistenceService();
      Data = Persistence;
      Geo = new GeoService();
      Messaging = new MessagingService();
      Files = new FileService();
      UserService = new UserService();
      Events = Events.GetInstance();
      Cache = Cache.GetInstance();
      Counters = CounterService.GetInstance();
      Logging = new LoggingService();
      CustomService = new CustomService();
      
      #if WITHRT
      RT = new RTServiceImpl();
      #endif
  
      MessageWriter.DefaultWriter = new UnderflowWriter();
      MessageWriter.AddAdditionalTypeWriter( typeof( BackendlessUser ), new BackendlessUserWriter() );
      MessageWriter.AddAdditionalTypeWriter( typeof( Geometry ), new BackendlessGeometryWriter() );
      MessageWriter.AddAdditionalTypeWriter( typeof( Point ), new BackendlessGeometryWriter() );
      MessageWriter.AddAdditionalTypeWriter( typeof( LineString ), new BackendlessGeometryWriter() );
      MessageWriter.AddAdditionalTypeWriter( typeof( Polygon ), new BackendlessGeometryWriter() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( BackendlessUser ).FullName, new BackendlessUserFactory() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( GeometryDTO ).FullName, new BackendlessGeometryFactory() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( Geometry ).FullName, new BackendlessGeometryFactory() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( Point ).FullName, new BackendlessGeometryFactory() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( LineString ).FullName, new BackendlessGeometryFactory() );
      ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( typeof( Polygon ).FullName, new BackendlessGeometryFactory() );
 
      HeadersManager.CleanHeaders();
      LoginStorage loginStorage = new LoginStorage();

      if( loginStorage.HasData )
        HeadersManager.GetInstance().AddHeader( HeadersEnum.USER_TOKEN_KEY, loginStorage.UserToken );
    }

    public static int Timeout
    {
      get
      {
        return BackendlessAPI.Engine.Invoker.Timeout;
      }

      set
      {
        BackendlessAPI.Engine.Invoker.Timeout = value;
      }
    }
  }
}