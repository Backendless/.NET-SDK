using System;

namespace BackendlessAPI.Geo.Fence
{
  public delegate void GeoPointEntered( LocationInfo locationInfo );
  public delegate void GeoPointStayed( LocationInfo locationInfo );
  public delegate void GeoPointExited( LocationInfo locationInfo );

  public class GeofenceCallback
  {
    internal GeoPointEntered OnEnterHandler;
    internal GeoPointStayed OnStayHandler;
    internal GeoPointExited OnExitHandler;

    public GeofenceCallback( GeoPointEntered OnEnterHandler, GeoPointStayed OnStayHandler, GeoPointExited OnExitHandler )
    {
      this.OnEnterHandler = OnEnterHandler;
      this.OnStayHandler = OnStayHandler;
      this.OnExitHandler = OnExitHandler;
    }
  }


  public class LocationInfo
  {
    internal LocationInfo( String geofenceName, String geofenceId, double latitude, double longitude )
    {
      this.GeofenceName = geofenceName;
      this.GeofenceId = geofenceId;
      this.Latitude = latitude;
      this.Longitude = longitude;
    }

    public String GeofenceName { get; set; }
    public String GeofenceId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
  }
}