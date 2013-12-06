using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.File;
using BackendlessAPI.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI.Property;
using System.Collections.Generic;
using System.IO;

namespace BackendlessAPI.Test
{


  /*/// <summary>
  ///This is a test class for FileServiceFileTest and is intended
  ///to contain all FileServiceFileTest Unit Tests
  ///</summary>
  [TestClass()]
  public class AsyncFileServiceTest
  {


    private TestContext testContextInstance;
    private Random random = new Random( (int) DateTime.Now.Ticks );

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    ///
    private FileService fileService = new FileService();

    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
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
      Backendless.InitApp( applicationId, secretKey, version );
    }

    /// <summary>
    ///A test for Upload
    ///</summary>
    [TestMethod()]
    public void uploadFileTest()
    {
      try
      {
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { Assert.IsNotNull( u ); }, f =>
            { Assert.Fail( f.Message ); } );

        string filePath = @"c:\test.txt";
        string path = "test" + random.Next().ToString() + @"/test.txt";
        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.Fail( ex.Message );
      }
    }

    [TestMethod()]
    public void uploadFileWithInvalidPathTest()
    {
      try
      {
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( Assert.IsNotNull,
                                                                                      f => Assert.Fail( f.Message ) );
        string filePath = @"c:\test111.txt";
        string path = "test" + random.Next().ToString() + @"/test.txt";
        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.AreEqual( "N/A", ex.Code );
      }

    }

    [TestMethod()]
    public void uploadFileWithInvalidURLTest()
    {
      try
      {
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { Assert.IsNotNull( u ); }, f =>
            {
              //Assert.Fail(f.Message);
              Assert.AreEqual( "N/A", f.FaultCode );
            } );
        string filePath = @"c:\test.txt";
        string path = "test";
        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.Fail( ex.Message );
      }
    }

    [TestMethod()]
    public void deleteFileTest()
    {
      try
      {
        string filePath = @"c:\test.txt";
        string path = "test" + random.Next().ToString() + @"/test.txt";
        AsyncCallback<object> callback2 = new AsyncCallback<object>( u =>
          { }, f =>
            { Assert.Fail( f.Message ); } );
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { fileService.Remove( path, callback2 ); }, f =>
            { Assert.Fail( f.Message ); } );

        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.Fail( ex.Message );
      }
    }

    [TestMethod()]
    public void deleteNonExistingFileTest()
    {
      try
      {
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { }, f =>
            { Assert.AreEqual( "6000", f.FaultCode ); } );
        fileService.Remove( "" );
      }
      catch( BackendlessException ex )
      {
        Assert.AreEqual( "N/A", ex.Code );
      }
    }

    [TestMethod()]
    public void deleteDirectoryTest()
    {
      try
      {
        string filePath = @"c:\test.txt";
        string path = "test" + random.Next().ToString() + @"/test.txt";

        AsyncCallback<object> callback3 = new AsyncCallback<object>( u =>
          { Assert.IsNotNull( u ); }, f =>
            { Assert.Fail( f.Message ); } );
        AsyncCallback<object> callback2 = new AsyncCallback<object>( u =>
          { fileService.RemoveDirectory( @"\test\", callback3 ); }, f =>
            { Assert.Fail( f.Message ); } );
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { fileService.Remove( path, callback2 ); }, f =>
            { Assert.Fail( f.Message ); } );

        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.AreEqual( "aaa", ex.Code );
      }
    }

    [TestMethod()]
    public void deleteNonExistingDirectoryTest()
    {
      try
      {
        AsyncCallback<object> callback = new AsyncCallback<object>( u =>
          { Assert.IsNotNull( u ); }, f =>
            { Assert.Fail( f.Message ); } );
        fileService.RemoveDirectory( @"\111", callback );
      }
      catch( BackendlessException ex )
      {
        Assert.Fail( ex.Message );
      }
    }

    [TestMethod()]
    public void deleteDirectoryWithFilesTest()
    {
      try
      {
        string directoryName = "test" + random.Next().ToString();
        string path = directoryName + @"/test.txt";

        AsyncCallback<object> callback2 = new AsyncCallback<object>( u =>
          { Assert.IsNotNull( u ); }, f =>
            { Assert.AreEqual( "N/A", f.FaultCode ); } );
        AsyncCallback<BackendlessFile> callback = new AsyncCallback<BackendlessFile>( u =>
          { fileService.RemoveDirectory( directoryName, callback2 ); }, f =>
            { Assert.Fail( f.Message ); } );
        string filePath = @"c:\test.txt";

        fileService.Upload( filePath, path, callback );
      }
      catch( BackendlessException ex )
      {
        Assert.Fail( ex.Message );
      }
    }
}*/
}