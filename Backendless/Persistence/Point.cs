using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  public class Point : Geometry
  {
    public static double PRECISION = .000000001;
    public static String GEOJSON_TYPE = "Point";
    public static String WKT_TYPE = GEOJSON_TYPE.ToUpper();

    private double x;
    private double y;

    public Point() : base(SpatialReferenceSystemEnum.DEFAULT)
    {
    }

    public Point( SpatialReferenceSystemEnum.ReferenceSystemEnum srs) :base(srs)
    {
    }

    public Point SetX(double x)
    {
      this.x = x;
      return this;
    }
    public Point SetY(double y)
    {
      this.y = y;
      return this;
    }
    public double GetX()
    {
      return x;
    }
    public double GetY()
    {
      return y;
    }

    public Point SetLongitude(double Longitude)
    {
      return SetX( x );
    }
    public Point SetLatitude(double Latitude)
    {
      return SetY( y );
    }
    public double GetLongitude()
    {
      return x;
    }
    public double GetLatitude()
    {
      return y;
    }

    public override String GetGeoJSONType()
    {
      return Point.GEOJSON_TYPE;
    }

    public override String GetWKTType()
    {
      return Point.WKT_TYPE;
    }

    
    internal override String WKTCoordinatePairs()
    {
      return $"{x} {y}";
    }
    internal override String JSONCoordinatePairs()
    {
      return $"[{x},{y}]";
    }
    
    public override bool Equals( object obj )
    {
      if ( this == obj )
        return true;
      if ( !( obj is Point ) )
        return false;
      Point point = ( Point )obj;
      return Math.Abs( point.x - x ) < PRECISION && Math.Abs( point.y - y ) < PRECISION && srs == point.srs;
    }
    public override int GetHashCode()
    {
      return ( x, y, srs ).GetHashCode();
    }
  }
}
