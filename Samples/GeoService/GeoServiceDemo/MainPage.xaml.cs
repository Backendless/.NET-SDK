using System;
using System.Device.Location;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using BackendlessAPI;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.GeoServiceDemo
{
  public partial class MainPage : PhoneApplicationPage
  {
    private readonly GeoCoordinateWatcher _watcher;

    // Constructor
    public MainPage()
    {
      InitializeComponent();

      if( _watcher == null )
      {
        _watcher = new GeoCoordinateWatcher( GeoPositionAccuracy.High ) {MovementThreshold = 20};
        _watcher.StatusChanged += Watcher_StatusChanged;
      }

      _watcher.Start();

      this.Loaded += new RoutedEventHandler( MainPage_Loaded );
    }

    void MainPage_Loaded( object sender, RoutedEventArgs e )
    {
      if( string.IsNullOrEmpty( Defaults.APPLICATION_ID ) || string.IsNullOrEmpty( Defaults.SECRET_KEY ) ||
          string.IsNullOrEmpty( Defaults.VERSION ) )
      {
        NavigationService.Navigate( new Uri( "/ErrorPage.xaml", UriKind.Relative ) );
        return;
      }

      Backendless.URL = "http://api.backendless.com";
      Backendless.InitApp( Defaults.APPLICATION_ID, Defaults.SECRET_KEY, Defaults.VERSION );
    }

    private void SearchButton_Click( object sender, RoutedEventArgs e )
    {
      NavigationService.Navigate(
                                       new Uri(
                                         "/PointsPage.xaml?" + Defaults.LATITUDE_TAG + "=" + LatitudeField.Text + "&" +
                                         Defaults.LONGITUDE_TAG + "=" + LongitudeField.Text + "&" + Defaults.RADIUS_TAG + "=" +
                                         RadiusSlider.Value, UriKind.Relative ) );
    }

    private void RadiusSlider_OnMovedSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      RadiusField.Text = ((Int32)e.NewValue).ToString(CultureInfo.InvariantCulture);
    }

    private void Watcher_StatusChanged( object sender, GeoPositionStatusChangedEventArgs e )
    {
      if( e.Status == GeoPositionStatus.Ready )
      {
        var geoCoordinate = _watcher.Position.Location;
        LatitudeField.Text = geoCoordinate.Latitude.ToString( "0.000" );
        LongitudeField.Text = geoCoordinate.Longitude.ToString( "0.000" );

        _watcher.Stop();
      }
    }
  }
}