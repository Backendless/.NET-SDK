using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  abstract public class Geometry
  {
    protected SpatialReferenceSystemEnum srs;

    protected Geometry( SpatialReferenceSystemEnum srs )
    {
      if ( srs == null )
        throw new ArgumentNullException( "Spatial Reference System(SRS) cannot be null" );
      this.srs = srs;
    }

    public static T FromWKT( String wellKnownText )
    {
      return ( T )new WKTParser().Read( wellKnownText );
    }

    public static T FromGeoJSON( String geoJSON )
    {
      return ( T )new GeoJSONParser().Read( geoJSON );
    }

    public static T FromWKT( String wellKnownText, SpatialReferenceSystemEnum srs )
    {
      return ( T )new WKTParser( srs ).Read( wellKnownText );
    }

    public static T FromGeoJSON( String geoJSON, SpatialReferenceSystemEnum srs )
    {
      return ( T )new GeoJSONParser( srs ).Read( geoJSON );
    }
    public SpatialReferenceSystemEnum getSRS()
    {
      return srs;
    }
    
    abstract public String GetGeoJSONType();

    abstract public String GetWKTType();

    abstract internal String JSONCoordinatePairs();

    abstract internal String WKTCoordinatePairs();

    public String AsGeoJSON()
    {
      return "{\"type\":\"" + this.GetGeoJSONType() + "\",\"coordinates:\":" + this.JSONCoordinatePairs() + "}";
    }

    public String AsWKT()
    {
      return GetWKTType() + $"({this.WKTCoordinatePairs()})";
    }

    public override string ToString()
    {
      return $"'{AsWKT()}'";
    }
  }
}
