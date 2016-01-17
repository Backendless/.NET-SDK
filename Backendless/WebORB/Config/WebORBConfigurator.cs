using System;
using System.Collections;

namespace Weborb.Config
{
  internal abstract class WebORBConfigurator
  {
    public delegate void ConfiguredHandler();

    public static event ConfiguredHandler Configured;

    private static void InvokeConfigured()
    {
      if ( Configured != null ) 
        Configured();
    }

    public IDictionary Configure( ORBConfig config, ArrayList sectionsToConfig )
    {
      IDictionary configObjects = ConfigureHandler(config, sectionsToConfig);
      InvokeConfigured();
      return configObjects;
    }

    protected abstract IDictionary ConfigureHandler( ORBConfig config, ArrayList sectionsToConfig );
    public abstract void EnableConfigHotDeploy( ORBConfig config );
    public abstract void DisableConfigHotDeploy();
  }
}
