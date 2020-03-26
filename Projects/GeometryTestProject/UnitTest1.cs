using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryTestProject
{
  public class Person
  {
    private Point pickUpLocation;
    private Point dropOffLocation;
    private String personName;
    private String objectId;

    public String getObjectId()
    {
      return objectId;
    }

    public void setObjectId( String objectId )
    {
      this.objectId = objectId;
    }

    public Point getPickupLocation()
    {
      return pickUpLocation;
    }

    public void setPickupLocation( Point pickUpLocation )
    {
      this.pickUpLocation = pickUpLocation;
    }

    public Point getDropOffLocation()
    {
      return dropOffLocation;
    }

    public void setDropOffLocation( Point dropOffLocation )
    {
      this.dropOffLocation = dropOffLocation;
    }

    public String GetPersonName()
    {
      return personName;
    }

    public void setPersonName( String personName )
    {
      this.personName = personName;
    }
  }

  [TestClass]
  public class UnitTest1
  {
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

      bool y = polygon.Equals( poly );
      Assert.AreEqual( polygon, poly, "Object \"Polygon\" is not equal actual object" );
    }

    [TestMethod]
    public void TestReceiveGeo()
    {
      Backendless.InitApp( "B5D20616-5565-2674-FF73-C5CAC72BD200", "18BF3443-B8A8-48E1-90ED-2783F9AF2D40" );

      Dictionary<string, object> result = Backendless.Data.Of( "Person" ).FindFirst();
      Assert.IsTrue( true );
    }
    [TestMethod]
    public void TestPointSave()
    {
      Backendless.InitApp( "B5D20616-5565-2674-FF73-C5CAC72BD200", "18BF3443-B8A8-48E1-90ED-2783F9AF2D40" );

      Dictionary<string, object> pers = new Dictionary<string, object>();
      pers.Add( "PersonName", "Person name" );
      pers.Add( "pickUpLocation", new Point().SetX( 50.1 ).SetY( 30.1 ) );
      pers.Add( "dropOffLocation", new Point().SetX( 50.2 ).SetY( 30.2 ) );
      Backendless.Data.Of( "Person" ).Save( pers );
      Assert.IsTrue( true );
    }
    [TestMethod]
    public void TestLineStringSave()
    {
      Backendless.InitApp( "B5D20616-5565-2674-FF73-C5CAC72BD200", "18BF3443-B8A8-48E1-90ED-2783F9AF2D40" );
      Dictionary<string, object> pers = new Dictionary<string, object>();

      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 5.0 ).SetY( 10.2 ) );
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX( 2.04 ).SetY( 11.006 ) );

      LineString finalLine = new LineString( list );
      pers.Add( "LineValue", finalLine );

      Backendless.Data.Of( "Person" ).Save( pers );
    }
    
    [TestMethod]
    public void TestPolygonSave()
    {
      Backendless.InitApp( "B5D20616-5565-2674-FF73-C5CAC72BD200", "18BF3443-B8A8-48E1-90ED-2783F9AF2D40" );
      Dictionary<string, object> pers = new Dictionary<string, object>();

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
      pers.Add( "PolyValue", poly );

      Backendless.Data.Of( "Person" ).Save( pers );
    }
  }
}
