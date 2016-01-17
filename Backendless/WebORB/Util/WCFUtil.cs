#if NET_30
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Weborb.Util
{
  class WCFUtil
  {
    internal static ServiceElementCollection GetServices()
    {
      //string siteVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
      string siteVirtualPath = AppDomain.CurrentDomain.BaseDirectory;
      Configuration config = null;
      try
      {
        config = WebConfigurationManager.OpenWebConfiguration( siteVirtualPath );
      }
      catch( Exception )
      {
        try
        {
          config = WebConfigurationManager.OpenWebConfiguration( "~" );
        }
        catch( Exception )
        {
          if( Logging.Log.isLogging( Logging.LoggingConstants.ERROR ) )
            Logging.Log.log( Logging.LoggingConstants.ERROR, "Unable to load web.config for WCF services inspection. If you run WebORB using ASP.NET Development Server, make sure to start Visual Studio 'as Administrator'" );
          
          return null;
        }
      }

      Weborb.Util.Logging.Log.log( Logging.LoggingConstants.INFO, "CONFIG PATH - " + config.FilePath );

      ServiceModelSectionGroup group = ServiceModelSectionGroup.GetSectionGroup( config );
      return group.Services.Services;
    }
  }
}
#endif