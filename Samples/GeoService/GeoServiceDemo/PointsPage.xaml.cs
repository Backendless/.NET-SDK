using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Geo;
using Microsoft.Phone.Controls;

namespace Examples.MessagingService.GeoServiceDemo
{
  public partial class PointsPage : PhoneApplicationPage
  {
    private readonly BackendlessGeoQuery _backendlessGeoQuery;
    private readonly CityPointsList _cityPointsList;

    internal event AsyncStartedEventHandler AsyncStartedEvent;

    internal event AsyncFinishedEventHandler AsyncFinishedEvent;

    internal delegate void AsyncFinishedEventHandler();

    internal delegate void AsyncStartedEventHandler();

    public PointsPage()
    {
      InitializeComponent();

      _cityPointsList = new CityPointsList();
      _backendlessGeoQuery = new BackendlessGeoQuery();
      CityPointsDataGrid.DataContext = _cityPointsList;

      AsyncStartedEvent += () =>
      {
        ProgressBar.Visibility = Visibility.Visible;
      };
      AsyncFinishedEvent += () =>
      {
        ProgressBar.Visibility = Visibility.Collapsed;
      };
    }

    protected override void OnNavigatedTo( NavigationEventArgs e )
    {
      base.OnNavigatedTo( e );

      _backendlessGeoQuery.Categories.Add( Defaults.SAMPLE_CATEGORY );
      _backendlessGeoQuery.Latitude = Convert.ToDouble( NavigationContext.QueryString[Defaults.LATITUDE_TAG] );
      _backendlessGeoQuery.Longitude = Convert.ToDouble( NavigationContext.QueryString[Defaults.LONGITUDE_TAG] );
      _backendlessGeoQuery.Radius = Convert.ToDouble( NavigationContext.QueryString[Defaults.RADIUS_TAG] );
      _backendlessGeoQuery.Units = Units.KILOMETERS;
      RadiusSlider.Value = _backendlessGeoQuery.Radius;

      SearchPoints();
    }

    private void RadiusSlider_OnValueChanged( object sender,
                                              ManipulationCompletedEventArgs manipulationCompletedEventArgs )
    {
      if(RadiusSlider.Value < 1)
        return;

      _backendlessGeoQuery.Radius = RadiusSlider.Value;
      SearchPoints();
    }

    private void SearchPoints()
    {
      if(_backendlessGeoQuery.Radius < 1 )
        return;

      AsyncStartedEvent.Invoke();
      _cityPointsList.Clear();
      Backendless.Geo.GetPoints( _backendlessGeoQuery,
                                 new AsyncCallback<BackendlessCollection<GeoPoint>>(
                                   response =>
                                   Dispatcher.BeginInvoke( () =>
                                     {
                                       _cityPointsList.SetAll( response.GetCurrentPage() ); 
                                       AsyncFinishedEvent.Invoke();
                                     }),
                                   fault => Dispatcher.BeginInvoke( () =>
                                     {
                                       AsyncFinishedEvent.Invoke();
                                       MessageBox.Show( fault.Message );
                                     } ) ) );
    }

    private void RadiusSlider_OnMovedSlider( object sender, RoutedPropertyChangedEventArgs<double> e )
    {
      RadiusField.Text = ((Int32) e.NewValue).ToString( CultureInfo.InvariantCulture );
    }
  }
}