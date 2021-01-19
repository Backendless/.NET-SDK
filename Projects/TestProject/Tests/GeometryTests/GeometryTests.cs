using Xunit;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;


namespace TestProject
{
  [Collection( "Tests" )]
  public class GeometryDataTypesTestClass : IClassFixture<GeometryTestsInitializator>
  {
    [Fact]
    public void PullAndCompareGeoObjects()
    {
      IList<Dictionary<String, Object>> pers = Backendless.Data.Of( "GeoData" ).Find();

      WKTParser wkt = new WKTParser();

      Point point = (Point) wkt.Read( "POINT (40.41 -3.706)" );
      Geometry geometry = wkt.Read( "POINT (10.2 48.5)" );
      LineString line = (LineString) wkt.Read( "LINESTRING (30.1 10.05, 30.2 10.04)" );
      Polygon poly = (Polygon) wkt.Read( "POLYGON((-77.05786152 38.87261877,-77.0546978 38.87296123,-77.05317431 38.87061405," +
      "-77.0555883 38.86882611,-77.05847435 38.87002898,-77.05786152 38.87261877),(-77.05579215 38.87026286," +
      "-77.05491238 38.87087264,-77.05544882 38.87170794,-77.05669337 38.87156594,-77.05684357 38.87072228," +
      "-77.05579215 38.87026286))" );

      Dictionary<String, Object> result = new Dictionary<String, Object>();

      foreach( Dictionary<String, Object> entry in pers )
        if( entry.ContainsValue( (String) "Geo data name" ) )
        {
          result = new Dictionary<String, object>( entry );
          break;
        }
      Assert.Equal( result[ "P1" ], point );
      Assert.Equal( result[ "GeoValue" ], geometry );
      Assert.Equal( result[ "LineValue" ], line );
      Assert.Equal( result[ "PolyValue" ], poly );
    }

    [Fact]
    public void FactPoint()
    {
      WKTParser wkt = new WKTParser();

      Geometry geometry = wkt.Read( "POINT (30.05 10.1)" );
      Assert.IsType<Point>( geometry );

      Point point = (Point) geometry;

      Assert.Equal( 30.05, point.GetX() );
      Assert.Equal( 10.1, point.GetY() );
    }
    [Fact]
    public void FactLineString()
    {
      WKTParser wkt = new WKTParser();

      Geometry geometry = wkt.Read( "LINESTRING(5.0 10.2, 3.05 8.6, 2.04 11.006)" );
      Assert.IsType<LineString>( geometry );
      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      LineString line = (LineString) geometry;

      Assert.Equal( line, finalLine );
    }
    [Fact]
    public void FactPolygon()
    {
      WKTParser wkt = new WKTParser();
      Geometry geometry = wkt.Read( "POLYGON((1.01 1.02, 2.01 2.02, 3.01 3.02, 1.05 2.02), (1.013 1.014, 1.015 1.016))" );
      Assert.IsType<Polygon>( geometry );

      List<Point> tempList = new List<Point>();
      tempList.Add( new Point().SetX( 1.01 ).SetY( 1.02 ) );
      tempList.Add( new Point().SetX( 2.01 ).SetY( 2.02 ) );
      tempList.Add( new Point().SetX( 3.01 ).SetY( 3.02 ) );
      tempList.Add( new Point().SetX( 1.05 ).SetY( 2.02 ) );

      List<Point> tempList2 = new List<Point>();

      tempList2.Add( new Point().SetX( 1.013 ).SetY( 1.014 ) );
      tempList2.Add( new Point().SetX( 1.015 ).SetY( 1.016 ) );
      LineString tempLines = new LineString( tempList2 );
      List<LineString> lines = new List<LineString>();
      lines.Add( tempLines );


      Polygon poly = new Polygon( tempList, lines );

      Polygon polygon = (Polygon) geometry;

      Assert.Equal( polygon, poly );
    }
    [Fact]
    public void TestJSONPointMethod()
    {
      GeoJSONParser<Point> geo = new GeoJSONParser<Point>();
      Geometry geometry = geo.Read( "{ \"type\": \"Point\", \"coordinates\": [ 1.01, 1.02 ] }" );
      Assert.IsType<Point>( geometry );

      Point point = (Point) geometry;

      Assert.Equal( 1.01, point.GetX() );
      Assert.Equal( 1.02, point.GetY() );

    }
    [Fact]
    public void TestJSONLineStringMethod()
    {
      GeoJSONParser<LineString> geo = new GeoJSONParser<LineString>();
      Geometry geometry = geo.Read( "{ \"type\": \"LineString\", \"coordinates\": [ [ 1.01, 1.02 ]," +
                         "[ 2.01, 2.02 ], [ 3.01, 3.02 ], [ 4.01, 4.02 ] ] }" );
      Assert.IsType<LineString>( geometry );

      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 1.01 ).SetY( 1.02 ) );
      list.Add( new Point().SetX( 2.01 ).SetY( 2.02 ) );
      list.Add( new Point().SetX( 3.01 ).SetY( 3.02 ) );
      list.Add( new Point().SetX( 4.01 ).SetY( 4.02 ) );

      LineString listLineStr = new LineString( list );
      LineString line = (LineString) geometry;

      Assert.Equal( line, listLineStr );
    }
    [Fact]
    public void TestJSONPolygonMethod()
    {
      GeoJSONParser<Polygon> geo = new GeoJSONParser<Polygon>();
      Geometry geometry = geo.Read( "{ \"type\": \"Polygon\", \"coordinates\": [ [ [ 1.01, 1.02 ]," +
                         "[ 2.01, 2.02 ], [ 2.02, 2.03 ], [ 1.015, 1.017 ] ], [ [ 1.013, 1.014 ], [ 1.015, 1.016 ] ] ] }" );
      Assert.IsType<Polygon>( geometry );

      List<Point> tempList = new List<Point>();
      tempList.Add( new Point().SetX( 1.01 ).SetY( 1.02 ) );
      tempList.Add( new Point().SetX( 2.01 ).SetY( 2.02 ) );
      tempList.Add( new Point().SetX( 2.02 ).SetY( 2.03 ) );
      tempList.Add( new Point().SetX( 1.015 ).SetY( 1.017 ) );

      List<Point> tempList2 = new List<Point>();

      tempList2.Add( new Point().SetX( 1.013 ).SetY( 1.014 ) );
      tempList2.Add( new Point().SetX( 1.015 ).SetY( 1.016 ) );
      LineString tempLines = new LineString( tempList2 );
      List<LineString> lines = new List<LineString>();
      lines.Add( tempLines );


      Polygon poly = new Polygon( tempList, lines );
      Polygon polygon = (Polygon) geometry;

      Assert.Equal( polygon, poly );
    }

    [Fact]
    public void TestReceiveGeo()
    {
      IList<Dictionary<String, Object>> result = Backendless.Data.Of( "GeoData" ).Find();
      String StrCoordinates = "POINT(40.41 -3.706)";
      Point point = new Point();

      foreach( Dictionary<String, Object> entry in result )
        if( entry.ContainsValue( (String) "Geo data name" ) )
        {
          point = (Point) entry[ "P1" ];
          break;
        }

      Assert.Equal( point.AsWKT(), StrCoordinates );
    }
    [Fact]
    public void TestPointSave()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();

      pers.Add( "pickupLocation", new Point().SetX( 30.05 ).SetY( 10.1 ) );

      pers = Backendless.Data.Of( "GeoData" ).Save( pers );
      Dictionary<String, Object> result = Backendless.Data.Of( "GeoData" ).FindById( (String) pers[ "objectId" ] );

      Assert.Equal( (Point) result[ "pickupLocation" ], new Point().SetX( 30.05 ).SetY( 10.1 ) );
    }
    [Fact]
    public void TestLineStringSave()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      pers.Add( "LineValue", finalLine );

      pers = Backendless.Data.Of( "GeoData" ).Save( pers );
      Dictionary<String, Object> result = Backendless.Data.Of( "GeoData" ).FindById( (String) pers[ "objectId" ] );

      Assert.Equal( (LineString) result[ "LineValue" ], finalLine );
    }
    [Fact]
    public void TestPolygonSave()
    {
      Dictionary<String, Object> pers = new Dictionary<String, Object>();
      List<Point> tempList = new List<Point>();

      tempList.Add( new Point().SetX( -77.05786152 ).SetY( 38.87261877 ) );
      tempList.Add( new Point().SetX( -77.0546978 ).SetY( 38.87296123 ) );
      tempList.Add( new Point().SetX( -77.05317431 ).SetY( 38.87061405 ) );
      tempList.Add( new Point().SetX( -77.0555883 ).SetY( 38.86882611 ) );
      tempList.Add( new Point().SetX( -77.05847435 ).SetY( 38.87002898 ) );
      tempList.Add( new Point().SetX( -77.05786152 ).SetY( 38.87261877 ) );

      List<Point> tempList2 = new List<Point>();

      tempList2.Add( new Point().SetX( -77.05579215 ).SetY( 38.87026286 ) );
      tempList2.Add( new Point().SetX( -77.05491238 ).SetY( 38.87087264 ) );
      tempList2.Add( new Point().SetX( -77.05544882 ).SetY( 38.87170794 ) );
      tempList2.Add( new Point().SetX( -77.05669337 ).SetY( 38.87156594 ) );
      tempList2.Add( new Point().SetX( -77.05684357 ).SetY( 38.87072228 ) );
      tempList2.Add( new Point().SetX( -77.05579215 ).SetY( 38.87026286 ) );

      LineString tempLines = new LineString( tempList2 );
      List<LineString> lines = new List<LineString>();
      lines.Add( tempLines );


      Polygon poly = new Polygon( tempList, lines );
      pers.Add( "PolyValue", poly );

      pers = Backendless.Data.Of( "GeoData" ).Save( pers );
      Dictionary<String, Object> result = Backendless.Data.Of( "GeoData" ).FindById( (String) pers[ "objectId" ] );

      Assert.Equal( (Polygon) result[ "PolyValue" ], poly );
    }
    [Fact]
    public void PointWKTEquals()
    {
      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POINT(30.05 10.1)";

      Point point = (Point) wkt.Read( StrCoordinates );

      Assert.Equal( point.AsWKT(), StrCoordinates );
    }
    [Fact]
    public void LineStringWKTEquals()
    {
      WKTParser wkt = new WKTParser();
      String StrCoordinates = "LINESTRING(5 10.2,3.05 8.6,2.04 11.006)";

      LineString line = (LineString) wkt.Read( StrCoordinates );

      Assert.Equal( line.AsWKT(), StrCoordinates );
    }
    [Fact]
    public void PolygonWKTEquals()
    {
      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POLYGON((-77.05786152 38.87261877,-77.0546978 38.87296123,-77.05317431 38.87061405," +
      "-77.0555883 38.86882611,-77.05847435 38.87002898,-77.05786152 38.87261877),(-77.05579215 38.87026286," +
      "-77.05491238 38.87087264,-77.05544882 38.87170794,-77.05669337 38.87156594,-77.05684357 38.87072228," +
      "-77.05579215 38.87026286))";

      Polygon poly = (Polygon) wkt.Read( StrCoordinates );
      Assert.Equal( poly.AsWKT(), StrCoordinates );
    }
  }
}