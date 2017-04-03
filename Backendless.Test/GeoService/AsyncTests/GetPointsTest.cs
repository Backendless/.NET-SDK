using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.AsyncTests
{
  [TestClass]
  public class GetPointsTest : TestsFrame
  {
    [TestMethod]
    public void TestGetPointsForRectangle()
    {
      RunAndAwait( () =>
        {
          double startingLat = 10;
          double startingLong = 10;
          int maxPoints = 10;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          var geoQuery = new BackendlessGeoQuery( startingLat + 1, startingLong - 1, startingLat - 1,
                                                  startingLong + maxPoints + 1 );
          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
        } );
    }

    [TestMethod]
    public void TestGetPointsByMetadata()
    {
      RunAndAwait( () =>
        {
          double startingLat = 20;
          double startingLong = 10;
          int maxPoints = 10;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = new Dictionary<string, string>();
          meta.Add( "object_type_sync", "office" );
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, new BackendlessGeoQuery(meta) );
        } );
    }

    [TestMethod]
    public void TestGetPointsWithNegativeOffset()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.GetPoints( new BackendlessGeoQuery( 50, 50, 10, -10 ),
                                   new AsyncCallback<IList<GeoPoint>>(
                                     response => FailCountDownWith( "Client send a query with negative offset" ),
                                     fault => CheckErrorCode( ExceptionMessage.WRONG_OFFSET, fault ) ) ) );
    }

    [TestMethod]
    public void TestGetPointsWithBothRectAndRadiusQuery()
    {
      RunAndAwait( () =>
        {
          var geoQuery = new BackendlessGeoQuery( 10, 10, 20, 20 ) {Radius = 10, Latitude = 10, Longitude = 10, Units = Units.KILOMETERS};
          Backendless.Geo.GetPoints( geoQuery,
                                     new AsyncCallback<IList<GeoPoint>>(
                                       response =>
                                       FailCountDownWith(
                                         "Client send a query with both rectangle and radius search query" ),
                                       fault => CheckErrorCode( ExceptionMessage.INCONSISTENT_GEO_QUERY, fault ) ) );
        } );
    }

    [TestMethod]
    public void TestGetPointsForCategory()
    {
      RunAndAwait( () =>
        {
          double startingLat = 30;
          double startingLong = 10;
          int maxPoints = 10;
          SetDefinedCategory( GetRandomCategory() );
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), new Dictionary<string, object>(),
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, null,
                                 new BackendlessGeoQuery( GetDefinedCategories() ) );
        } );
    }

    [TestMethod]
    public void TestGetPointsForMultipleCategories()
    {
      RunAndAwait( () =>
        {
          double startingLat = 80;
          double startingLong = 160;
          int maxPoints = 10;
          SetDefinedCategories( GetRandomCategoriesList( 10 ) );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta,
                                 new BackendlessGeoQuery( GetDefinedCategories() ) );
        } );
    }

    [TestMethod]
    public void TestGetPointsWithoutMetadata()
    {
      RunAndAwait( () =>
        {
          double startingLat = 50;
          double startingLong = 10;
          int maxPoints = 10;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          meta = null;
          var geoQuery = new BackendlessGeoQuery( GetDefinedCategories() ) {IncludeMeta = false};
          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
        } );
    }

    [TestMethod]
    public void TestGetPointsWithOffsetGreaterThenPointsCount()
    {
      RunAndAwait( () =>
        {
          double startingLat = 60;
          double startingLong = -60;
          int maxPoints = 1;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            Backendless.Geo.SavePoint( startingLat, startingLong + i, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          var geoQuery = new BackendlessGeoQuery( GetDefinedCategories() ) {Offset = maxPoints*2};
          Backendless.Geo.GetPoints( geoQuery,
                                     new AsyncCallback<IList<GeoPoint>>(
                                       response => FailCountDownWith( "Server accepted request" ),
                                       fault => CheckErrorCode( 4003, fault ) ) );
        } );
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn10Meters()
    {
      RunAndAwait( () =>
        {
          double startingLat = 80;
          double startingLong = 10;
          int maxPoints = 10;
          double offset = 0;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            offset += METER;
            Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + (offset/2), maxPoints,
                                                                  Units.METERS );
          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
        } );
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn1Kilometer()
    {
      RunAndAwait( () =>
        {
          double startingLat = 10;
          double startingLong = 15;
          int maxPoints = 10;
          double offset = 0;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            offset += METER*100;
            Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + offset/2, 1,
                                                                  Units.KILOMETERS );
          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
        } );
    }

    [TestMethod]
    public void TestGetPointsByRadiusIn100Yards()
    {
      RunAndAwait( () =>
        {
          double startingLat = 10;
          double startingLong = 15;
          int maxPoints = 10;
          double offset = 0;
          SetDefinedCategory( GetRandomCategory() );
          Dictionary<String, String> meta = GetRandomSimpleMetadata();
          CountdownEvent latch = new CountdownEvent( maxPoints );

          for( int i = 0; i < maxPoints; i++ )
          {
            offset += METER*0.914399998610112;
            Backendless.Geo.SavePoint( startingLat, startingLong + offset, GetDefinedCategories(), meta,
                                       new AsyncCallback<GeoPoint>( response => latch.Signal(), fault =>
                                         {
                                           for( int j = 0; j < latch.CurrentCount; j++ )
                                             latch.Signal();
                                         } ) );
          }
          latch.Wait();

          BackendlessGeoQuery geoQuery = new BackendlessGeoQuery( startingLat, startingLong + offset/2, 100, Units.YARDS );
          GetCollectionAndCheck( startingLat, startingLong, maxPoints, maxPoints, meta, geoQuery );
        } );
    }
  }
}