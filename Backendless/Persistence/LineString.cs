using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackendlessAPI
{
  public class LineString : Geometry
  {
    public const String GEOJSON_TYPE = "LineString";
    public static String WKT_TYPE = GEOJSON_TYPE.ToUpper();

    private List<Point> points;

    public LineString( List<Point> points ) : this( points, SpatialReferenceSystem.DEFAULT )
    {
    }

    public LineString( List<Point> points, ReferenceSystemEnum srs ) : base( srs )
    {
      this.points = new List<Point>( points );
    }

    public List<Point> GetPoints()
    {
      return new List<Point>( this.points );
    }

    public LineString SetPoints( List<Point> points )
    {
      this.points.Clear();
      this.points.AddRange( points );
      return this;
    }

    public override String GetGeoJSONType()
    {
      return LineString.GEOJSON_TYPE;
    }

    public override String GetWKTType()
    {
      return LineString.WKT_TYPE;
    }

    internal override String JSONCoordinatePairs()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append( '[' );

      foreach( Point p in this.GetPoints() )
      {
        sb.Append( p.JSONCoordinatePairs() );

        if( points[ points.Count - 1 ] != p )
          sb.Append( ',' );
      }

      sb.AppendLine( "]" );
      return sb.ToString();
    }

    internal override String WKTCoordinatePairs()
    {
      StringBuilder sb = new StringBuilder();
      foreach( Point p in this.GetPoints() )
      {
        sb.Append( p.WKTCoordinatePairs() );

        if( points[ points.Count - 1 ] != p )
          sb.Append( ',' );
      }

      return sb.ToString();
    }

    public override bool Equals( object obj )
    {
      if( this == obj )
        return true;

      if( !( obj is LineString ) )
        return false;

      LineString that = ( LineString )obj;

      return srs == that.srs && points.SequenceEqual( that.points );
    }

    public override int GetHashCode()
    {
      return  points.GetHashCode() + srs.GetHashCode();
    }
  }
}
