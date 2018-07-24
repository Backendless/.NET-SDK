using System.Collections.Generic;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.SyncTests
{
  [TestClass]
  public class AddPointTest : TestsFrame
  {
    [TestMethod]
    public void TestAddPointWithExceedingLatitude()
    {
      try
      {
        Backendless.Geo.SavePoint( 146, -36, GetRandomSimpleMetadata() );
        Assert.Fail( "Client accepted a point with exceeding latitude" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( ExceptionMessage.WRONG_LATITUDE_VALUE, e );
      }
    }

    [TestMethod]
    public void TestAddPointWithExceedingLongitude()
    {
      try
      {
        Backendless.Geo.SavePoint( -45, 235, GetRandomSimpleMetadata() );
        Assert.Fail( "Client accepted a point with exceeding longitude" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( ExceptionMessage.WRONG_LONGITUDE_VALUE, e );
      }
    }

    [TestMethod]
    public void TestAddPointWithNullCategory()
    {
      string category = null;
      double latitude = -44;
      double longtitude = -34;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<string> {category};

      GeoPoint geoPoint = Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata );

      Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
      Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                     "Server returned a geopoint with wrong category" );
      Assert.IsTrue( geoPoint.Categories.Count == 1, "Server returned a geopoint with wrong categories size" );
      Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000001, "Server returned a geopoint with wrong latitude" );
      Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000001, "Server returned a geopoint with wrong longtitude" );
      Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
      foreach (string key in metadata.Keys)
      {
        Assert.IsTrue(geoPoint.Metadata.ContainsKey(key), "Server returned a geopoint with wrong metadata");
        Assert.IsTrue(geoPoint.Metadata[key].Equals(metadata[key]), "Server returned a geopoint with wrong metadata");
      }
    }

    [TestMethod]
    public void TestAddPointWithEmptyCategory()
    {
      string category = "";
      int latitude = -43;
      int longtitude = -33;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<string> {category};

      GeoPoint geoPoint = Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata );

      Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
      Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                     "Server returned a geopoint with wrong category" );
      Assert.IsTrue( geoPoint.Categories.Count == 1, "Server returned a geopoint with wrong categories size" );
      Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000000001d, "Server returned a geopoint with wrong latitude" );
      Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000000001d, "Server returned a geopoint with wrong longtitude" );
      Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
      foreach (string key in metadata.Keys)
      {
        Assert.IsTrue(geoPoint.Metadata.ContainsKey(key), "Server returned a geopoint with wrong metadata");
        Assert.IsTrue(geoPoint.Metadata[key].Equals(metadata[key]), "Server returned a geopoint with wrong metadata");
      }
    }

    [TestMethod]
    public void TestAddPointWithNullCategoriesList()
    {
      int latitude = -42;
      int longtitude = -32;
      var metadata = GetRandomSimpleMetadata();
      List<string> categories = null;

      GeoPoint geoPoint = Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata );

      Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
      Assert.IsTrue( geoPoint.Categories.Contains( DEFAULT_CATEGORY_NAME ),
                     "Server returned a geopoint with wrong category" );
      Assert.IsTrue( geoPoint.Categories.Count == 1, "Server returned a geopoint with wrong categories size" );
      Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000000001d, "Server returned a geopoint with wrong latitude" );
      Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000000001d, "Server returned a geopoint with wrong longtitude" );
      Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
      foreach (string key in metadata.Keys)
      {
        Assert.IsTrue(geoPoint.Metadata.ContainsKey(key), "Server returned a geopoint with wrong metadata");
        Assert.IsTrue(geoPoint.Metadata[key].Equals(metadata[key]), "Server returned a geopoint with wrong metadata");
      }
    }

    [TestMethod]
    public void TestAddPointWithMetaData()
    {
      string category = GetRandomCategory();
      int latitude = -41;
      int longtitude = -31;
      var metadata = GetRandomSimpleMetadata();
      var categories = new List<string> {category};

      GeoPoint geoPoint = Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata );

      Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
      foreach (string cat in categories)
        Assert.IsTrue(geoPoint.Categories.Contains(cat), "Server returned a geopoint with wrong category");
      Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000000001d, "Server returned a geopoint with wrong latitude" );
      Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000000001d, "Server returned a geopoint with wrong longtitude" );
      Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
      foreach (string key in metadata.Keys)
      {
        Assert.IsTrue(geoPoint.Metadata.ContainsKey(key), "Server returned a geopoint with wrong metadata");
        Assert.IsTrue(geoPoint.Metadata[key].Equals(metadata[key]), "Server returned a geopoint with wrong metadata");
      }
    }

    [TestMethod]
    public void TestAddPointToMultipleCategories()
    {
      int latitude = -40;
      int longtitude = -30;
      var metadata = GetRandomSimpleMetadata();
      var categories = GetRandomCategoriesList( 10 );

      GeoPoint geoPoint = Backendless.Geo.SavePoint( latitude, longtitude, categories, metadata );

      Assert.IsNotNull( geoPoint, "Server returned a null geopoint" );
      foreach( string category in categories )
        Assert.IsTrue( geoPoint.Categories.Contains( category ), "Server returned a geopoint with wrong category" );
      Assert.AreEqual( latitude, geoPoint.Latitude, 0.0000000001d, "Server returned a geopoint with wrong latitude" );
      Assert.AreEqual( longtitude, geoPoint.Longitude, 0.0000000001d, "Server returned a geopoint with wrong longtitude" );
      Assert.IsNotNull( geoPoint.ObjectId, "Server returned a geopoint with null id" );
      foreach( string key in metadata.Keys )
      {
        Assert.IsTrue(geoPoint.Metadata.ContainsKey(key), "Server returned a geopoint with wrong metadata");
        Assert.IsTrue(geoPoint.Metadata[key].Equals(metadata[key]), "Server returned a geopoint with wrong metadata");
      }
    }

    [TestMethod]
    public void TestAddNullGeoPoint()
    {
      try
      {
        Backendless.Geo.SavePoint( null );

        Assert.Fail( "Client accepted a null" );
      }
      catch( System.Exception e )
      {
        CheckErrorCode( ExceptionMessage.NULL_GEOPOINT, e );
      }
    }
  }
}