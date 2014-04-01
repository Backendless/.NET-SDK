using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Geo
{
  public class GeoPoint
  {
    private List<string> _categories;
    private Dictionary<string, string> _metadata;

    public GeoPoint()
    {
    }

    public GeoPoint( double latitude, double longitude )
    {
      Latitude = latitude;
      Longitude = longitude;
    }

    public GeoPoint( double latitude, double longitude, List<string> categories, Dictionary<string, string> metadata )
    {
      Latitude = latitude;
      Longitude = longitude;
      Categories = categories;
      Metadata = metadata;
    }

    [SetClientClassMemberName( "objectId" )]
    public string ObjectId { get; set; }

    [SetClientClassMemberName( "latitude" )]
    public double Latitude { get; set; }

    [SetClientClassMemberName( "longitude" )]
    public double Longitude { get; set; }

    [SetClientClassMemberName( "distance" )]
    public double Distance { get; set; }

    [SetClientClassMemberName( "categories" )]
    public List<string> Categories
    {
      get { return _categories ?? (_categories = new List<string>()); }
      set { _categories = value; }
    }

    [SetClientClassMemberName( "metadata" )]
    public Dictionary<string, string> Metadata
    {
      get { return _metadata ?? (_metadata = new Dictionary<string, string>()); }
      set { _metadata = value; }
    }
  }
}