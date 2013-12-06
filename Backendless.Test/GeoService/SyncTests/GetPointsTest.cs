using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.SyncTests
{
  [TestClass]
  public class GetPointsTest : TestsFrame
  {
    [TestMethod]
    public void TestGetPointsForRectangle()
    {
      double startingLat = 10;
      double startingLong = 10;
      int maxPoints = 10;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat + 1, startingLong - 1, startingLat - 1,
                                                              startingLong + maxPoints + 1 );
      GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
    }

    [TestMethod]
    public void TestGetPointsByMetadata()
    {
      double startingLat = 20;
      double startingLong = 10;
      int maxPoints = 10;
      SetDefinedCategory( GetRandomCategory() );

      Dictionary<string, string> meta = new Dictionary<string, string>();
      meta.Add( "object_type_sync", "office" );

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, new BackendlessGeoQuery( meta ) );
    }

    [TestMethod]
    public void TestGetPointsWithNegativeOffset()
    {
      try
      {
        Backendless.Geo.GetPoints( new BackendlessGeoQuery( 50, 50, 10, -10 ) );

        Assert.Fail( "Client send a query with negative offset" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( ExceptionMessage.WRONG_OFFSET, e );
      }
    }

    [TestMethod]
    public void TestGetPointsWithBothRectAndRadiusQuery()
    {
      try
      {
        BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( 10d, 10d, 20d, 20d );
        geoQuery.Radius = 10d;
        geoQuery.Latitude = 10d;
        geoQuery.Longitude = 10d;

        Backendless.Geo.GetPoints( geoQuery );

        Assert.Fail( "Client send a query with both rectangle and radius search query" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( ExceptionMessage.INCONSISTENT_GEO_QUERY, e );
      }
    }

    [TestMethod]
    public void TestGetPointsForCategory()
    {
      double startingLat = 30;
      double startingLong = 10;
      int maxPoints = 10;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta,
                             new BackendlessGeoQuery( GetDefinedCategories() ) );
    }

    [TestMethod]
    public void TestGetPointsForMultipleCategories()
    {
      double startingLat = 80;
      double startingLong = 160;
      int maxPoints = 10;
      SetDefinedCategories( GetRandomCategoriesList( 10 ) );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta,
                             new BackendlessGeoQuery( GetDefinedCategories() ) );
    }

    [TestMethod]
    public void TestGetPointsWithoutMetadata()
    {
      double startingLat = 50;
      double startingLong = 10;
      int maxPoints = 10;
      Dictionary<string, string> meta = GetRandomSimpleMetadata();
      SetDefinedCategory( GetRandomCategory() );

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      meta = null;
      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( GetDefinedCategories() );
      geoQuery.IncludeMeta = false;

      GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
    }

    [TestMethod]
    public void TestGetPointsWithOffsetGreaterThenPointsCount()
    {
      double startingLat = 60;
      double startingLong = 10;
      int maxPoints = 10;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta );
      }

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( GetDefinedCategories() );
      geoQuery.Offset = maxPoints*2;

      try
      {
        Backendless.Geo.GetPoints( geoQuery );
        Assert.Fail( "Server accepted request" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( 4003, e );
      }
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn10Meters()
    {
      double startingLat = 80;
      double startingLong = 10;
      int maxPoints = 10;
      double offset = 0;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        offset += METER;
        Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta );
      }

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + (offset/2), maxPoints,
                                                              Units.METERS );
      GetCollectionAndCheck( startingLat, startingLong, maxPoints, offset, meta, geoQuery );
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn1Kilometer()
    {
      double startingLat = 10;
      double startingLong = 15;
      int maxPoints = 10;
      double offset = 0;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        offset += METER*100;
        Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta );
      }

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + offset/2, 1, Units.KILOMETERS );
      GetCollectionAndCheck( startingLat, startingLong, maxPoints, offset, meta, geoQuery );
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn100Yards()
    {
      double startingLat = 10;
      double startingLong = 30;
      int maxPoints = 10;
      double offset = 0;
      SetDefinedCategory( GetRandomCategory() );
      Dictionary<string, string> meta = GetRandomSimpleMetadata();

      for( int i = 0; i < maxPoints; i++ )
      {
        offset += METER*0.914399998610112;
        Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta );
      }

      BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + offset/2, 100, Units.YARDS );
      GetCollectionAndCheck( startingLat, startingLong, maxPoints, offset, meta, geoQuery );
    }
  }
}