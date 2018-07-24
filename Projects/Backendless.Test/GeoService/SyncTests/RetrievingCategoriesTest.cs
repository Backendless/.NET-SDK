using System.Collections.Generic;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.SyncTests
{
  [TestClass]
  public class RetrievingCategoriesTest : TestsFrame
  {
    [TestMethod]
    public void TestRetrieveCategoriesList()
    {
      List<GeoCategory> geoCategories = Backendless.Geo.GetCategories();

      Assert.IsNotNull( "Server returned a null list" );
      Assert.IsTrue( geoCategories.Count != 0, "Server returned an empty list" );

      foreach( GeoCategory geoCategory in geoCategories )
      {
        Assert.IsNotNull( geoCategory.Id, "Server returned a category with null id" );
        Assert.IsNotNull( geoCategory.Name, "Server returned a category with null name" );
      }
    }
  }
}