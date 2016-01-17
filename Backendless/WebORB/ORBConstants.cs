using System;
using System.Reflection;
using Weborb.Util.Logging;

namespace Weborb
  {
  public sealed class ORBConstants
    {
    public const string DESCRIBE_SERVICE = "DescribeService";
    public const string INSPECTSERVICE = "InspectService";

    //logging config constants
    public const string NAME = "name";
    public const string CLASS = "class";
    public const string TYPE = "type";
    public const string SERVICEID = "serviceId";
    public const string LOG = "log";
    public const string ENABLE = "enable";
    public const string YES = "yes";
    public const string NO = "no";

    public const string CURRENT_POLICY = "currentPolicy";
    public const string DATE_FORMATTER = "dateFormatter";    
    public const string LOG_THREAD_NAME = "logThreadName";
    public const string LOGGING_POLICY = "loggingPolicy";
    public const string POLICY_NAME = "policyName";
    public const string CLASS_NAME = "className";
    public const string PARAMETER = "parameter";
    public const string VALUE = "value";
    public const string FILE_NAME = "fileName";

    //call trace config constants
    public const string  QUEUE_FLUSH_THRESHOLD = "queueFlushThreshold";
    public const string  QUEUE_CHECK_WAIT_TIME = "queueCheckWaitTime";
    public const string  CALL_STORE_FOLDER = "callStoreFolder";
    public const string  CALL_STORE_BUFFER_SIZE = "callStoreBufferSize";
    public const string  MAX_CALLS_PER_FILE = "maxCallsPerCallStoreFile";

    public const string TRC_ZIP = "-trc.zip";
    public const string CALLTRACE = "calltrace";
    public const string UNABLE_CREATE_CALLTRACE_FILE = "unable to create call trace file";

    public const string ONRESULT = "/onResult";
    public const string ONSTATUS = "/onStatus";
    public const string NONE_HANDLERS_INSPECT_TARGETSERVICE = "None of the handlers were able to inspect the target service. The service may not be found: ";

    //license constants
    public const string SOFTWARE = "Software";
    public const string MIDNIGHT_CODERS = "Midnight Coders";
    public const string WEBORB = "WebORB";
    public const string LICENSE_KEY = "LicenseKey";
    public const string TS_KEY = "TSKey";
    public const string AUTHORIZATION_KEY = "AuthKey";
    public const string WEBORBPATH_KEY = "WebORBPath";

    //console constants
    public const string HTTP = "http";
    public const string HTTPS = "https";

    public const string GATEWAYURL_EQUALS = "gatewayURL=";
    public const string CHAR_COLONSLASHSLASH = "://";
    public const string CHAR_COLON = ":";
    public const string ANDPERSANDHOSTADDRESS = "&hostAddress=";

    public const string REQUEST_ACTION_PARAM = "action";
    public const string CONSOLE_ACTION_SERVICE_UPLOAD = "ConsoleServiceUpload";

    // metadata constants
    public const string CLIENT_ID = "clientId";
    public const string REQUEST_HEADERS = "requestHeaders";
    public const string RESPONSE_METADATA = "responseMetadata";
    public const string CLIENT_MAPPING = "clientMapping_";
    public const string REQUEST_IP = "requestIP";    

    // service context
    public const string ACTIVATION = "activation";

    public const string SERVER = "server";
    public const string ALLOW_SUBTOPICS = "allow-subtopics";
    public const string SUBTOPIC_SEPARATOR = "subtopic-separator";
    public const string DISALLOW_WILDCARD_SUBTOPICS = "disallow-wildcard-subtopics";


    public const string MESSAGE_SERVICE_HANDLER = "message-service-handler";
    public const string MESSAGE_STORAGE_POLICY = "message-storage-policy";
    public const string MESSAGE_HANDLER = "message-factory";
    public const string MP3_REQUEST_SUFFIX = "_to_mp3";

    public const string DISCONNECT_SECONDS = "diconnect-seconds";

    public const string IsRTMPChannel = "IsRTMP";

    // this constant is duplicated on javascript side (keep it consistent)
    public const string CLASS_NAME_FIELD = "javascript_class_alias";

    // Azure
    public const string STORAGE_ACCOUNT_CONFIGURATION = "DataConnectionString";
    public const string WEBORB_AZURE_CONTAINER = "weborb";
    public const string LOCAL_STORAGE = "MessagingStorage";

    public const string CHANNEL_MY_LONG_POLLING_AMF = "my-long-polling-amf";

    public const string WEB_SOCKET_MODE = "webSocketMode";

    // dynamic destination
    public const String DYNAMIC_DESTINATION_FLAG = "dynamic-destination"; 

    //Amazon
    public const String ACCESS_KEY = "weborb.accesskey";
    public const String AWS_SECRET_KEY = "weborb.aws.secretkey";
    public const String AMAZON_WEBORB_PORT = "weborb.port";
    public const String AMAZON_CLUSTER_NAME = "weborb.amazon.cluster.name";
    public const String AMAZON_USE_PUBLICIP = "weborb.amazon.use.public.ip";

	}
}
