using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BackendlessAPI
{
  public class SpatialReferenceSystemEnum
  {
    public IList<SpatialReferenceSystemEnum> values = new List<SpatialReferenceSystemEnum>(); 
    static SpatialReferenceSystemEnum CARTESIAN = new SpatialReferenceSystemEnum( 0, "Cartesian" ));
    static SpatialReferenceSystemEnum PULKOVO_1995 = new SpatialReferenceSystemEnum( 4200, "Pulkovo_1995" );
    static SpatialReferenceSystemEnum WGS84 = new SpatialReferenceSystemEnum( 4326, "WGS 84" );
    static SpatialReferenceSystemEnum WGS84_PSEUDO_MERCATOR = new SpatialReferenceSystemEnum( 3857, "WGS 84 / Pseudo-Mercator" );
    static SpatialReferenceSystemEnum WGS84_WORLD_MERCATOR = new SpatialReferenceSystemEnum( 3395, "WGS 84 / World Mercator" );

    

    private int SrsId { get; }
    private String Name { get; }
    
    public static SpatialReferenceSystemEnum SERVER_DEFAULT = CARTESIAN;
    public static SpatialReferenceSystemEnum DEFAULT = SpatialReferenceSystemEnum.WGS84;

    public static SpatialReferenceSystemEnum valueBySRSId( IList<SpatialReferenceSystemEnum> values, int srsId)
    {
      foreach ( SpatialReferenceSystemEnum srs in values )
      {
        if ( srs.SrsId == srsId )
          return srs;
      }

      throw new ArgumentException( $"SpatialReferenceSystem doesn't contain value with id {srsId}" );
    }

    internal SpatialReferenceSystemEnum( int srsId, String name )
    {
      SrsId = srsId;
      Name = name;
      values.Add( this );
    }

    public override String ToString()
    {
      return $"{Name}({SrsId})";
    }
  }
}
