using System.Collections.Generic;
using System.Text;
using Weborb.Service;

namespace BackendlessAPI.Geo
{
  public class GeoPoint
  {
    private List<string> _categories;
    private Dictionary<string, object> _metadata;

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

      foreach( KeyValuePair<string, string> keyValue in metadata )
        Metadata.Add( keyValue.Key, keyValue.Value );
    }

    public GeoPoint( double latitude, double longitude, List<string> categories, Dictionary<string, object> metadata )
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
    public Dictionary<string, object> Metadata
    {
      get { return _metadata ?? (_metadata = new Dictionary<string, object>()); }
      set { _metadata = value; }
    }

    public override string ToString()
    {
      var myStringBuilder = new StringBuilder();
      bool first = true;

      foreach( KeyValuePair<string, object> pair in Metadata )
      {
        if( first )
          first = false;
        else
          myStringBuilder.Append( ";" );

        string value = pair.Value.GetType().IsArray ? string.Join( ",", (string[])pair.Value ) : pair.Value.ToString();
        myStringBuilder.AppendFormat( "{0}={1}", pair.Key, value );
      }

      string metaStr = myStringBuilder.ToString();

      return string.Format( "GeoPoint{{ objectId='{0}', latitude={1}, longitude={2}, categories={3}, metadata={4}, distance={5} }}", ObjectId, Latitude, Longitude,
        string.Join( ",", Categories.ToArray() ), metaStr, Distance );
    }
  }
}