using BackendlessAPI;
using BackendlessAPI.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BackendlessAPI.Test
{
    
    
    /// <summary>
    ///This is a test class for AppTest and is intended
    ///to contain all AppTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppTest
    {


        private TestContext testContextInstance;

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


        /// <summary>
        ///A test for InitApp
        ///</summary>
        [TestMethod()]
        public void InitTest()
        {
            try
            {
                string applicationId = "validApp-Ids0-0000-0000-000000000000"; 
                string secretKey = "validSec-retK-eys0-0000-000000000000";
                string version = "v1"; 
                Backendless.InitApp(applicationId, secretKey, version); 
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
