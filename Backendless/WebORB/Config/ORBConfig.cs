using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;

#if (FULL_BUILD)
using System.Web;
using System.Xml.XPath;
using Weborb.Activation;
using Weborb.Cloud;
using Weborb.Handler;
using Weborb.Data;
using Weborb.Management.CodeGen;
using Weborb.Security;
using Weborb.V3Types.Core;
using Weborb.Util.Config;
#endif

#if CLOUD
using Weborb.Cloud;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
#endif

using Weborb.Util;
using Weborb.Types;
using Weborb.Protocols;
using Weborb.Registry;
using Weborb.Util.Logging;
using Weborb.Writer;


namespace Weborb.Config
{
  public delegate void ConfigRetrieved( ORBConfig config );

  public class ORBConfig
  {
  //  public delegate void InitializedHandler();

  //  public static event InitializedHandler Initialized;

    public event ConfigRetrieved GetConfigInstanceListener;

    private static object configLockObject = new object();
    private static volatile object _lock = new object();

#if (FULL_BUILD)
    private IDictionary configObjects = new Hashtable();
#else
    private IDictionary configObjects = new Dictionary<string, object>();
#endif

#if (FULL_BUILD)
    private Activators activators = new Activators();
#endif
    private ObjectFactories objectFactories = new ObjectFactories();
    private Types.Types typeMapper = new Types.Types();

#if (FULL_BUILD)
    private Handlers handlers = new Handlers();
    private ProtocolRegistry protocolRegistry = new ProtocolRegistry();
#endif
    private ServiceRegistry serviceRegistry = new ServiceRegistry();
    //private bool _serializePrivateFields;
    //private bool serializeGenericCollectionsAsVector;
    private static ORBConfig instance;
    //private static bool _initialized;
    //private static bool _initializing;
    

#if (FULL_BUILD)
    private ORBSecurity security = new ORBSecurity();
    private DataServices dataServices = new DataServices();
    private BusinessIntelligenceConfig businessIntelligenceConfig = new BusinessIntelligenceConfig();
    private Dictionary<int, CodegenFormat> _codegenFormats = new Dictionary<int, CodegenFormat>();
    private Dictionary<int, CodegenFormat> _codegenMessagingFormats = new Dictionary<int, CodegenFormat>();
    private Dictionary<int, MessagingCodegenFormat> _codegenMessagingApplicationFormats = new Dictionary<int, MessagingCodegenFormat>();
    private Dictionary<int, MessagingCodegenFeature> _codegenMessagingApplicationFeatures = new Dictionary<int, MessagingCodegenFeature>();
#endif

    internal static ORBConfig GetInitializedConfig()
    {
      ORBConfig config = null;
      AutoResetEvent autoResetEvent = new AutoResetEvent( false );
      ThreadPool.QueueUserWorkItem( state =>
                                      {
                                        config = GetInstance();
                                        autoResetEvent.Set();
                                      });
      WaitHandle.WaitAll(new WaitHandle[] {autoResetEvent});
      return config;
    }

    public static ORBConfig GetInstance()
    {
      if( instance == null )
        lock ( _lock )
        {
          if ( instance == null )
            instance = new ORBConfig();
        }

      if ( instance.GetConfigInstanceListener != null )
        instance.GetConfigInstanceListener( instance );

      return instance;
    }

    public static void reset()
    {
      // to support hot deploy some clean up needs to be done
      MessageWriter.CleanAdditionalWriters();
#if( FULL_BUILD)
      instance.Initialize( null );
#endif
    }

    private ORBConfig()
    {
        instance = this;
#if( FULL_BUILD)
        Initialize( null );
#endif
#if ( PURE_CLIENT_LIB || WINDOWS_PHONE8 )
        getTypeMapper()._AddClientClassMapping( "flex.messaging.messages.AsyncMessage", typeof( Weborb.V3Types.AsyncMessage ));
        getTypeMapper()._AddClientClassMapping( "flex.messaging.messages.CommandMessage", typeof( Weborb.V3Types.CommandMessage ));
        getTypeMapper()._AddClientClassMapping( "flex.messaging.messages.RemotingMessage", typeof( Weborb.V3Types.ReqMessage ));
        getTypeMapper()._AddClientClassMapping( "flex.messaging.messages.AcknowledgeMessage", typeof( Weborb.V3Types.AckMessage ));
        getTypeMapper()._AddClientClassMapping( "flex.messaging.messages.ErrorMessage", typeof( Weborb.V3Types.ErrMessage ));
        getTypeMapper()._AddClientClassMapping( "flex.messaging.io.ObjectProxy", typeof( Weborb.Util.ObjectProxy ));
        getTypeMapper()._AddClientClassMapping( "weborb.v3types.V3Message", typeof( Weborb.V3Types.V3Message ) );

        IArgumentObjectFactory factory = (IArgumentObjectFactory) getObjectFactories()._CreateServiceObject( "Weborb.V3Types.BodyHolderFactory" );
        getObjectFactories().AddArgumentObjectFactory( "Weborb.V3Types.BodyHolder", factory );

        getTypeMapper()._AddAbstractTypeMapping( typeof( System.Collections.Generic.IList<> ), typeof( System.Collections.Generic.List<> ) );

        ITypeWriter writerObject = (ITypeWriter) getObjectFactories()._CreateServiceObject( "Weborb.V3Types.BodyHolderWriter" );
        Type mappedType = typeof( Weborb.V3Types.BodyHolder );
        MessageWriter.AddAdditionalTypeWriter( mappedType, writerObject );
#endif

#if( PURE_CLIENT_LIB )
        getTypeMapper()._AddAbstractTypeMapping( typeof( System.Collections.ICollection ), typeof( System.Collections.ArrayList ) );
        getTypeMapper()._AddAbstractTypeMapping( typeof( System.Collections.IList ), typeof( System.Collections.ArrayList ) );
        getTypeMapper()._AddAbstractTypeMapping( typeof( System.Collections.IDictionary ), typeof( System.Collections.Hashtable ) );
#endif

#if( WINDOWS_PHONE8 )
        getTypeMapper()._AddAbstractTypeMapping(typeof(System.Collections.ICollection), typeof(System.Collections.Generic.List<>));
        getTypeMapper()._AddAbstractTypeMapping(typeof(System.Collections.IList), typeof(System.Collections.Generic.List<>));
        getTypeMapper()._AddAbstractTypeMapping(typeof(System.Collections.IDictionary), typeof(System.Collections.Generic.Dictionary<,>));

#endif
    }

    public ObjectFactories getObjectFactories()
    {
      return objectFactories;
    }

    public Types.Types getTypeMapper()
    {
      return typeMapper;
    }

    public ServiceRegistry GetServiceRegistry()
    {
      return serviceRegistry;
    }

    public object GetConfig( string configName )
    {
      return configObjects[ configName ];
    }

#if (FULL_BUILD)
    public Activators getActivators()
    {
      return activators;
    }

    internal Hashtable GetConfigObjects()
    {
      if ( configObjects == null )
        configObjects = new Hashtable();
      return configObjects as Hashtable;
    }

    public Handlers getHandlers()
    {
      return handlers;
    }

    public ProtocolRegistry getProtocolRegistry()
    {
      return protocolRegistry;
    }

    public ORBSecurity getSecurity()
    {
      return security;
    }

    internal void setSecurity(ORBSecurity security)
    {
      this.security = security;
    }

    public Dictionary<int, CodegenFormat> CodegenFormats
    {
      get { return _codegenFormats; }
    }

    public Dictionary<int, CodegenFormat> CodegenMessagingFormats
    {
      get { return _codegenMessagingFormats; }
    }

    public Dictionary<int, MessagingCodegenFeature> CodegenMessagingApplicationFeatures
    {
      get { return _codegenMessagingApplicationFeatures; }
    }

    public Dictionary<int, MessagingCodegenFormat> CodegenMessagingApplicationFormats
    {
      get { return _codegenMessagingApplicationFormats; }
    }

    public DataServices GetDataServices()
    {
      return dataServices;
    }

    public string GetConfigFilePath()
    {
      return Path.Combine( Paths.GetWebORBPath(), "weborb.config" );
    }

    public string GetAlternateConfigFilePath()
    {
      return Path.Combine( (string) GetConfig( "weborb/alternateConfigPath" ), "weborb.config" );
    }

    public string GetFlexConfigPath()
    {
      return Path.Combine( Paths.GetWebORBPath(), "WEB-INF" + Path.DirectorySeparatorChar + "flex" );
    }

    private void ConfigHotDeployCheck()
    {
      bool enableHotDeploy = false;

      XmlTextReader reader = null;

      try
      {
        reader = new XmlTextReader( GetConfigFilePath() );
        XPathDocument xpathDoc = new XPathDocument( reader );
        XPathNavigator navigator = xpathDoc.CreateNavigator();
        XPathNodeIterator nodeIterator = navigator.Select( "/configuration/weborb[@hotDeploy]" );

        if( nodeIterator != null && nodeIterator.Count == 1 )
        {
          nodeIterator.MoveNext();
          String value = nodeIterator.Current.GetAttribute( "hotDeploy", "" );

          if( value.Equals( "yes" ) )
            enableHotDeploy = true;
        }
      }
      catch( Exception )
      {
      }
      finally
      {
        if( reader != null )
          reader.Close();
      }

      if( enableHotDeploy )
        DeploymentMode.GetConfigurator().EnableConfigHotDeploy( this );
    }

    private volatile Object _initializeLock = new Object();
    private void Initialize( ArrayList sectionToConfig )
    {
      lock (_initializeLock)
      {
        WebORBConfigurator configurator = DeploymentMode.GetConfigurator();
        configObjects = configurator.Configure(this, sectionToConfig);

        new FlexDynamicServiceConfig().Configure(GetFlexConfigPath(), this);
        new FlexRemotingServiceConfig().Configure(GetFlexConfigPath(), this);
        new FlexDataServiceConfig().Configure(GetFlexConfigPath(), this);
        new FlexMessagingServiceConfig().Configure(GetFlexConfigPath(), this);
        new FlexServicesConfig().Configure(GetFlexConfigPath(), this);
        new FlexWeborbServicesConfig().Configure(GetFlexConfigPath(), this);
        getHandlers().LockLicense();
        ConfigHotDeployCheck();

        if (Initialized != null)
          Initialized();
      }
    }

    public BusinessIntelligenceConfig getBusinessIntelligenceConfig()
    {
      return GetInstance().businessIntelligenceConfig;
    }

#endif
  }
}
