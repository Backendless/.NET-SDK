using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.AsyncTests
{
  [TestClass]
  public class AddPointTest : TestsFrame
  {
    [TestMethod]
    public void TestAddPointWithExceedingLatitude()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( 146, -36, GetRandomSimpleMetadata(),
                                   new AsyncCallback<GeoPoint>(
                                     response => FailCountDownWith( "Client accepted a point with exceeding latitude" ),
                                     fault => CheckErrorCode( ExceptionMessage.WRONG_LATITUDE_VALUE, fault ) ) ) );
    }

    [TestMethod]
    public void TestAddPointWithExceedingLongitude()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( -45, 235, GetRandomSimpleMetadata(),
                                   new AsyncCallback<GeoPoint>(
                                     response => FailCountDownWith( "Client accepted a point with exceeding longitude" ),
                                     fault => CheckErrorCode( ExceptionMessage.WRONG_LONGITUDE_VALUE, fault ) ) ) );
    }

    [TestMethod]
    public void TestAddPointWithNullCategory()
    {
      string category = null;
      double latitude = -44;
      double longtitude = -34;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<String> {category};

      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata,
                                   new ResponseCallback<GeoPoint>( this )
                                     {
                                       ResponseHandler = geoPoint =>
                                         {
                                           Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
                                           Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                                                          "Server returned a geopoint with wrong category" );
                                           Assert.IsTrue( geoPoint.Categories.Count == 1,
                                                          "Server returned a geopoint with wrong categories size" );
                                           Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001,
                                                            "Server returned a geopoint with wrong latitude" );
                                           Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001,
                                                            "Server returned a geopoint with wrong longtitude" );
                                           Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
                                           foreach( KeyValuePair<string, string> keyValuePair in metadata )
                                           {
                                             Assert.IsTrue( geoPoint.Metadata.ContainsKey( keyValuePair.Key ),
                                                            "Server returned a geopoint with wrong metadata" );
                                             Assert.IsTrue(
                                               geoPoint.Metadata[keyValuePair.Key].Equals( keyValuePair.Value ),
                                               "Server returned a geopoint with wrong metadata" );
                                           }
                                           CountDown();
                                         }
                                     } ) );
    }

    [TestMethod]
    public void TestAddPointWithEmptyCategory()
    {
      string category = "";
      double latitude = -43;
      double longtitude = -33;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<String> {category};

      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata,
                                   new ResponseCallback<GeoPoint>( this )
                                     {
                                       ResponseHandler = geoPoint =>
                                         {
                                           Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
                                           Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                                                          "Server returned a geopoint with wrong category" );
                                           Assert.IsTrue( geoPoint.Categories.Count == 1,
                                                          "Server returned a geopoint with wrong categories size" );
                                           Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001,
                                                            "Server returned a geopoint with wrong latitude" );
                                           Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001,
                                                            "Server returned a geopoint with wrong longtitude" );
                                           Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
                                           foreach( KeyValuePair<string, string> keyValuePair in metadata )
                                           {
                                             Assert.IsTrue( geoPoint.Metadata.ContainsKey( keyValuePair.Key ),
                                                            "Server returned a geopoint with wrong metadata" );
                                             Assert.IsTrue(
                                               geoPoint.Metadata[keyValuePair.Key].Equals( keyValuePair.Value ),
                                               "Server returned a geopoint with wrong metadata" );
                                           }
                                           CountDown();
                                         }
                                     } ) );
    }

    [TestMethod]
    public void TestAddPointWithNullCategoriesList()
    {
      double latitude = -42;
      double longtitude = -32;
      var metadata = GetRandomSimpleMetadata();
      List<String> categories = null;

      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata,
                                   new ResponseCallback<GeoPoint>( this )
                                     {
                                       ResponseHandler = geoPoint =>
                                         {
                                           Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
                                           Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                                                          "Server returned a geopoint with wrong category" );
                                           Assert.IsTrue( geoPoint.Categories.Count == 1,
                                                          "Server returned a geopoint with wrong categories size" );
                                           Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001,
                                                            "Server returned a geopoint with wrong latitude" );
                                           Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001,
                                                            "Server returned a geopoint with wrong longtitude" );
                                           Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
                                           foreach( KeyValuePair<string, string> keyValuePair in metadata )
                                           {
                                             Assert.IsTrue( geoPoint.Metadata.ContainsKey( keyValuePair.Key ),
                                                            "Server returned a geopoint with wrong metadata" );
                                             Assert.IsTrue(
                                               geoPoint.Metadata[keyValuePair.Key].Equals( keyValuePair.Value ),
                                               "Server returned a geopoint with wrong metadata" );
                                           }
                                           CountDown();
                                         }
                                     } ) );
    }

    [TestMethod]
    public void TestAddPointWithMetaData()
    {
      double latitude = -41;
      double longtitude = -31;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<string>() {GetRandomCategory()};

      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata,
                                   new ResponseCallback<GeoPoint>( this )
                                     {
                                       ResponseHandler = geoPoint =>
                                         {
                                           Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
                                           foreach( string category in categories )
                                             Assert.IsTrue( geoPoint.Categories.Contains( category ),
                                                            "Server returned a geopoint with wrong categories" );
                                           Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001,
                                                            "Server returned a geopoint with wrong latitude" );
                                           Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001,
                                                            "Server returned a geopoint with wrong longtitude" );
                                           Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
                                           foreach( KeyValuePair<string, string> keyValuePair in metadata )
                                           {
                                             Assert.IsTrue( geoPoint.Metadata.ContainsKey( keyValuePair.Key ),
                                                            "Server returned a geopoint with wrong metadata" );
                                             Assert.IsTrue(
                                               geoPoint.Metadata[keyValuePair.Key].Equals( keyValuePair.Value ),
                                               "Server returned a geopoint with wrong metadata" );
                                           }
                                           CountDown();
                                         }
                                     } ) );
    }

    [TestMethod]
    public void TestAddPointToMultipleCategories()
    {
      double latitude = -40;
      double longtitude = -30;
      var metadata = GetRandomSimpleMetadata();
      var categories = GetRandomCategoriesList( 10 );

      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata,
                                   new ResponseCallback<GeoPoint>( this )
                                     {
                                       ResponseHandler = geoPoint =>
                                         {
                                           Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
                                           foreach( string category in categories )
                                             Assert.IsTrue( geoPoint.Categories.Contains( category ),
                                                            "Server returned a geopoint with wrong categories" );
                                           Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001,
                                                            "Server returned a geopoint with wrong latitude" );
                                           Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001,
                                                            "Server returned a geopoint with wrong longtitude" );
                                           Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
                                           foreach( KeyValuePair<string, string> keyValuePair in metadata )
                                           {
                                             Assert.IsTrue( geoPoint.Metadata.ContainsKey( keyValuePair.Key ),
                                                            "Server returned a geopoint with wrong metadata" );
                                             Assert.IsTrue(
                                               geoPoint.Metadata[keyValuePair.Key].Equals( keyValuePair.Value ),
                                               "Server returned a geopoint with wrong metadata" );
                                           }
                                           CountDown();
                                         }
                                     } ) );
    }

    [TestMethod]
    public void TestAddNullGeoPoint()
    {
      RunAndAwait(
        () =>
        Backendless.Geo.SavePoint( null,
                                   new AsyncCallback<GeoPoint>(
                                     response => FailCountDownWith( "Client accepted a null" ),
                                     fault => CheckErrorCode( ExceptionMessage.NULL_GEOPOINT, fault ) ) ) );
    }
  }
}