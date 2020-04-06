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

    static Dictionary<int, String> values = new Dictionary<int, String>(){{ (int) ReferenceSystemEnum.CARTESIAN, "Cartesian"},{(int) ReferenceSystemEnum.PULKOVO_1995, "Pulkovo_1995"},
                                   {(int) ReferenceSystemEnum.WGS84, "WGS84"},{(int) ReferenceSystemEnum.WGS84_PSEUDO_MERCATOR,"WGS 84 / Pseudo-Mercator"}, {(int) ReferenceSystemEnum.WGS84_WORLD_MECATOR, "WGS 84 / World Mercator"}};
    public static ReferenceSystemEnum GetName( int srsId )
    {
        foreach( KeyValuePair<int, string> srs in values )
          if( srsId == srs.Key )
            return (ReferenceSystemEnum) srs.Key;
         
      throw new ArgumentException( $"SpatialReferenceSystem does not contain value with id {values.Keys}" );
    }

    public override String ToString()
    {
      return $"{values.Values}({values.Keys})";
    }
  }
}
