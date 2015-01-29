using System;
using Weborb.Service;

namespace BackendlessAPI.Geo
{
  public class GeoCluster : GeoPoint
  {
    [SetClientClassMemberName( "totalPoints" )]
    public int TotalPoints { get; set; }

    internal BackendlessGeoQuery GeoQuery { get; set; }
  }
}
