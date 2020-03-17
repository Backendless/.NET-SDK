using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  public class GeometryDTO
  {
    private String geomClass;
    private int srsId;
    private String geoJSON;

    public String GeomClass
    {
      get { return geomClass; }
      set { geomClass = value; }
    }

    public int SrsId
    {
      get { return srsId; }
      set { srsId = value; }
    }

    public String GeoJSON
    {
      get { return geoJSON; }
      set { geoJSON = value; }
    }
    public GeometryDTO()
    {
    }

    public GeometryDTO(String geomClass, int srsId, String geoJSON)
    {
      this.geomClass = geomClass;
      this.srsId = srsId;
      this.geoJSON = geoJSON;
    }

    //public T Geometry where T : Geometry()
    public override bool Equals( object obj )
    {
      if ( this == obj )
        return true;
      if ( !( obj is GeometryDTO ) )
        return false;
      GeometryDTO that = ( GeometryDTO )obj;
      return Object.Equals( geomClass, that.geomClass ) && Object.Equals( srsId, that.srsId ) 
                                                    && Object.Equals( geoJSON, that.geoJSON );
    }

    public override int GetHashCode()
    {
      return ( geomClass, srsId, geoJSON ).GetHashCode();
    }
  }
}
