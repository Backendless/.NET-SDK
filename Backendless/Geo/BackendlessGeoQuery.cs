using System;
using System.Collections.Generic;
using BackendlessAPI.Data;
using Weborb.Service;

namespace BackendlessAPI.Geo
{
  public class BackendlessGeoQuery : IBackendlessQuery
  {
    public static readonly int CLUSTER_SIZE_DEFAULT_VALUE = 100; // pixels
    private List<string> _categories;
    private bool _includeMeta = true;
    private Dictionary<string, string> _metadata = new Dictionary<string, string>();
    private Double _latitude = Double.NaN;
    private Double _longitude = Double.NaN;
    private Double _radius = Double.NaN;
    private Units? _units;
    private double[] _searchRectangle;
    private int _pageSize;
    private int _offset;
    private Dictionary<string, string> _relativeFindMetadata = new Dictionary<string, string>();
    private double _relativeFindPercentThreshold = 0;
    private String _whereClause;

    public BackendlessGeoQuery()
    {
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
    }

    public BackendlessGeoQuery( double latitude, double longitude, int pageSize, int offset )
    {
      Latitude = latitude;
      Longitude = longitude;
      PageSize = pageSize;
      Offset = offset;
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
    }

    public BackendlessGeoQuery( List<string> categories )
    {
      Categories = categories;
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
    }

    public BackendlessGeoQuery( double latitude, double longitude, double radius, Units units ) : this( latitude, longitude, radius, units, null )
    {

    }

    public BackendlessGeoQuery( double latitude, double longitude, double radius, Units units, List<String> categories ) :
      this( latitude, longitude, radius, units, categories, null )
    {
    }

    public BackendlessGeoQuery( double latitude, double longitude, double radius, Units units, List<string> categories, Dictionary<string, string> metadata )
    {
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
      Latitude = latitude;
      Longitude = longitude;
      Radius = radius;
      Units = units;
      Categories = categories;
      Metadata = metadata;

      if( metadata != null )
        IncludeMeta = true;
    }

    public BackendlessGeoQuery( GeoPoint NW, GeoPoint SE ) : this( NW.Latitude, NW.Longitude, SE.Latitude, SE.Longitude )
    {
    }

    public BackendlessGeoQuery( double NWLat, double NWLon, double SELat, double SWLon ) 
    {
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
      SearchRectangle = new[] { NWLat, NWLon, SELat, SWLon };
    }

    public BackendlessGeoQuery( double NWLat, double NWLon, double SELat, double SWLon, Units units, List<string> categories )
    {
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
      SearchRectangle = new[] { NWLat, NWLon, SELat, SWLon };
      Units = units;
      Categories = categories;
    }

    public BackendlessGeoQuery( Dictionary<string, string> metadata )
    {
      ClusterGridSize = CLUSTER_SIZE_DEFAULT_VALUE;
      Metadata = metadata;

      if( metadata != null )
        IncludeMeta = true;
    }

    [SetClientClassMemberName( "latitude" )]
    public double Latitude
    {
      get { return _latitude; }
      set { _latitude = value; }
    }

    [SetClientClassMemberName( "longitude" )]
    public double Longitude
    {
      get { return _longitude; }
      set { _longitude = value; }
    }

    [SetClientClassMemberName( "radius" )]
    public double Radius
    {
      get { return _radius; }
      set { _radius = value; }
    }

    [SetClientClassMemberName( "units" )]
    public Units? Units
    {
      get { return _units; }
      set { _units = value; }
    }

    [SetClientClassMemberName( "categories" )]
    public List<string> Categories
    {
      get { return _categories ?? ( _categories = new List<string>() ); }
      set { _categories = value; }
    }

    [SetClientClassMemberName( "metadata" )]
    public Dictionary<string, string> Metadata
    {
      get { return _metadata; }
      set { _metadata = value; }
    }

    [SetClientClassMemberName( "includeMeta" )]
    public bool IncludeMeta
    {
      get { return _includeMeta; }
      set { _includeMeta = value; }
    }

    [SetClientClassMemberName( "searchRectangle" )]
    public double[] SearchRectangle
    {
      get { return _searchRectangle; }
      set { _searchRectangle = value; }
    }

    [SetClientClassMemberName( "pageSize" )]
    public int PageSize
    {
      get { return _pageSize; }
      set { _pageSize = value; }
    }

    [SetClientClassMemberName( "offset" )]
    public int Offset
    {
      get { return _offset; }
      set { _offset = value; }
    }

    [SetClientClassMemberName( "whereClause" )]
    public String WhereClause
    {
      get { return _whereClause; }
      set { _whereClause = value; }
    }

    [SetClientClassMemberName( "relativeFindMetadata" )]
    public Dictionary<string, string> RelativeFindMetadata
    {
      get { return _relativeFindMetadata; }
      set { _relativeFindMetadata = value; }
    }

    [SetClientClassMemberName( "relativeFindPercentThreshold" )]
    public double RelativeFindPercentThreshold
    {
      get { return _relativeFindPercentThreshold; }
      set { _relativeFindPercentThreshold = value; }
    }

    [SetClientClassMemberName( "dpp" )]
    public double DPP { get; set; }

    [SetClientClassMemberName( "clusterGridSize" )]
    public int ClusterGridSize { get; set; }

    public void SetSearchRectangle( GeoPoint topLeft, GeoPoint bottomRight )
    {
      _searchRectangle = new[] { topLeft.Latitude, topLeft.Longitude, bottomRight.Latitude, bottomRight.Longitude };
    }

    // same as above, but uses the default clusterGridSize of 100
    public void SetClusteringParams( double westLongitude, double eastLongitude, int mapWidth )
    {
      SetClusteringParams( westLongitude, eastLongitude, mapWidth, CLUSTER_SIZE_DEFAULT_VALUE );
    }

    public void SetClusteringParams( double westLongitude, double eastLongitude, int mapWidth, int cluisterGridSize )
    {
      double longDiff = eastLongitude - westLongitude;

      if( longDiff < 0 )
        longDiff += 360;

      DPP = longDiff / mapWidth;
      ClusterGridSize = cluisterGridSize;
    }

    public IBackendlessQuery NewInstance()
    {
      return new BackendlessGeoQuery
          {
            Latitude = Latitude,
            Longitude = Longitude,
            Radius = Radius,
            Units = Units,
            Categories = Categories,
            IncludeMeta = IncludeMeta,
            Metadata = Metadata,
            SearchRectangle = SearchRectangle,
            PageSize = PageSize,
            Offset = Offset,
            RelativeFindMetadata = RelativeFindMetadata,
            RelativeFindPercentThreshold = RelativeFindPercentThreshold,
            WhereClause = WhereClause
          };
    }
  }
}