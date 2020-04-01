using System;
using System.Collections.Generic;

namespace BackendlessAPI
{
  public enum ReferenceSystemEnum
  {
    CARTESIAN = 0,
    PULKOVO_1995 = 4200,
    WGS84 = 4326,
    WGS84_PSEUDO_MERCATOR = 3857,
    WGS84_WORLD_MECATOR = 3395
  }
  public class SpatialReferenceSystem
  {
    public static ReferenceSystemEnum SERVER_DEFAULT = ReferenceSystemEnum.CARTESIAN;
    public static ReferenceSystemEnum DEFAULT = ReferenceSystemEnum.WGS84;

    static Dictionary<object, String> values = new Dictionary<object, String>(){ { ReferenceSystemEnum.CARTESIAN, "Cartesian"},{ ReferenceSystemEnum.PULKOVO_1995, "Pulkovo_1995"},
                                   { ReferenceSystemEnum.WGS84, "WGS84" },{ ReferenceSystemEnum.WGS84_PSEUDO_MERCATOR,"WGS 84 / Pseudo-Mercator"}, { ReferenceSystemEnum.WGS84_WORLD_MECATOR, "WGS 84 / World Mercator"}};
    public static ReferenceSystemEnum GetName( int srsId )
    {
        foreach ( KeyValuePair<object, string> srs in values )
          if( srsId == (int) srs.Key )
            return ( ReferenceSystemEnum ) srs.Key;
         
      throw new ArgumentException( $"SpatialReferenceSystem does not contain value with id {values.Keys}" );
    }

    public override String ToString()
    {
      return $"{values.Values}({values.Keys})";
    }
  }
}
