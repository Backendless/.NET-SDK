using System;
using System.Reflection;
using System.Net;
using System.Threading;
using Weborb.Config.Configurators;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config
{
  class DeploymentMode
  {
    private static WebORBConfigurator configurator = null;

    private static bool? isCloud = null;
    private static bool? isAmazon = null;
    private static bool? isAzure = null;

    public static WebORBConfigurator GetConfigurator()
    {
      if( configurator != null )
        return configurator;
#if( CLOUD )
      if( IsAzure() )
        configurator = new AzureConfigurator();
      else
#endif      
      //if( IsAmazon() )
      //  configurator = new AmazonConfigurator();
      configurator = new LocalConfigurator();

      runAmazonCheck();
      return configurator;
    }

    public static bool IsCloud()
    {
      if( isCloud == null )
       isCloud = IsAzure() || IsAmazon();

      return (bool) isCloud;
    }

    public static bool IsAzure()
    {
      if( isAzure != null )
        return (bool) isAzure;

      isAzure = false;

      try
      {
        Type roleEnvironmentType = TypeLoader.LoadType( "Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment" );
        PropertyInfo isAvailable = roleEnvironmentType.GetProperty( "IsAvailable" );
        isAzure = (bool) isAvailable.GetValue( null, null );
      }
      catch
      {
      }

      if ( (bool)isAzure )
        isAmazon = false;

      return (bool) isAzure;
    }

    private static void runAmazonCheck()
    {
      ThreadPool.QueueUserWorkItem(
        state =>
        {
          isAmazon = getResponseCode( "http://169.254.169.254" ) != 404;

          if(Log.isLogging(LoggingConstants.INFO))
            Log.log(LoggingConstants.INFO, String.Format("Server is {0} deployed on Amazon EC2", ((bool)isAmazon ? "" : "not")));

          if( (bool) isAmazon )
          {
            AmazonConfigurator notCofiguredConfigurator = new AmazonConfigurator();
            notCofiguredConfigurator.FlexConfigWatcher = ( (LocalConfigurator) configurator ).FlexConfigWatcher;
            notCofiguredConfigurator.WeborbConfigWatcher = ( (LocalConfigurator) configurator ).WeborbConfigWatcher;

            // make sure config has been fully initialized. GetInstance does the job..
            configurator = notCofiguredConfigurator;
            isAzure = false;
            isCloud = true;
            ORBConfig.reset();
          }
        } );
    }

    public static bool IsAmazon()
    {
      if ( isAmazon != null )
        return (bool)isAmazon;
      
      return false;
    }

    private static int getResponseCode( String urlString )
    {
      try
      {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create( new Uri( urlString ) );
        request.Timeout = 60000;
        request.Method = "HEAD";
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        return (int) response.StatusCode;
      }
      catch
      {
        return 404;
      }
    }
  }
}
