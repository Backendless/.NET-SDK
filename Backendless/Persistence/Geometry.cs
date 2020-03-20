using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  abstract public class Geometry
  {
    protected SpatialReferenceSystemEnum.ReferenceSystemEnum srs;

    protected Geometry( SpatialReferenceSystemEnum.ReferenceSystemEnum srs )
    {
      /*if ( srs == null )
        throw new ArgumentNullException( "Spatial Reference System(SRS) cannot be null" );*/
      this.srs = srs;
    }

    public static T FromWKT<T>( String wellKnownText ) where T : Geometry
    {
      return ( T )new WKTParser().Read( wellKnownText );
    }

   /* public static T FromGeoJSON<T>( String geoJSON ) where T : Geometry
    {
      return ( T )new GeoJSONParser<T>().Read( geoJSON );
    }*/

    public static T FromWKT<T>( String wellKnownText, SpatialReferenceSystemEnum.ReferenceSystemEnum srs ) where T : Geometry
    {
      return ( T )new WKTParser( srs ).Read( wellKnownText );
    }

    /*public static T FromGeoJSON<T>( String geoJSON, SpatialReferenceSystemEnum.ReferenceSystemEnum srs ) where T : Geometry
    {
      return ( T )new GeoJSONParser<T>( srs ).Read( geoJSON );
    }*/


    public SpatialReferenceSystemEnum.ReferenceSystemEnum getSRS()
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
