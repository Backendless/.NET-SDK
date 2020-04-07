using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BackendlessAPI.Persistence
{
  abstract public class Geometry
  {
    protected ReferenceSystemEnum srs;

    protected Geometry( ReferenceSystemEnum srs )
    {
      this.srs = srs;
    }

    public static T FromWKT<T>( String wellKnownText ) where T : Geometry
    {
      return (T) new WKTParser().Read( wellKnownText );
    }

    public static T FromGeoJSON<T>( String geoJSON ) where T : Geometry
    {
      return (T) new GeoJSONParser<T>().Read( geoJSON );
    }

    public static T FromWKT<T>( String wellKnownText, ReferenceSystemEnum srs ) where T : Geometry
    {
      return (T) new WKTParser( srs ).Read( wellKnownText );
    }

    public static T FromGeoJSON<T>( String geoJSON, ReferenceSystemEnum srs ) where T : Geometry
    {
      return (T) new GeoJSONParser<T>( srs ).Read( geoJSON );
    }


    public ReferenceSystemEnum getSRS()
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
