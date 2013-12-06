using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackendlessAPI.Geo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.GeoService.AsyncTests
{
  [TestClass]
  public class RetrievingCategoriesTest: TestsFrame
  {
    [TestMethod]
  public void TestRetrieveCategoriesList() 
  {
      RunAndAwait( () => Backendless.Geo.GetCategories(new ResponseCallback<List<GeoCategory>>( this )
        {
          ResponseHandler = geoCategories =>
            {
              Assert.IsNotNull("Server returned a null list");
              Assert.IsTrue(geoCategories.Count != 0, "Server returned an empty list");

              foreach (GeoCategory geoCategory in geoCategories)
              {
                Assert.IsNotNull(geoCategory.Id, "Server returned a category with null id");
                Assert.IsNotNull(geoCategory.Name, "Server returned a category with null name");
              }

              CountDown();
            }
        }) );
  }
  }
}
