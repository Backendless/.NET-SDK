using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Acquaint.Abstractions;
using Acquaint.Common.Droid;
using Acquaint.Data;
using Acquaint.Models;
using Acquaint.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FFImageLoading.Forms.Platform;
using Autofac;
using GalaSoft.MvvmLight.Ioc;

namespace Acquaint.XForms.NetStandard.Droid
{
  [Activity( Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation )]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    // an IoC Container
    IContainer _IoCContainer;

    protected override void OnCreate( Bundle bundle )
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.toolbar;
      RegisterDependencies();

      Settings.OnDataPartitionPhraseChanged += ( sender, e ) =>
      {
        UpdateDataSourceIfNecessary();
      };


      CachedImageRenderer.Init( true );

      // this line is essential to wiring up the toolbar styles defined in ~/Resources/layout/toolbar.axml
      FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
      base.OnCreate( bundle );

      global::Xamarin.Forms.Forms.Init( this, bundle );
      // register HockeyApp as the crash reporter
      //CrashManager.Register( this, Settings.HockeyAppId );

      Forms.Init( this, bundle );

      Xamarin.FormsMaps.Init( this, bundle );
      LoadApplication( new App() );
    }


    /// <summary>
    /// Registers dependencies with an IoC container.
    /// </summary>
    /// <remarks>
    /// Since some of our libraries are shared between the Forms and Native versions 
    /// of this app, we're using an IoC/DI framework to provide access across implementations.
    /// </remarks>
    void RegisterDependencies()
    {
      SimpleIoc.Default.Register<IEnvironmentService, EnvironmentService>();
      SimpleIoc.Default.Register<IHttpClientHandlerFactory, HttpClientHandlerFactory>();
      SimpleIoc.Default.Register<IDatastoreFolderPathProvider, DatastoreFolderPathProvider>();

      if( Settings.IsUsingLocalDataSource )
        SimpleIoc.Default.Register<IDataSource<Acquaintance>>( () =>
        {
          return _LazyFilesystemOnlyAcquaintanceDataSource.Value;
        } );
      else
        SimpleIoc.Default.Register<IDataSource<Acquaintance>>( () =>
        {
          return _LazyAzureAcquaintanceSource.Value;
        } );




      var builder = new ContainerBuilder();

      builder.RegisterInstance( new EnvironmentService() ).As<IEnvironmentService>();

      builder.RegisterInstance( new HttpClientHandlerFactory() ).As<IHttpClientHandlerFactory>();

      builder.RegisterInstance( new DatastoreFolderPathProvider() ).As<IDatastoreFolderPathProvider>();

      // Set the data source dependent on whether or not the data parition phrase is "UseLocalDataSource".
      // The local data source is mainly for use in TestCloud test runs, but the app can be used in local-only data mode if desired.
      if( Settings.IsUsingLocalDataSource )
        builder.RegisterInstance( _LazyFilesystemOnlyAcquaintanceDataSource.Value ).As<IDataSource<Acquaintance>>();
      else
        builder.RegisterInstance( _LazyAzureAcquaintanceSource.Value ).As<IDataSource<Acquaintance>>();

      _IoCContainer = builder.Build();

      //var csl = new AutofacServiceLocator( _IoCContainer );
      //ServiceLocator.SetLocatorProvider( () => csl );
    }

    /// <summary>
    /// Updates the data source if necessary.
    /// </summary>
    void UpdateDataSourceIfNecessary()
    {
      var dataSource = SimpleIoc.Default.GetInstance<IDataSource<Acquaintance>>();

      // Set the data source dependent on whether or not the data parition phrase is "UseLocalDataSource".
      // The local data source is mainly for use in TestCloud test runs, but the app can be used in local-only data mode if desired.

      // if the settings dictate that a local data source should be used, then register the local data provider and update the IoC container
      if( Settings.IsUsingLocalDataSource && !(dataSource is FilesystemOnlyAcquaintanceDataSource) )
      {
        var builder = new ContainerBuilder();
        builder.RegisterInstance( _LazyFilesystemOnlyAcquaintanceDataSource.Value ).As<IDataSource<Acquaintance>>();
        builder.Update( _IoCContainer );
        return;
      }

      // if the settings dictate that a local data souce should not be used, then register the remote data source and update the IoC container
      if( !Settings.IsUsingLocalDataSource && !(dataSource is BackendlessDataSource) )
      {
        var builder = new ContainerBuilder();
        builder.RegisterInstance( _LazyAzureAcquaintanceSource.Value ).As<IDataSource<Acquaintance>>();
        builder.Update( _IoCContainer );
      }
    }

    // we need lazy loaded instances of these two types hanging around because if the registration on IoC container changes at runtime, we want the same instances
    static Lazy<FilesystemOnlyAcquaintanceDataSource> _LazyFilesystemOnlyAcquaintanceDataSource = new Lazy<FilesystemOnlyAcquaintanceDataSource>( () => new FilesystemOnlyAcquaintanceDataSource() );
    static Lazy<BackendlessDataSource> _LazyAzureAcquaintanceSource = new Lazy<BackendlessDataSource>( () => new BackendlessDataSource() );
  }
}
