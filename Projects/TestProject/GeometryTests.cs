using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;


namespace TestProject
{
  [TestClass]
  public class GeometryDataTypesTestClass
  {
    static HttpClient client;
    const String URL_BASE_ADRESS = "https://develop.backendless.com";
    const String Login = "";
    const String Password = "";

    [ClassInitialize]
    public static void TestGeometrySetupData( TestContext context )
    {
      try
      {
        Backendless.Data.Of( "GeoData" ).Find();
      }
      catch
      {
        const String POINT_NAME = "POINT";
        const String LINESTRING_NAME = "LINESTRING";
        const String POLYGON_NAME = "POLYGON";
        const String GEOMETRY_NAME = "GEOMETRY";

        Dictionary<String, Object> data = new Dictionary<String, Object>();
        data.Add( "GeoDataName", "Geo data name" );
        Dictionary<String, Object> deleteObject = Backendless.Data.Of( "GeoData" ).Save( data );
        Backendless.Data.Of( "GeoData" ).Remove( deleteObject );

        CreateColumn( POINT_NAME, "P1" );

        data.Add( "P1", new Point().SetX( 40.41 ).SetY( -3.706 ) );

        CreateColumn( POINT_NAME, "pickupLocation" );

        CreateColumn( LINESTRING_NAME, "LineValue" );

        List<Point> list = new List<Point>();
        list.Add( new Point().SetX( 30.1 ).SetY( 10.05 ) );
        list.Add( new Point().SetX( 30.2 ).SetY( 10.04 ) );

        LineString finalLine = new LineString( list );
        data.Add( "LineValue", finalLine );

        CreateColumn( POLYGON_NAME, "PolyValue" );

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
        data.Add( "PolyValue", poly );

        CreateColumn( GEOMETRY_NAME, "GeoValue" );

        data.Add( "GeoValue", new Point().SetX( 10.2 ).SetY( 48.5 ) );


        Backendless.Data.Of( "GeoData" ).Save( data );
      }
    }

    static String LoginAndGetToken()
    {
      HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, "https://develop.backendless.com/console/home/login" );
      request.Content = new StringContent( "{\"login\":\""+Login+"\",\"password\":\""+Password+"\"}", Encoding.UTF8, "application/json" );

      return client.SendAsync( request ).GetAwaiter().GetResult().Headers.GetValues( "auth-key" ).ToArray()[ 0 ];
    }

    static void CreateColumn( String typeName, String columnName, bool TableIsCreated = true )
    {
      client = new HttpClient();
      client.BaseAddress = new Uri( URL_BASE_ADRESS );

      String token_Auth_Key = LoginAndGetToken();

      client.DefaultRequestHeaders.Add( "auth-key", token_Auth_Key );


      HttpRequestMessage requestMessage = new HttpRequestMessage( HttpMethod.Post, "https://develop.backendless.com/"+TestInitialization.APP_API_KEY+"/console/data/tables/GeoData/columns" );

        requestMessage.Content = new StringContent( "{\"metaInfo\":{\"srsId\":4326},\"name\":\"" + columnName + "\"," +
                          "\"dataType\":\"" + typeName + "\",\"required\":false,\"unique\":false,\"indexed\":false}", Encoding.UTF8, "application/json" );

      Task.WaitAll( client.SendAsync( requestMessage ) );
    }

    [TestMethod]
    public void PullAndComapreGeoObjects()
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
      Assert.AreEqual( result[ "P1" ], point, "Point WKT data are not equals" );
      Assert.AreEqual( result[ "GeoValue" ], geometry, "Geometry(Point) WKT data are not equals" );
      Assert.AreEqual( result[ "LineValue" ], line, "LineString WKT data are not equals" );
      Assert.AreEqual( result[ "PolyValue" ], poly, "Polygon WKT data are not equals" );     
    }

    [TestMethod]
    public void TestMethodPoint()
    {
      WKTParser wkt = new WKTParser();

      Geometry geometry = wkt.Read( "POINT (30.05 10.1)" );
      Type t = typeof( Point );
      Assert.IsInstanceOfType( geometry, t, "Type is not a \"Point\" class" );

      Point point = (Point) geometry;

      Assert.AreEqual( point.GetX(), 30.05, "Point X was not equal to double 30.05" );
      Assert.AreEqual( point.GetY(), 10.1, "Point Y was not equal to double 10.1" );
    }
    [TestMethod]
    public void TestMethodLineString()
    {
      WKTParser wkt = new WKTParser();

      Geometry geometry = wkt.Read( "LINESTRING(5.0 10.2, 3.05 8.6, 2.04 11.006)" );
      Type t = typeof( LineString );
      Assert.IsInstanceOfType( geometry, t, "Type is not a \"LineString\"" );
      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      LineString line = (LineString) geometry;

      Assert.AreEqual( line, finalLine, "Points was not Equal" );
    }
    [TestMethod]
    public void TestMethodPolygon()
    {
      WKTParser wkt = new WKTParser();
      Geometry geometry = wkt.Read( "POLYGON((1.01 1.02, 2.01 2.02, 3.01 3.02, 1.05 2.02), (1.013 1.014, 1.015 1.016))" );
      Type t = typeof( Polygon );
      Assert.IsInstanceOfType( geometry, t, "Type is not equal \"Polygon\"" );

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

      Assert.AreEqual( polygon, poly, "Object \"Polygon\" is not equal actual object" );
    }
    [TestMethod]
    public void TestJSONPointMethod()
    {
      GeoJSONParser<Point> geo = new GeoJSONParser<Point>();
      Geometry geometry = geo.Read( "{ \"type\": \"Point\", \"coordinates\": [ 1.01, 1.02 ] }" );
      Type t = typeof( Point );
      Assert.IsInstanceOfType( geometry, t, "Type is not \"Point\" class" );

      Point point = ( Point )geometry;


      Assert.AreEqual( point.GetX(), 1.01, "Point X was not equal to double 1.01" );
      Assert.AreEqual( point.GetY(), 1.02, "Point Y was not equal to double 1.02" );

    }
    [TestMethod]
    public void TestJSONLineStringMethod()
    {
      GeoJSONParser<LineString> geo = new GeoJSONParser<LineString>();
      Geometry geometry = geo.Read( "{ \"type\": \"LineString\", \"coordinates\": [ [ 1.01, 1.02 ]," +
                         "[ 2.01, 2.02 ], [ 3.01, 3.02 ], [ 4.01, 4.02 ] ] }" );
      Type t = typeof( LineString );
      Assert.IsInstanceOfType( geometry, t, "Type is not \"LineString\" class" );

      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 1.01 ).SetY( 1.02 ) );
      list.Add( new Point().SetX( 2.01 ).SetY( 2.02 ) );
      list.Add( new Point().SetX( 3.01 ).SetY( 3.02 ) );
      list.Add( new Point().SetX( 4.01 ).SetY( 4.02 ) );

      LineString listLineStr = new LineString( list );
      LineString line = (LineString) geometry;

      Assert.AreEqual( line, listLineStr, "Points was not Equal" );
    }
    [TestMethod]
    public void TestJSONPolygonMethod()
    {
      GeoJSONParser<Polygon> geo = new GeoJSONParser<Polygon>();
      Geometry geometry = geo.Read( "{ \"type\": \"Polygon\", \"coordinates\": [ [ [ 1.01, 1.02 ]," +
                         "[ 2.01, 2.02 ], [ 2.02, 2.03 ], [ 1.015, 1.017 ] ], [ [ 1.013, 1.014 ], [ 1.015, 1.016 ] ] ] }" );
      Type t = typeof( Polygon );
      Assert.IsInstanceOfType( geometry, t, "Type is not \"Polygon\" class" );

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

      Assert.AreEqual( polygon, poly, "Object \"Polygon\" is not equal actual object" );
    }

    [TestMethod]
    public void TestReceiveGeo()
    {
      IList<Dictionary<String, object>> result = Backendless.Data.Of( "GeoData" ).Find();
      String StrCoordinates = "POINT(40.41 -3.706)";
      Point point = new Point();

      foreach( Dictionary<String, Object> entry in result )
        if( entry.ContainsValue( (String) "Geo data name" ) )
        {
          point = (Point) entry[ "P1" ];
          break;
        }

      Assert.AreEqual( point.AsWKT(), StrCoordinates, "Expected object Point VKT is not equal to the current object" );
    }
    [TestMethod]
    public void TestPointSave()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();

      pers.Add( "pickupLocation", new Point().SetX( 30.05 ).SetY( 10.1 ) );

      pers = Backendless.Data.Of( "GeoData" ).Save( pers );
      Dictionary<String, object> result = Backendless.Data.Of( "GeoData" ).FindById( (String) pers[ "objectId" ] );

      Assert.AreEqual( (Point)result[ "pickupLocation" ], new Point().SetX( 30.05 ).SetY( 10.1 ), "Saved Point object equal to received" );
    }
    [TestMethod]
    public void TestLineStringSave()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();
      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      pers.Add( "LineValue", finalLine );

      pers = Backendless.Data.Of( "GeoData" ).Save( pers );
      Dictionary<String, object> result = Backendless.Data.Of( "GeoData" ).FindById( (String)pers[ "objectId" ] );

      Assert.AreEqual( (LineString)result[ "LineValue" ], finalLine, "Saved LineString object equal to received" );
    }
    [TestMethod]
    public void TestPolygonSave()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();
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
      Dictionary<String, object> result = Backendless.Data.Of( "GeoData" ).FindById( (String) pers[ "objectId" ] );

      Assert.AreEqual( (Polygon) result[ "PolyValue" ], poly, "Saved Polygon object equal to received" );
    }
    [TestMethod]
    public void PointWKTEquals()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POINT(30.05 10.1)";

      Point point = (Point) wkt.Read( StrCoordinates );

      Assert.AreEqual( point.AsWKT(), StrCoordinates, "Point WKT data are not equals" );
    }
    [TestMethod]
    public void LineStringWKTEquals()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "LINESTRING(5 10.2,3.05 8.6,2.04 11.006)";

      LineString line = (LineString) wkt.Read( StrCoordinates );

      Assert.AreEqual( line.AsWKT(), StrCoordinates, "LineString WKT data are not equals" );
    }
    [TestMethod]
    public void PolygonWKTEquals()
    {
      Dictionary<String, object> pers = new Dictionary<String, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POLYGON((-77.05786152 38.87261877,-77.0546978 38.87296123,-77.05317431 38.87061405," +
      "-77.0555883 38.86882611,-77.05847435 38.87002898,-77.05786152 38.87261877),(-77.05579215 38.87026286," +
      "-77.05491238 38.87087264,-77.05544882 38.87170794,-77.05669337 38.87156594,-77.05684357 38.87072228," +
      "-77.05579215 38.87026286))";

      Polygon poly = ( Polygon )wkt.Read( StrCoordinates );
      Assert.AreEqual( poly.AsWKT(), StrCoordinates, "Polygon WKT data are not equals" );
    }
  }
}
