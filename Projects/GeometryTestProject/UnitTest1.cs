using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using System;
using System.Collections.Generic;

namespace GeometryTestProject
{
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
      list.Add( new Point().SetX(5.0).SetY(10.2));
      list.Add( new Point().SetX( 3.05 ).SetY( 8.6 ) );
      list.Add( new Point().SetX(2.04).SetY(11.006) );

      LineString line = ( LineString )geometry;

      Assert.AreEqual( line.GetPoints(), list , "Points was not Equal" );
    }
    [TestMethod]
    public void TestMethodPolygon()
    {
      WKTParser wkt = new WKTParser();
      Geometry geometry = wkt.Read( "POLYGON((1.01 1.02, 2.01 2.02, 3.01 3.02, 1.05 2.02), (1.012 1.015, 1.5 1.03, 1.05 1.06))" );
      Type t = typeof( Polygon );
      Assert.IsInstanceOfType( geometry, t, "Type is not equal \"Plygon\"" );
    }
  }
}
