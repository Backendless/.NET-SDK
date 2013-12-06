using BackendlessAPI;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BackendlessAPI.Test
{


    /// <summary>
    ///This is a test class for GeoServiceGeoServiceTests and is intended
    ///to contain all GeoServiceGeoServiceTests Unit Tests
    ///</summary>
    [TestClass()]
    public class GeoServiceGeoServiceTests
    {
       

        private double METERS = 0.00001d;
        private TestContext testContextInstance;
        Random random = new Random((int)DateTime.Now.Ticks);
        GeoService target = new GeoService();
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
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
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
            string categoryName = null;
            try
            {
                var cate=target.AddCategory(categoryName);
                Assert.IsNotNull(cate, "Category name can not be null or empty");
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void AddEmptyCategoryTest()
        {
            string categoryName = string.Empty;
            try
            {
                var cate=target.AddCategory(categoryName);
                Assert.IsNotNull(cate, "Category name can not be null or empty");
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }

        }
        [TestMethod()]
        public void DeleteNullCategoryTest()
        {
            string categoryName = null;
            try
            {
                var result=target.DeleteCategory(categoryName);
                Assert.IsTrue(result);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }


        }
        [TestMethod()]
        public void DeleteEmptyCategoryTest()
        {
            string categoryName = string.Empty;
            try
            {
                var result=target.DeleteCategory(categoryName);
                Assert.IsTrue(result);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void AddDefaultCategoryTest()
        {
            string categoryName = "Default";
            try
            {
                var cate=target.AddCategory(categoryName);
                Assert.IsNotNull(cate);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void DeleteDefaultCategoryTest()
        {
            string categoryName = "Default";
            try
            {
                var result=target.DeleteCategory(categoryName);
                Assert.IsTrue(result);
            }
            catch (BackendlessException e)
            {
                Assert.AreEqual("N/A", e.Code);
            }
        }
        [TestMethod()]
        public void AddCategoryTest()
        {
            string categoryName = "test category test_start_timestamp";
            try
            {
                var result=target.AddCategory(categoryName);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }


        }
        [TestMethod()]
        public void AddCategoryTwiceTest()
        {
            string categoryName = "cate test";
            try
            {
                var result1=target.AddCategory(categoryName);
                var result2=target.AddCategory(categoryName);
                Assert.AreEqual(result1.ObjectId, result2.ObjectId);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void RemoveCategoryTest()
        {
            string categoryName = "cate"+random.Next();
            try
            {
                target.AddCategory(categoryName);
                var result=target.DeleteCategory(categoryName);
                Assert.IsTrue(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }

        }
        [TestMethod()]
        public void RemoveNonExistingCategoryTest()
        {
            string categoryName = "cate" + random.Next();
            try
            {
                var result = target.DeleteCategory(categoryName);
                Assert.IsTrue(result);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("4001", ex.Code);
            }

        }
        [TestMethod()]
        public void RetrieveListOfCategoryTest()
        {
            try
            {
                string categoryName = "cate" + random.Next();
                target.AddCategory(categoryName);
                var result = target.GetCategories();
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void addPointWithWrongValueTest()
        {
            double latitude = 91;
            double longitude = 181;
            List<string> cateName = new List<string> { "Default" };
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("A", "aaa");
            metadata.Add("B", "bbb");
            try
            {
                var result=target.AddPoint(latitude, longitude,cateName, metadata);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }





        [TestMethod()]
        public void AddPointIntoNullCategoryTest()
        {
            double latitude = 1.23;
            double longitude = 2.36;
            List<string> categoryName = null;
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("A", "aaa");
            try
            {
                var result=target.AddPoint(latitude, longitude, categoryName, metadata);
                Assert.IsTrue(result.Categories[0].Contains("Default"));
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void addPointIntoEmptyCategoryTest()
        {
            double latitude = 1.23;
            double longitude = 2.36;
            List<string> categoryName = new List<string> {"" };
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("A", "aaa");
            try
            {
                var result = target.AddPoint(latitude, longitude, categoryName, metadata);
                Assert.IsTrue(result.Categories[0].Contains("Default"));
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        [TestMethod()]
        public void AddPointWithTrueValueTest()
        {
            double latitude = 1.23;
            double longitude = 2.36;
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("aa", "aa");

            try
            {
                var result=target.AddPoint(latitude, longitude, metadata);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        //[TestMethod()]
        //public void AddPointWithoutParametersTest()
        //{
        //    //GeoPoint actual = new GeoPoint();
        //    //try
        //    //{
        //    //    var result = target.AddPoint(actual);
        //    //    Assert.IsNotNull(result);
        //    //}
        //    //catch (BackendlessException e)
        //    //{
        //    //    Assert.Fail(e.Message);
        //    //}
        //}
        [TestMethod()]
        public void AddPointWithTrueMetadataTest()
        {
            double latitude = 1.23;
            double longitude = 2.36;
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("A", "aaa");
            metadata.Add("B", "bbb");
            try
            {
                var result=target.AddPoint(latitude, longitude, metadata);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
        //[TestMethod()]
        //public void AddPointAgainWithTrueMetadataTest()
        //{
        //    double latitude = 1.23;
        //    double longitude = 2.36;
        //    List<string> cateName = new List<string> { "cate" + random.Next() };
        //    Dictionary<string, string> metadata1 = new Dictionary<string, string>();
        //    metadata1.Add("A", "aaa");
        //    metadata1.Add("B", "bbb");
        //    Dictionary<string, string> metadata2 = new Dictionary<string, string>();
        //    metadata2.Add("A", "aaa2");
        //    metadata2.Add("B", "bbb2");
        //    try
        //    {
        //        var result1 = target.AddPoint(latitude, longitude,cateName, metadata1);
        //        var result2 = target.AddPoint(latitude, longitude,cateName, metadata2);
        //        Assert.AreEqual(result1.ObjectId, result2.ObjectId);
        //    }
        //    catch (BackendlessException e)
        //    {
        //        Assert.Fail(e.Message);
        //    }
        //}
        [TestMethod()]
        public void AddPointToMultipleCategoriesTest()
        {
            double latitude = 11.23;
            double longitude = 22.36;
            List<string> categoryName = new List<string>();
            categoryName.Add("test"+random.Next());
            categoryName.Add("test"+random.Next());
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("aa", "aa");
            try
            {
                var result=target.AddPoint(latitude, longitude, categoryName, metadata);
                Assert.AreEqual(2,result.Categories.Count);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void GetPointsForRectCategoriesTest()
        {
            try
            {
                for (int i = 1; i <= 10; i++)
                {
                    double nwlat = 80.01 - 0.00001 * i;
                    double nwlon = 60.01 + 0.00001 * i;
                    Dictionary<string, string> metadata = new Dictionary<string, string> ();
                    metadata.Add("TEST", "test" + i);
                    target.AddPoint(nwlat,nwlon,metadata);
                }
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(80.01, 60.01, 60.01, 80.01);
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                    double lat = i * 1609.344 * METERS;
                    target.AddPoint(lat,lon,metadata);
                }
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(5 * 1609.344 * METERS, 5 * 1609.344 * METERS, 10, Units.MILES);
                var result=target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(1.23, 2.36, 100, Units.YARDS);
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(1.23, 2.36, 1, Units.KILOMETERS);
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                double latitude = 1.23;
                double longitude = 2.36;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("A", "aaa");
                target.AddPoint(latitude, longitude, metadata);
                BackendlessGeoQuery query = new BackendlessGeoQuery(metadata);
                query.Radius = 20;
                query.Units = Units.METERS;
                var result = target.GetPoints(query);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }

        }
        [TestMethod()]
        public void SearchPointsInDefaultCategoryWithNegativeOffsetTest()
        {
            try
            {
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(60.01, 60.01, 2, -1);
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery);
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
                double lon = 10  * METERS;
                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("A", "aaa");
                metadata.Add("B", "bbb");
                for (int i = 0; i < 14; i++)
                {
                    double lat = i * METERS;
                    target.AddPoint(lat, lon, metadata);
                }
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(5  * METERS, 5 * METERS, 10, Units.MILES);
                geoQuery.PageSize = 1;
                geoQuery.Offset = 15;
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("4003", ex.Code);
            }
        }
        [TestMethod()]
        public void SearchPointsIncludemetadataWithNegativeOffsetTest()
        {
            
            try
            {
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.IncludeMeta = false;
                geoQuery.Latitude = 60.01;
                geoQuery.Longitude = 60.01;
                geoQuery.Radius = 20;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 0;
                geoQuery.Units = Units.METERS;
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                Dictionary<string,string> dict=new Dictionary<string,string>();
                dict.Add("Test","Test");
                List<string> cateName=new List<string>{"cateName1","cateName2"};
                //var point=target.AddPoint(2.22,3.33,cateName,dict);
                BackendlessGeoQuery query = new BackendlessGeoQuery();
                query.Categories = cateName;
                query.Radius = 20;
                query.Units = Units.METERS;
                query.PageSize = 5;
                query.Offset = 0;
                var result=target.GetPoints(query);
                Assert.IsNotNull(result);
            }
            catch(BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod()]
        public void GetPointsForRectangleAndRadiusTest()
        {
            try
            {
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery(80.01, 60.01, 60.01, 80.01);
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 1;
                var result = target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
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
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 91;
                geoQuery.Longitude = 180;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery);
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
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 90;
                geoQuery.Longitude = 181;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                target.GetPoints(geoQuery);
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
                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Latitude = 30;
                geoQuery.Longitude = 60;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = -2;
                geoQuery.Offset = 1;
                target.GetPoints(geoQuery);
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
                List<string> cateName = new List<string> { "cateName1","cateName2"};

                BackendlessGeoQuery geoQuery = new BackendlessGeoQuery();
                geoQuery.Categories = cateName;
                geoQuery.Radius = 20;
                geoQuery.Units = Units.METERS;
                geoQuery.PageSize = 2;
                geoQuery.Offset = 0;
                var result=target.GetPoints(geoQuery);
                Assert.IsNotNull(result);
            }
            catch (BackendlessException e)
            {
                Assert.Fail(e.Message);
            }
        }
       


    }
}
