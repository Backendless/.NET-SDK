using System;
using System.Collections.Generic;
using BackendlessAPI.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Reflection;

namespace BackendlessAPI
{
  public class GeoJSONParser<T> where T : Geometry
  {
   /* private T geomClass;
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
      if ( geomClassName != null )
      {
        try
        {
          Assembly asm = Assembly.GetExecutingAssembly();
          T unchekedClazz = ( T )asm.CreateInstance( geomClassName );
          this.geomClass = unchekedClazz;
        }
        catch
        {
          throw new ArgumentException( $"'geomClassName' contains uknown class '{geomClassName}'." );
        }
      }
      else
        this.geomClass = null;
    }

   /* public Geometry Read( String geoJSON )
    {
      if ( geoJSON == null )
        return null;

      Dictionary<string, object> geoJSONMap;
      try
      {
        Json json = new Json();
        geoJSONMap = json.Deserialize( geoJSON );
      }
      catch( System.Exception ex )
      {
        throw new GeoJSONParserException( ex );
      }
      return Read( geoJSONMap );
    }*/

    /*public Geometry Read(Dictionary<string, object> geoJSON)
    {
      //string type = (string) geoJSON.Get("type");
      //Object coordinatesObj = geoJSON.Get("coordinates");
      //int srsId = (int) geoJSON.Keys;
    }*/
    
   /* public class GeoJSONParserException : System.Exception
    {
      public GeoJSONParserException( String message ) : base( message )
      {
      }

      public GeoJSONParserException( System.Exception exception ) : base ( exception.Message )
      {
      }

      public GeoJSONParserException( String message, System.Exception exception) : base( message, exception )
      {
      }
    }*/
  }
}
