using System;

using Foundation;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FFImageLoading.Forms.Platform;
using GalaSoft.MvvmLight.Ioc;
using Acquaint.Abstractions;
using Acquaint.Common.iOS;
using Acquaint.Data;
using Acquaint.Models;
using Acquaint.Util;

namespace Acquaint.XForms.NetStandard.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register( "AppDelegate" )]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching( UIApplication app, NSDictionary options )
    {
      RegisterDependencies();

      Settings.OnDataPartitionPhraseChanged += ( sender, e ) =>
      {
        UpdateDataSourceIfNecessary();
      };

      global::Xamarin.Forms.Forms.Init();
      FormsMaps.Init();
      CachedImageRenderer.Init();
      LoadApplication( new App() );
      ConfigureTheming();
      return base.FinishedLaunching( app, options );
    }

    void ConfigureTheming()
    {
      UINavigationBar.Appearance.TintColor = UIColor.White;
      UINavigationBar.Appearance.BarTintColor = Color.FromHex( "547799" ).ToUIColor();
      UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes { ForegroundColor = UIColor.White };
      UIBarButtonItem.Appearance.SetTitleTextAttributes( new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal );
    }

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
          return _LazyBackendlessAcquaintanceSource.Value;
        } );
    }

    void UpdateDataSourceIfNecessary()
    {
      var dataSource = SimpleIoc.Default.GetInstance<IDataSource<Acquaintance>>();

      // Set the data source dependent on whether or not the data parition phrase is "UseLocalDataSource".
      // The local data source is mainly for use in TestCloud test runs, but the app can be used in local-only data mode if desired.

      // if the settings dictate that a local data source should be used, then register the local data provider and update the IoC container
      if( Settings.IsUsingLocalDataSource && !(dataSource is FilesystemOnlyAcquaintanceDataSource) )
      {
        SimpleIoc.Default.Register<IDataSource<Acquaintance>>( () =>
          {
            return _LazyFilesystemOnlyAcquaintanceDataSource.Value;
          } );
        return;
      }

      // if the settings dictate that a local data souce should not be used, then register the remote data source and update the IoC container
      if( !Settings.IsUsingLocalDataSource && !(dataSource is BackendlessDataSource) )
      {
        SimpleIoc.Default.Register<IDataSource<Acquaintance>>( () =>
        {
          return _LazyBackendlessAcquaintanceSource.Value;
        } );
      }
    }

    // we need lazy loaded instances of these two types hanging around because if the registration on IoC container changes at runtime, we want the same instances
    static Lazy<FilesystemOnlyAcquaintanceDataSource> _LazyFilesystemOnlyAcquaintanceDataSource = new Lazy<FilesystemOnlyAcquaintanceDataSource>( () => new FilesystemOnlyAcquaintanceDataSource() );
    static Lazy<BackendlessDataSource> _LazyBackendlessAcquaintanceSource = new Lazy<BackendlessDataSource>( () => new BackendlessDataSource() );

  }
}
