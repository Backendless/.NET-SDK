﻿using System;
using System.Collections.Generic;
using BackendlessAPI.Utils;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BackendlessAPI.Persistence
{
  public class GeoJSONParser<T> where T : Geometry
  {
    private T geomClass;
    private ReferenceSystemEnum srs;

    public GeoJSONParser() : this( SpatialReferenceSystem.DEFAULT, null )
    {
    }
    
    public GeoJSONParser( ReferenceSystemEnum srs ) : this( srs, null )
    {
    }

    public GeoJSONParser( String geomClassName ) : this( SpatialReferenceSystem.DEFAULT, geomClassName )
    {
    }

    public GeoJSONParser( ReferenceSystemEnum srs, String geomClassName )
    {
      this.srs = srs;

      if( geomClassName != null )
      {
        try
        {
          Assembly asm = Assembly.GetExecutingAssembly();
          T unchekedClazz = (T) asm.CreateInstance( geomClassName );
          geomClass = unchekedClazz;
        }
        catch
        {
          throw new ArgumentException( $"'geomClassName' contains unknown class '{geomClassName}'." );
        }
      }

      else
        geomClass = null;
    }

    public Geometry Read( String geoJSON )
    {
      if( geoJSON == null )
        return null;

      Dictionary<string, object> geoJSONMap;
      try
      {
        geoJSONMap = new Json().Deserialize( geoJSON );
      }
      catch( System.Exception ex )
      {
        throw new GeoJSONParserException( ex );
      }

      return Read( geoJSONMap );
    }

    public Geometry Read( Dictionary<string, object> geoJSON )
    {
      string type = (string) geoJSON[ "type" ];
      Object coordinatesObj = geoJSON[ "coordinates" ];
      Object[] coordinates = null;

      if( coordinatesObj is List<Object> )
        coordinates = ( (List<Object>) coordinatesObj ).ToArray();
      else if( coordinatesObj is Double[] )
        coordinates = ( (Double[]) coordinatesObj ).Select( d => (Object) d ).ToArray();
      else if( coordinatesObj != null )
        coordinates = ( (List<Double>) coordinatesObj ).Select( d => (Object) d ).ToArray();

      if( type == null || coordinates == null )
        throw new GeoJSONParserException( "Both 'type' and 'coordinates' should be present in GeoJSON object." );

      if( this.geomClass == null || this.geomClass.GetType() == typeof( Geometry ) )
      {
        switch( type )
        {
          case Point.GEOJSON_TYPE:
          return ConstructPointFromCoordinates( coordinates );
          case LineString.GEOJSON_TYPE:
          return ConstructLineStringFromCoordinates( coordinates );
          case Polygon.GEOJSON_TYPE:
          return ConstructPolygonFromCoordinates( coordinates );
        }
      }
      else
        throw new GeoJSONParserException( $"Unknown geometry class: '{this.geomClass}" );

      throw new GeoJSONParserException( $"Unknown geometry type: '{type}'" );
    }

    private Point ConstructPointFromCoordinates( Object[] coordinatePair )
    {
      return new Point( srs ).SetX( (double) coordinatePair[ 0 ] ).SetY( (double) coordinatePair[ 1 ] );
    }

    private LineString ConstructLineStringFromCoordinates( Object[] arrayOfCoordinatePairs)
    {
      List<Point> points = new List<Point>();
      Object[] coordinatePairNumbers;

      foreach( Object coordinatePairObj in arrayOfCoordinatePairs )
      {
        coordinatePairNumbers = ((List<Double>) coordinatePairObj).Select( d => (Object) d ).ToArray();
        points.Add( new Point( srs ).SetX( (double) coordinatePairNumbers[ 0 ] ).SetY( (double) coordinatePairNumbers[ 1 ] ) );
      }

      return new LineString( points, this.srs );
    }

    private Polygon ConstructPolygonFromCoordinates( Object[] arrayOfCoordinateArrayPairs )
    {
      List<LineString> lineStrings = new List<LineString>();
      Object[] arrayOfCoordinatePairs;

      foreach( Object arrayOfCoordinatePairsObj in arrayOfCoordinateArrayPairs )
      {
        arrayOfCoordinatePairs = ((List<Object>) arrayOfCoordinatePairsObj).ToArray();
        LineString lineString = ConstructLineStringFromCoordinates( arrayOfCoordinatePairs );
        lineStrings.Add( lineString );
      }

      if( lineStrings == null )
        throw new GeoJSONParserException( "Polygon's GeoJSON should contain at least one LineString." );

      LineString shell = lineStrings.ElementAt( 0 );
      List<LineString> holes = lineStrings.Skip( 1 ).ToList();

      return new Polygon( shell, holes, srs );
    }

    public class GeoJSONParserException : System.Exception
    {
      public GeoJSONParserException( String message ) : base( message )
      {
      }

      public GeoJSONParserException( System.Exception exception ) : base ( exception.Message )
      {
      }

      public GeoJSONParserException( String message, System.Exception exception ) : base( message, exception )
      {
      }
    }
  }
}
