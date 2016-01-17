#if( CLOUD )
using System;
using System.Collections;
using Microsoft.WindowsAzure.ServiceRuntime;

using Weborb.Cloud;
using Weborb.Util.Config;
using Weborb.Util.Logging;

namespace Weborb.Config.Configurators
{
  class AzureConfigurator : WebORBConfigurator
  {
    private const string SUBSCRIPTION_ID = "SubscriberID";
    private const string EMAIL_ADDRESS = "EmailAddress";
    private AzureBlobWatcher azureBlobWatcher;
    
    public AzureBlobWatcher AzureBlobWatcher
    {
      get { return azureBlobWatcher; }
      set { azureBlobWatcher = value; }
    }

    protected override IDictionary ConfigureHandler( ORBConfig config, ArrayList sectionsToConfig )
    {
      IExternalConfig extConfig = new AzureExternalConfig();
      //extConfig.FlushConfig();
      IDictionary configObjects = extConfig.Configure( config, sectionsToConfig );

      if( configObjects.Count < 1 )
      {
        System.Console.Out.WriteLine( "loading config from " + config.GetConfigFilePath() );
        //ThreadContext.setConfig( this );
        configObjects = ConfigEngine.Configure( config, config.GetConfigFilePath(), sectionsToConfig );
      }

      int subscriptionId;
      String emailAddress;

      try
      {
        subscriptionId = Int32.Parse( RoleEnvironment.GetConfigurationSettingValue( SUBSCRIPTION_ID ) );
        emailAddress = RoleEnvironment.GetConfigurationSettingValue( EMAIL_ADDRESS );
        CloudBillingClient.GetInstance().Initialize( subscriptionId, emailAddress );
      }
      catch( Exception e )
      {
        if( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, "Azure SubscriberID or EmailAddress is incorrectly specified", e );
      }

      return configObjects;
    }


    public override void EnableConfigHotDeploy( ORBConfig config )
    {
      //hot deploy takes to much time for azure
      return;
      azureBlobWatcher = new AzureBlobWatcher();
      azureBlobWatcher.Blobs.Add( "weborb.config" );
      //List<string> files = new List<string>();
      //foreach ( string file in Directory.EnumerateFiles( GetFlexConfigPath() ) )
      //  {
      //  files.Add( "WEB-INF\\flex\\" + Path.GetFileName( file ) );
      //  }
      azureBlobWatcher.Blobs.AddRange( AzureUtil.WebInfConfigs );
      azureBlobWatcher.SaveLastUpdateTime();
      config.GetConfigInstanceListener += azureBlobWatcher.Watch;
    }


    public override void DisableConfigHotDeploy()
    {
      azureBlobWatcher = null;
    }
  }
}
#endif