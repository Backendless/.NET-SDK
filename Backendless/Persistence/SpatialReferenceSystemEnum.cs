using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BackendlessAPI
{
  public class SpatialReferenceSystemEnum
  {
    public enum ReferenceSystemEnum
    {
      CARTESIAN = 0,
      PULKOVO_1995 = 4200,
      WGS84 = 4326,
      WGS84_PSEUDO_MERCATOR = 3857,
      WGS84_WORLD_MECATOR = 3395
    }

    public static ReferenceSystemEnum SERVER_DEFAULT = ReferenceSystemEnum.CARTESIAN;
    public static ReferenceSystemEnum DEFAULT = ReferenceSystemEnum.WGS84;

    static Dictionary<object, String> values = new Dictionary<object, String>(){ { 0, "Cartesian"},{ 4200, "Pulkovo_1995"},
                                   { 4326, "WGS84" },{3867,"WGS 84 / Pseudo-Mercator"}, {3395, "WGS 84 / World Mercator"}};
    public static String valueBySRSId( int srsId )
    {
      if ( values["srsId"] != null )
        return values["srsId"]; 
      throw new ArgumentException( $"SpatialReferenceSystem doesn't contain value with id {values.Keys}" );
    }

    public override String ToString()
    {
      return $"{values.Values}({values.Keys})";
    }
  }
}
