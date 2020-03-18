using System;
using System.Collections.Generic;
using BackendlessAPI.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  public class GeoJSONParser<T> where T : Geometry
  {
    private T geomClass;
    private SpatialReferenceSystemEnum.ReferenceSystemEnum srs;

    public GeoJSONParser()
    {
      this( null, null ); //ask it
    }
    
    public GeoJSONParser( SpatialReferenceSystemEnum.ReferenceSystemEnum srs )
    {
      this( srs, null );//ask it
    }

    public GeoJSONParser( String geomClassName )
    {
      this( null, geomClass );
    }

    public GeoJSONParser( SpatialReferenceSystemEnum srs, String geomClassName)
    {
      this.srs = srs;
      if(geomClassName != null)
      {

      }
    }

    public Geometry<T> Read( String geoJSON )
    {
      if ( geoJSON == null )
        return null;

      Dictionary<string, object> geoJSONMap;
      try
      {
        
      }
      catch( Exception ex )
      {
        throw new GeoJSONParserException( ex );
      }
    }
    
    public class GeoJSONParserException
    {
      public GeoJSONParserException( String message )
      {
        
      }
    }
  }
}
