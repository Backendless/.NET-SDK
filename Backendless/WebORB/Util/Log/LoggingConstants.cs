using System;

namespace Weborb.Util.Logging
{
  public sealed class LoggingConstants
  {
    public const string WEBORB_INFO = "WEBORB INFO";
    public const string WEBORB_DEBUG = "WEBORB DEBUG";
    public const string WEBORB_ERROR = "WEBORB ERROR";
    public const string WEBORB_SERIALIZATION = "WEBORB SERIALIZATION";
    public const string WEBORB_EXCEPTION = "WEBORB EXCEPTION";
    public const string WEBORB_MESSAGINGEXCEPTION = "WEBORB MESSAGING EXCEPTION";
    public const string WEBORB_INSTRUMENTATION = "WEBORB INSTRUMENTATION";
    public const string WEBORB_SECURITY = "WEBORB SECURITY";
    public const string WEBORB_MESSAGE_SERVER = "WEBORB MESSAGE SERVER";
    public const string WEBORB_THREADING = "WEBORB THREADING";
    public const string ZIP_DEBUG_STRING = "ZIP DEBUG";
    public const string WEBORB_RDS = "WEBORB RDS";
    public const string WEBORB_RTMP_IO = "WEBORB RTMP IO";

    public const string WEBORB_CLUSTER = "WEBORB CLUSTER";

    public static long INFO = Log.getCode( WEBORB_INFO );
    public static long DEBUG = Log.getCode( WEBORB_DEBUG );
    public static long ERROR = Log.getCode( WEBORB_ERROR );
    public static long SERIALIZATION = Log.getCode( WEBORB_SERIALIZATION );
    public static long EXCEPTION = Log.getCode( WEBORB_EXCEPTION );
    public static long MESSAGINGEXCEPTION = Log.getCode( WEBORB_MESSAGINGEXCEPTION );
    public static long INSTRUMENTATION = Log.getCode( WEBORB_INSTRUMENTATION );
    public static long SECURITY = Log.getCode( WEBORB_SECURITY );
    public static long MESSAGE_SERVER = Log.getCode( WEBORB_MESSAGE_SERVER );
    public static long THREADING = Log.getCode( WEBORB_THREADING );
    public static long ZIP_DEBUG = Log.getCode( ZIP_DEBUG_STRING );
    public static long RDS = Log.getCode( WEBORB_RDS );
    public static long RTMP_IO = Log.getCode( WEBORB_RTMP_IO );
    
    public static long CLUSTER = Log.getCode( WEBORB_CLUSTER );
  }
}
