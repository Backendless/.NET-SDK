using BackendlessAPI;
using BackendlessAPI.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI.Property;
using System.Collections.Generic;
using System.IO;

namespace BackendlessAPI.Test
{
    //FileService.Upload switched to Async logic. Tests shouldn`t work
    
    /*/// <summary>
    ///This is a test class for FileServiceFileTest and is intended
    ///to contain all FileServiceFileTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileServiceFileTest
    {


        private TestContext testContextInstance;
        Random random = new Random((int)DateTime.Now.Ticks);
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        ///
        FileService fileService = new FileService();
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
        ///A test for Upload
        ///</summary>
        [TestMethod()]
        public void uploadFileTest()
        {
            string filePath = @"c:\test.txt";
            string path = "test" + random.Next().ToString() + @"/test.txt";
            try
            {
                BackendlessFile file = fileService.Upload(filePath, path);
                Assert.IsNotNull(file);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            }  
        }

        [TestMethod()]
        public void uploadFileWithInvalidPathTest()
        {
            string filePath = @"c:\test.txt";
            string path = "test" + random.Next().ToString() + @"/test.txt";
            try
            {
                BackendlessFile file = fileService.Upload(filePath, path);
                Assert.IsNotNull(file);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            } 
            
          
        }
        [TestMethod()]
        public void uploadFileWithInvalidURLTest()
        {
            string filePath = @"c:\test.txt";
            string path = "test";
            try
            {
                BackendlessFile file = fileService.Upload(filePath, path);
                Assert.IsNotNull(file);
            }
            catch (BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }


        }
         [TestMethod()]
        public void deleteFileTest()
        {
            string filePath = @"c:\test.txt";
            string path = "test" + random.Next().ToString() + @"/test.txt";
            try
            {
                BackendlessFile file = fileService.Upload(filePath, path);
                fileService.Remove(path);                
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            } 
        }

        [TestMethod()]
        public void deleteNonExistingFileTest()
        {
            try
            {
                fileService.Remove("");
                Assert.IsTrue(true);
            }
            catch(BackendlessException ex)
            {
                Assert.AreEqual("N/A", ex.Code);
            }
        }
         [TestMethod()]
        public void deleteDirectoryTest()
        {
            string filePath = @"c:\test.txt";
            string directoryName = "test" + random.Next().ToString();
            string path = directoryName + @"/test.txt";
            try
            {
                BackendlessFile file = fileService.Upload(filePath, path);
                fileService.Remove(path);
                fileService.RemoveDirectory(directoryName);
                Assert.IsTrue(true);
            }
            catch (BackendlessException ex)
            {
                Assert.Fail(ex.Message);
            } 
        }
        [TestMethod()]
         public void deleteNonExistingDirectoryTest()
         {
             string directoryName = "test" + random.Next().ToString();
             try
             {
                 fileService.RemoveDirectory(directoryName);
                 Assert.IsTrue(true);
             }
             catch (BackendlessException ex)
             {
                 Assert.AreEqual("6000", ex.Code);
             } 
         }
        [TestMethod()]
         public void deleteDirectoryWithFilesTest()
         {
             string filePath = @"c:\test.txt";
             string directoryName = "test" + random.Next().ToString();
             string path = directoryName + @"/test.txt";
             try
             {
                 BackendlessFile file = fileService.Upload(filePath, path);
                 fileService.RemoveDirectory(directoryName);
                 Assert.IsTrue(true);
             }
             catch (BackendlessException ex)
             {
                 Assert.AreEqual("N/A", ex.Code);
             }
         }
      
       
    }*/
}
