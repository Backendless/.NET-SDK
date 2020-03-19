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

    public GeoJSONParser() : this( SpatialReferenceSystemEnum.DEFAULT, null )
    {
    }
    
    public GeoJSONParser( SpatialReferenceSystemEnum.ReferenceSystemEnum srs ) : this( srs, null )
    {
    }

    public GeoJSONParser( String geomClassName ) : this( SpatialReferenceSystemEnum.DEFAULT, geomClassName )
    {
    }

    public GeoJSONParser( SpatialReferenceSystemEnum.ReferenceSystemEnum srs, String geomClassName )
    {
      this.srs = srs;
      if(geomClassName != null)
      {
        T unchekedClazz = ( T )Type.GetType( geomClassName );
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
