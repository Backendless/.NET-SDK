using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Config
{
    public sealed class ConfigConstants
    {
        public static string PERSISTENTWEBSERVICESSTORE = "persistentWebServicesStore";

        public static string LOG = "log";
        public static string ENABLE = "enable";
        public static string YES = "yes";
        public static string LOGTOFILE = "logToFile";

        //String WEBORB_CONFIG_XML = "weborb-config.xml";
        public static string WEBORB_CONFIG_XML = "weborb-config.xml";
        public static string WEBORB_DEFAULT_CONFIG_XML = "weborb-default-config.xml";
        public static string WEB_INF_CLASSES = "/WEB-INF/classes/";

        public static string SERVICEBROWSER = "serviceBrowser";
        public static string SERVICEINSPECTOR = "serviceInspector";
        public static string CALLTRACE = "callTrace";
        public static string ENABLECALLTRACE = "enable";
        public static string QUEUEFLUSHTHRESHOLD = "queueFlushThreshold";
        public static string QUEUECHECKWAITTIME = "queueCheckWaitTime";
        public static string CALLSPERFILE = "maxCallsPerCallStoreFile";
        public static string BUFFERSIZE = "callStoreBufferSize";
        public static string SERVICEINVOKER = "serviceInvoker";
        public static string ABSTRACTCLASSMAPPING = "abstractClassMapping";
        public static string ABSTRACTCLASSMAPPINGS = "abstractClassMappings";
        public static string CLASSNAME = "className";
        public static string MAPPEDCLASSNAME = "mappedClassName";
        public static string CUSTOMWRITER = "customWriter";
        public static string CUSTOMWRITERS = "customWriters";
        public static string WRITERCLASSNAME = "writerClassName";
        public static string ACTIVATOR = "activator";
        public static string ACTIVATIONMODENAME = "activationModeName";
        public static string REACTIVATEEXPIREDSESSIONS = "reactivateExpiredSessions";
        public static string SERVICE = "service";
        public static string SERVICES = "services";
        public static string NAME = "name";
        public static string SERVICEID = "serviceId";
        public static string DATASETS = "datasets";
        public static string SERIALIZATION = "serialization";
        public static string DEFAULTPAGESIZE = "defaultPageSize";
        public static string PROTOCOLHANDLER = "protocolHandler";
        public static string CLASSMAPPING = "classMapping";
        public static string CLASSMAPPINGS = "classMappings";
        public static string CLIENTCLASS = "clientClass";
        public static string SERVERCLASS = "serverClass";
        public static string BUSINESSINTELLIGENCE = "businessIntelligence";
        public static string MONITOREDSERVICE = "monitoredService";
        public static string MONITOREDSERVICES = "monitoredServices";
        public static string RBISERVERCONFIGURATION = "serverConfiguration";
        public static string READINGFORMATDEFINITIONS = "readingFormatDefinitions";
        public static string READINGFORMATDEFINITION = "readingFormatDefinition";
        public static string READINGINDICATORS = "readingIndicators";
        public static string READINGINDICATOR = "readingIndicator";
        public static string DATACOLLECTIONAGENT = "dataCollectionAgent";

        public const string SECTION_DEPLOYMENT = "weborb/deployment";

        public const string CLUSTER = "cluster";
        public const string NODE_ADDRESS = "nodeAddress";
    }
}
