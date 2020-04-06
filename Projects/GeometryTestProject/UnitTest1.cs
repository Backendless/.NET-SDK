using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryTestProject
{
  [TestClass]
  public class UnitTest1
  {
    private const String APP_API_KEY = "B5D20616-5565-2674-FF73-C5CAC72BD200";
    private const String DOTNET_API_KEY = "18BF3443-B8A8-48E1-90ED-2783F9AF2D40";

    [AssemblyInitialize]
    public static void AssemblyInit( TestContext context )
    {
      Backendless.URL = "http://api.backendless.com";
      Backendless.InitApp( APP_API_KEY, DOTNET_API_KEY );
    }
    [TestMethod]
    public void TestMethodPoint()
    {
      WKTParser wkt = new WKTParser();

      Geometry geometry = wkt.Read( "POINT (30.05 10.1)" );
      Type t = typeof( Point );
      Assert.IsInstanceOfType( geometry, t, "Type is not a \"Point\" class" );

      Point point = ( Point )geometry;

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
      LineString line = ( LineString )geometry;

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

      Polygon polygon = ( Polygon )geometry;

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
      LineString line = ( LineString )geometry;

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
      Polygon polygon = ( Polygon )geometry;

      Assert.AreEqual( polygon, poly, "Object \"Polygon\" is not equal actual object" );
    }

    [TestMethod]
    public void TestReceiveGeo()
    {
      Dictionary<string, object> result = Backendless.Data.Of( "Order" ).FindFirst();
      String StrCoordinates = "POINT(40.41 -3.706)";

      Point point = ( Point )result["Dot"];
      Assert.AreEqual( point.AsWKT(), StrCoordinates, "Expected object Point VKT is not equal to the current object" );
    }
    [TestMethod]
    public void TestPointSave()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();
      pers.Add( "PersonName", "Person name" );
      pers.Add( "pickUpLocation", new Point().SetX( 30.05 ).SetY( 10.1 ) );

      pers = Backendless.Data.Of( "Person" ).Save( pers );
      Dictionary<string, object> result = Backendless.Data.Of( "Person" ).FindById( ( String )pers["objectId"] );
      Assert.AreEqual( ( Point )result["pickUpLocation"], new Point().SetX( 30.05 ).SetY( 10.1 ), "Saved Point object equal to received" );
    }
    [TestMethod]
    public void TestLineStringSave()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();

      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      pers.Add( "LineValue", finalLine );

      pers = Backendless.Data.Of( "Person" ).Save( pers );
      Dictionary<string, object> result = Backendless.Data.Of( "Person" ).FindById( ( String )pers["objectId"] );
      Assert.AreEqual( ( LineString )result["LineValue"], finalLine, "Saved LineString object equal to received" );
    }
    [TestMethod]
    public void TestPolygonSave()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();

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

      pers = Backendless.Data.Of( "Person" ).Save( pers );
      Dictionary<string, object> result = Backendless.Data.Of( "Person" ).FindById( ( String )pers["objectId"] );
      Assert.AreEqual( ( Polygon )result["PolyValue"], poly, "Saved Polygon object equal to received" );
    }
    [TestMethod]
    public void PointWKTEquals()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POINT(30.05 10.1)";

      Point point = ( Point )wkt.Read( StrCoordinates );

      Assert.AreEqual( point.AsWKT(), StrCoordinates, "Point WKT data are not equals" );

      pers.Add( "pickUpLocation", point );
      Backendless.Data.Of( "person" ).Save( pers );
    }
    [TestMethod]
    public void LineStringWKTEquals()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "LINESTRING(5 10.2,3.05 8.6,2.04 11.006)";

      LineString line = ( LineString )wkt.Read( StrCoordinates );

      Assert.AreEqual( line.AsWKT(), StrCoordinates, "LineString WKT data are not equals" );

      pers.Add( "LineValue", line );
      Backendless.Data.Of( "Person" ).Save( pers );
    }
    [TestMethod]
    public void PolygonWKTEquals()
    {
      Dictionary<string, object> pers = new Dictionary<string, object>();

      WKTParser wkt = new WKTParser();
      String StrCoordinates = "POLYGON((-77.05786152 38.87261877,-77.0546978 38.87296123,-77.05317431 38.87061405," +
      "-77.0555883 38.86882611,-77.05847435 38.87002898,-77.05786152 38.87261877),(-77.05579215 38.87026286," +
      "-77.05491238 38.87087264,-77.05544882 38.87170794,-77.05669337 38.87156594,-77.05684357 38.87072228," +
      "-77.05579215 38.87026286))";

      Polygon poly = ( Polygon )wkt.Read( StrCoordinates );
      Assert.AreEqual( poly.AsWKT(), StrCoordinates, "Polygon WKT data are not equals" );

      pers.Add( "PolyValue", poly );
      Backendless.Data.Of( "Person" ).Save( pers );
    }
    [TestMethod]
    public void PullAndComapreGeoObjects()
    {
      Dictionary<string, object> pers = Backendless.Data.Of( "GeoObject" ).FindFirst();

      WKTParser wkt = new WKTParser();

      Point point = ( Point )wkt.Read( "POINT (40.41 -3.706)" );
      Geometry geometry = wkt.Read( "POINT (10.2 48.5)" );
      LineString line = ( LineString )wkt.Read( "LINESTRING (30.1 10.05, 30.2 10.04)" );
      Polygon poly = ( Polygon )wkt.Read( "POLYGON((-77.05786152 38.87261877,-77.0546978 38.87296123,-77.05317431 38.87061405," +
      "-77.0555883 38.86882611,-77.05847435 38.87002898,-77.05786152 38.87261877),(-77.05579215 38.87026286," +
      "-77.05491238 38.87087264,-77.05544882 38.87170794,-77.05669337 38.87156594,-77.05684357 38.87072228," +
      "-77.05579215 38.87026286))" );

      Assert.AreEqual( pers["PointCol"], point, "Point WKT data are not equals" );
      Assert.AreEqual( pers["GeometryCol"], geometry, "Geometry(Point) WKT data are not equals" );
      Assert.AreEqual( pers["LineStringCol"], line, "LineString WKT data are not equals" );
      Assert.AreEqual( pers["PolygonCol"], poly, "Polygon WKT data are not equals" );
    }

    [TestMethod]
    public void EP1ExcludePropetiesEP()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );
    }

    [TestMethod]
    public void EP2ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperties( "trim(name)" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindFirst( queryBuilder );
    }

    [TestMethod]
    public void EP3ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindLast( queryBuilder );
    }

    [TestMethod]
    public void EP4ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindById( "52095A11-F700-948C-FFE6-196F8F177E00" );
    }

    [TestMethod]
    public void EP5ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );
    }

    [TestMethod]
    public void EP6ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindFirst( queryBuilder );
    }

    [TestMethod]
    public void EP7ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindLast( queryBuilder );
    }

    [TestMethod]
    public void EP8ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindById( "52095A11-F700-948C-FFE6-196F8F177E00" );
    }

    [TestMethod]
    public void EP9ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress", "TIME(created)" );
      queryBuilder.ExcludeProperties( "name", "location" );
    }

    [TestMethod]
    public void EP10ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress", "TIME(created)" );
      queryBuilder.ExcludeProperties( "name", "location" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindFirst( queryBuilder );
    }

    [TestMethod]
    public void EP11ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress", "TIME(created)" );
      queryBuilder.ExcludeProperties( "name", "location" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindLast( queryBuilder );
    }

    [TestMethod]
    public void EP12ExcludeProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress", "TIME(created)" );
      queryBuilder.ExcludeProperties( "name", "location" );

      Dictionary<string, object> res = Backendless.Data.Of( "A" ).FindById( "52095A11-F700-948C-FFE6-196F8F177E00" );
    }
    [TestMethod]
    public void TestAddAllProperties()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperty( "trim(name)" );
      queryBuilder.ExcludeProperty( "name" );
      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );
    }
  }
}
