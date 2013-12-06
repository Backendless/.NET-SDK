using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BackendlessAPI.Property;

namespace BackendlessAPI.Test
{

    [TestClass()]
    public class AsyncGeoServiceTest
    {


        private double METERS = 0.00001d;
        private TestContext testContextInstance;
        GeoService target = new GeoService();
        Random random = new Random((int)DateTime.Now.Ticks);
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        #endregion
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string applicationId = "validApp-Ids0-0000-0000-000000000000"; // TODO: Initialize to an appropriate value
            string secretKey = "validSec-retK-eys0-0000-000000000000"; // TODO: Initialize to an appropriate value
            string version = "v1"; // TODO: Initialize to an appropriate value
            Backendless.InitApp(applicationId, secretKey, version);
        }

        /// <summary>
        ///A test for AddCategory
        ///</summary>
        [TestMethod()]
        public void AddNullCategoryTest()
        {
            try
            {
                AsyncCallback<GeoCategory> callback = new AsyncCallback<GeoCategory>(
                u =>
                {
                    
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.AddCategory(null, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void AddEmptyCategoryTest()
        {
            try
            {
                AsyncCallback<GeoCategory> callback = new AsyncCallback<GeoCategory>(
                u =>
                {
                    
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.AddCategory(string.Empty, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void DeleteNullCategoryTest()
        {
            try
            {
                AsyncCallback<Boolean> callback = new AsyncCallback<Boolean>(
                u =>
                {
                    Assert.IsTrue(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                string categoryName = null;
                target.DeleteCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void DeleteEmptyCategoryTest()
        {
            try
            {
                AsyncCallback<Boolean> callback = new AsyncCallback<Boolean>(
                u =>
                {
                    Assert.IsTrue(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                string categoryName = string.Empty;
                target.DeleteCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void AddDefaultCategoryTest()
        {
            try
            {
                AsyncCallback<GeoCategory> callback = new AsyncCallback<GeoCategory>(
                u =>
                {
                    Assert.AreEqual("Default", u.Name);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                string categoryName = "Default";
                target.AddCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void DeleteDefaultCategoryTest()
        {
            try
            {
                AsyncCallback<Boolean> callback = new AsyncCallback<Boolean>(
                u =>
                {
                    Assert.IsTrue(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                string categoryName ="Default";
                target.DeleteCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void AddCategoryTest()
        {
            try
            {
                string categoryName = "category_test";
                AsyncCallback<GeoCategory> callback = new AsyncCallback<GeoCategory>(
                u =>
                {
                    Assert.AreEqual(categoryName, u.Name);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                
                target.AddCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void AddCategoryTwiceTest()
        {
            try
            {
                string categoryName = "cate"+random.Next();
                AsyncCallback<GeoCategory> callback2 = new AsyncCallback<GeoCategory>(
                u =>
                {
                    AsyncCallback<GeoCategory> callback1 = new AsyncCallback<GeoCategory>(
                    r =>
                    {
                        Assert.AreEqual(r.ObjectId, u.ObjectId);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    target.AddCategory(categoryName, callback1);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                
                target.AddCategory(categoryName, callback2);
                
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void RemoveCategoryTest()
        {
            try
            {
                string categoryName = "cate" + random.Next();
                AsyncCallback<bool> deleteCallback = new AsyncCallback<bool>(
                u =>
                {
                    Assert.IsTrue(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                AsyncCallback<GeoCategory> saveCallback = new AsyncCallback<GeoCategory>(
                u =>
                {
                    target.DeleteCategory(u.Name,deleteCallback);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.AddCategory(categoryName, saveCallback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void RemoveNonExistingCategoryTest()
        {
            try
            {
                string categoryName = "cate" + random.Next();
                AsyncCallback<Boolean> callback = new AsyncCallback<Boolean>(
                u =>
                {
                    Assert.IsTrue(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.DeleteCategory(categoryName, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void RetrieveListOfCategoryTest()
        {
            try
            {
                AsyncCallback<List<GeoCategory>> callback = new AsyncCallback<List<GeoCategory>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.GetCategories(callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
          }
        [TestMethod()]
        public void AddPointIntoEmptyCategoryTest()
        {
            try
            {
                AsyncCallback<GeoPoint> callback = new AsyncCallback<GeoPoint>(
                u =>
                {
                    Assert.IsTrue(u.Categories[0].Contains("Default"));
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                double latitude = 1.23;
                double longitude = 2.36;
                List<string> categoryName = null;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("aa", "aa");
                target.AddPoint(latitude, longitude, categoryName, metadata, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void AddPointWithTrueValueTest()
        {
            try
            {
                AsyncCallback<GeoPoint> callback = new AsyncCallback<GeoPoint>(
                u =>
                {
                    Assert.AreEqual(1.23, u.Latitude);
                    Assert.AreEqual(2.36, u.Longitude);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                double latitude = 1.23;
                double longitude = 2.36;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("aa", "aa");
                target.AddPoint(latitude, longitude, metadata, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        //[TestMethod()]
        //public void AddPointWithoutParametersTest()
        //{
        //}
        [TestMethod()]
        public void AddPointWithTrueMetadataTest()
        {
            try
            {
                AsyncCallback<GeoPoint> callback = new AsyncCallback<GeoPoint>(
                u =>
                {
                    Assert.AreEqual(1.23, u.Latitude);
                    Assert.AreEqual(2.36, u.Longitude);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("aa", "aa");
                double latitude = 1.23;
                double longitude = 2.36;
                List<string> categories = new List<string> {"cateName1","cateName2" };
                target.AddPoint(latitude, longitude,categories, metadata, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void AddPointAgainWithTrueMetadataTest()
        {
            try
            {
                double latitude = 1.23;
                double longitude = 2.36;
                List<string> cateName = new List<string> { "cate" + random.Next() };
                Dictionary<string, string> metadata1 = new Dictionary<string, string>();
                metadata1.Add("A", "aaa");
                metadata1.Add("B", "bbb");
                Dictionary<string, string> metadata2 = new Dictionary<string, string>();
                metadata2.Add("A", "aaa2");
                metadata2.Add("B", "bbb2");

                AsyncCallback<GeoPoint> callback = new AsyncCallback<GeoPoint>(
                u =>
                {
                    AsyncCallback<GeoPoint> callback2 = new AsyncCallback<GeoPoint>(
                    u2 =>
                    {
                         Assert.AreEqual(u.ObjectId, u2.ObjectId);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    target.AddPoint(latitude, longitude, cateName, metadata2,callback2);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.AddPoint(latitude, longitude, cateName, metadata1,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void AddPointToMultipleCategoriesTest()
        {
            try
            {
                double latitude = 11.23;
                double longitude = 22.36;
                List<string> categoryName = new List<string>();
                categoryName.Add("test" + random.Next());
                categoryName.Add("test" + random.Next());
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("aa", "aa");
                AsyncCallback<GeoPoint> callback = new AsyncCallback<GeoPoint>(
                u =>
                {
                    Assert.AreEqual(2, u.Categories.Count);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                target.AddPoint(latitude, longitude, categoryName, metadata, callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }




        [TestMethod()]
        public void GetPointsForRectCategoriesTest()
        {
            try
            {
                List<string> categories = new List<string> { "cate" + random.Next() };
                for (int i = 1; i <= 10; i++)
                {
                    double nwlat = 80.01 - 0.00001 * i;
                    double nwlon = 60.01 + 0.00001 * i;
                    Dictionary<string, string> metadata = new Dictionary<string, string>();
                    metadata.Add("TEST", "test" + i);
                    AsyncCallback<GeoPoint> saveCallback = new AsyncCallback<GeoPoint>(
                    u =>
                    {
                        Assert.IsNotNull(u);
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    target.AddPoint(nwlat, nwlon,categories, metadata,saveCallback);
                }
                
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(80.01, 60.01, 60.01, 80.01);
                geoQuery.Categories = categories;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }

        }
        [TestMethod()]
        public void SearchPointsInDefaultCategoryFor10MilesTest()
        {

            try
            {
                double lon = 10 * 1609.344 * METERS;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("A", "aaa");
                metadata.Add("B", "bbb");
                for (int i = 0; i < 10; i++)
                {
                    AsyncCallback<GeoPoint> saveCallback = new AsyncCallback<GeoPoint>(
                    u =>
                    {
                        
                    },
                    f =>
                    {
                        Assert.Fail(f.Message);
                    });
                    double lat = i * 1609.344 * METERS;
                    target.AddPoint(lat, lon, metadata, saveCallback);
                }
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(5 * 1609.344 * METERS, 5 * 1609.344 * METERS, 10, Units.MILES);
                target.GetPoints(geoQuery,callback);
                
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void SearchPointsInDefaultCategoryFor100YardsTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(1.23, 2.36, 100, Units.YARDS);
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void SearchPointsInDefaultCategoryFor1KilometerTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(1.23, 2.36, 1, Units.KILOMETERS);
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void SearchPointsInMetadataTest()
        {

            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                double latitude = 1.23;
                double longitude = 2.36;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("A", "aaa");
                target.AddPoint(latitude, longitude, metadata);
                BackendlessGeoQuery query = new BackendlessGeoQuery(metadata);
                target.GetPoints(query,callback);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }

        }
        [TestMethod()]
        public void SearchPointsInDefaultCategoryWithNegativeOffsetTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.AreEqual("4003",f.FaultCode);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(60.01, 60.01, 2, -1);
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void searchPointsInDefaultCategoryWithOffsetGreaterThanMaxPointTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.AreEqual("4003", f.FaultCode);
                });
                double lon = 10 * METERS;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("A", "aaa");
                metadata.Add("B", "bbb");
                for (int i = 0; i < 14; i++)
                {
                    double lat = i * METERS;
                    target.AddPoint(lat, lon, metadata);
                }
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(5 * METERS, 5 * METERS, 10, Units.MILES);
                geoQuery.PageSize = 1;
                geoQuery.Offset = 15;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod()]
        public void SearchPointsIncludemetadataWithNegativeOffsetTest()
        {

            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.IncludeMeta = false;
                geoQuery.Latitude = 60.01;
                geoQuery.Longitude = 60.01;
                geoQuery.Radius = 20;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 0;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }

        }
        [TestMethod()]
        public void SearchPointsInMultipleCategoriesTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Test", "Test");
                List<string> cateName = new List<string> { "cateName1", "cateName2" };
                BackendlessGeoQuery query = new BackendlessGeoQuery();
                query.Categories = cateName;
                query.Radius = 20;
                query.Units = Units.METERS;
                query.PageSize = 5;
                query.Offset = 0;
                target.GetPoints(query,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod()]
        public void GetPointsForRectangleAndRadiusTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.AreEqual("4008", f.FaultCode);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(80.01, 60.01, 60.01, 80.01);
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 1;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
        [TestMethod()]
        public void SearchPointsWithWrongLatitudeTest()
        {

            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 91;
                geoQuery.Longitude = 180;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void SearchPointsWithWrongLongitudeTest()
        {

            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 90;
                geoQuery.Longitude = 181;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void SearchPointsWithNegativePagesizeTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 30;
                geoQuery.Longitude = 60;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = -2;
                geoQuery.Offset = 1;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void SearchPointsInCategoriesTest()
        {
            try
            {
                AsyncCallback<BackendlessCollection<GeoPoint>> callback = new AsyncCallback<BackendlessCollection<GeoPoint>>(
                u =>
                {
                    Assert.IsNotNull(u);
                },
                f =>
                {
                    Assert.Fail(f.Message);
                });
                List<string> cateName = new List<string> { "cateName1", "cateName2" };
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Categories = cateName;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 0;
                target.GetPoints(geoQuery,callback);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }

    }
}
