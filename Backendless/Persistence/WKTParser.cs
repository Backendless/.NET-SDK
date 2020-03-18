using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  class WKTParser
  {
    private static String EMPTY = "EMPTY";
    private static String COMMA = ",";
    private static String L_PAREN = "(";
    private static String R_PAREN = ")";
    private static String NAN_SYMBOL = "NaN";

    private SpatialReferenceSystemEnum srs;

    public WKTParser()
    {
      this( SpatialReferenceSystemEnum.DEFAULT );//ask it
    }

    public WKTParser( SpatialReferenceSystemEnum srs )
    {
      if ( srs == null )
        throw new ArgumentNullException( "Spatial Reference System (SRS) cannot be null." );
      this.srs = srs;
    }

    public Geometry Read( String wellKnownText )
    {
      StringReader reader = new StringReader( wellKnownText );      
    }
  }
}
