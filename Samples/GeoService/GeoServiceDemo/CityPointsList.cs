using System.Collections.Generic;
using System.Collections.ObjectModel;
using BackendlessAPI.Geo;

namespace Examples.MessagingService.GeoServiceDemo
{
  public class CityPointsList : ObservableCollection<CityPoint>
  {
    public void SetAll( IEnumerable<GeoPoint> geoPoints )
    {
      Clear();

      foreach( var geoPoint in geoPoints )
        Add( new CityPoint( geoPoint, (string) geoPoint.Metadata[Defaults.CITY_TAG] ) );
    }
  }

  public class CityPoint : GeoPoint
  {
    public CityPoint( GeoPoint geoPoint, string city ) : base( geoPoint.Latitude, geoPoint.Longitude )
    {
      City = city;
    }

    public string City { get; set; }
  }
}