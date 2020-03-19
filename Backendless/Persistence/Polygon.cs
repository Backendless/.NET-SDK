using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendlessAPI
{
  public class Polygon : Geometry
  {
    public static String GEOJSON_TYPE = "Polygon";
    public static String WKT_TYPE = GEOJSON_TYPE.ToUpper();

    private LineString boundary;
    private List<LineString> holes;

    public Polygon( LineString boundary ) : this( boundary, null, SpatialReferenceSystemEnum.DEFAULT )
    {
    }

    public Polygon( List<Point> boundary, List<LineString> holes ) : this( boundary, holes, SpatialReferenceSystemEnum.DEFAULT )
    {
    }

    public Polygon( List<Point> boundary, List<LineString> holes, SpatialReferenceSystemEnum.ReferenceSystemEnum srs)
    : this( new LineString( boundary, srs), holes, srs )
    {
    }

    public Polygon( LineString boundary, List<LineString> holes ) 
    : this( boundary, holes, SpatialReferenceSystemEnum.DEFAULT )
    {
    }

    public Polygon( LineString boundary, List<LineString> holes, SpatialReferenceSystemEnum.ReferenceSystemEnum srs )
    :base( srs )
    {
      if ( boundary == null )
        throw new ArgumentException( "The 'shell' shouldn't be null." );
      if ( holes != null )
      {
        this.holes = new List<LineString>( holes );
      }
      else
        this.holes = new List<LineString>( holes );

      this.boundary = boundary;
    }

    public LineString GetBoundary()
    {
      return this.boundary;
    }

    public Polygon SetBoundary(LineString boundary)
    {
      this.boundary = boundary;
      return this;
    }

    public List<LineString> GetHoles()
    {
      return this.holes;
    }

    public Polygon SetHoles(List<LineString> holes)
    {
      this.holes.Clear();
      this.holes.AddRange( holes );
      return this;
    }

    public override String GetGeoJSONType()
    {
      return Polygon.GEOJSON_TYPE;
    }

    public override String GetWKTType()
    {
      return Polygon.WKT_TYPE;
    }

    internal override String JSONCoordinatePairs()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append( '[' );
      sb.Append( this.GetBoundary().JSONCoordinatePairs() ).Append( "," );

      if( this.GetHoles() != null )
        foreach ( LineString ls in this.GetHoles() )
          sb.Append( ls.JSONCoordinatePairs() ).Append( "," );

      sb.AppendLine( "]" );
        return sb.ToString();
    }

    internal override string WKTCoordinatePairs()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append( '(' ).Append( this.GetBoundary().WKTCoordinatePairs() ).Append( ")," );

      if ( this.GetHoles() != null )
        foreach ( LineString ls in this.GetHoles() )
          sb.Append( '(' ).Append( this.GetBoundary().WKTCoordinatePairs() ).Append( ")," );

      return sb.ToString();
    }

    public override bool Equals( object obj )
    {
      if ( this == obj )
        return true;
      if ( !( obj is Polygon ) )
        return false;
      Polygon polygon = ( Polygon )obj;
      return Object.Equals( this.boundary, polygon.boundary ) && Object.Equals( this.holes, polygon.holes )
                                                                                     && srs == polygon.srs;
    }

    public override int GetHashCode()
    {
      return (boundary, holes, srs).GetHashCode();
    }
  }
}
