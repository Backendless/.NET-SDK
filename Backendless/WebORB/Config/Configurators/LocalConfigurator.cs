using System;
using System.Collections;
using System.IO;
using Weborb.Util;
using Weborb.Util.Config;

namespace Weborb.Config.Configurators
{
  class LocalConfigurator : WebORBConfigurator
  {
    private FileSystemWatcher flexConfigWatcher;
    private FileSystemWatcher weborbConfigWatcher;
    private FileSystemEventHandler flexEventHandler;
    private FileSystemEventHandler weborbEventHandler;
    
    internal static void EnableFlexWatcher()
    {
      LocalConfigurator configurator = DeploymentMode.GetConfigurator() as LocalConfigurator;
      if ( configurator == null )
        return;

      if ( configurator.flexConfigWatcher != null )
          configurator.flexConfigWatcher.Changed += configurator.flexEventHandler;
    }

    internal static void DisableFlexWatcher()
    {
      LocalConfigurator configurator = DeploymentMode.GetConfigurator() as LocalConfigurator;
      if ( configurator == null )
        return;

      if ( configurator.flexConfigWatcher != null )
        configurator.flexConfigWatcher.Changed -= configurator.flexEventHandler;
    }

    public FileSystemWatcher FlexConfigWatcher
    {
      get { return flexConfigWatcher; }
      set { flexConfigWatcher = value; }
    }

    public FileSystemWatcher WeborbConfigWatcher
    {
      get { return weborbConfigWatcher; }
      set { weborbConfigWatcher = value; }
    }

    protected override IDictionary ConfigureHandler( ORBConfig config, ArrayList sectionsToConfig )
    {
      System.Console.Out.WriteLine( "loading config from " + config.GetConfigFilePath() );
      //ThreadContext.setConfig( this );
      return ConfigEngine.Configure( config, config.GetConfigFilePath(), sectionsToConfig );
    }


    public override void EnableConfigHotDeploy( ORBConfig config )
    {
      if( Directory.Exists( config.GetFlexConfigPath() ) )
      {
        if( flexConfigWatcher != null )
        {
          flexConfigWatcher.Changed -= flexEventHandler;
          flexConfigWatcher.EnableRaisingEvents = false;
        }

        flexConfigWatcher = new FileSystemWatcher( config.GetFlexConfigPath() );
        flexConfigWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
        flexEventHandler = new FileSystemEventHandler( OnChanged );
        flexConfigWatcher.Changed += flexEventHandler;
        flexConfigWatcher.EnableRaisingEvents = true;
      }

      if( weborbConfigWatcher != null )
      {
        weborbConfigWatcher.Changed -= weborbEventHandler;
        weborbConfigWatcher.EnableRaisingEvents = false;
      }

      weborbConfigWatcher = new FileSystemWatcher( Paths.GetWebORBPath(), "weborb.config" );
      weborbConfigWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
      weborbEventHandler = new FileSystemEventHandler( OnChanged );
      weborbConfigWatcher.Changed += weborbEventHandler;
      weborbConfigWatcher.EnableRaisingEvents = true;
    }

    public void OnChanged( object source, FileSystemEventArgs evt )
    {
      ORBConfig.reset();
    }


    public override void DisableConfigHotDeploy()
    {
      if( flexConfigWatcher != null )
      {
        flexConfigWatcher.Changed -= flexEventHandler;
        flexConfigWatcher.EnableRaisingEvents = false;
        flexConfigWatcher.Dispose();
      }

      if( weborbConfigWatcher != null )
      {
        weborbConfigWatcher.Changed -= weborbEventHandler;
        weborbConfigWatcher.EnableRaisingEvents = false;
        weborbConfigWatcher.Dispose();
      }
    }
  }
}
