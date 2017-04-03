using System.IO;
using Backendless.Test;
using BackendlessAPI.Async;
using BackendlessAPI.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.FileService.AsyncTests
{
  [TestClass]
  public class DeleteTest : TestsFrame
  {
    [TestMethod]
    public void TestDeleteSingleFile()
    {
      RunAndAwait( () =>
        {
          FileStream fileToUpload = CreateRandomFile();
          string path = GetRandomPath();

          Backendless.Files.Upload( fileToUpload, path,
                                    new ResponseCallback<BackendlessFile>( this )
                                      {
                                        ResponseHandler = backendlessFile =>
                                          {
                                            Assert.IsNotNull( backendlessFile, "Server returned a null" );
                                            Assert.IsNotNull( backendlessFile.FileURL, "Server returned a null url" );
                                            Assert.AreEqual(
                                              "https://api.backendless.com/" + Defaults.TEST_APP_ID.ToLower() + "/" +
                                              Defaults.TEST_SECRET_KEY.ToLower() + "/files/" + path + "/" + Path.GetFileName(fileToUpload.Name),
                                              backendlessFile.FileURL,
                                              "Server returned wrong url " + backendlessFile.FileURL );

                                            Backendless.Files.Remove( path,
                                                                      new ResponseCallback<object>( this )
                                                                        {
                                                                          ResponseHandler = response => CountDown()
                                                                        } );
                                          }
                                      } );
        } );
    }

    [TestMethod]
    public void TestDeleteNonExistingFile()
    {
      RunAndAwait(
        () =>
        Backendless.Files.Remove( "foobarfoo",
                                  new AsyncCallback<object>(
                                    response => Assert.Fail( "Server didn't send an exception" ),
                                    fault => CheckErrorCode( 6000, fault ) ) ) );
    }

    [TestMethod]
    public void TestDeleteEmptyDirectory()
    {
      RunAndAwait( () =>
        {
          var fileToUpload = CreateRandomFile();
          var path = GetRandomPath();
          var dirName = "somedir";

          Backendless.Files.Upload( fileToUpload, dirName + "/" + path,
                                    new ResponseCallback<BackendlessFile>( this )
                                      {
                                        ResponseHandler = backendlessFile =>
                                          {
                                            Assert.IsNotNull( backendlessFile, "Server returned a null" );
                                            Assert.IsNotNull( backendlessFile.FileURL, "Server returned a null url" );
                                            Assert.AreEqual(
                                              "https://api.backendless.com/" + Defaults.TEST_APP_ID.ToLower() + "/" +
                                              Defaults.TEST_SECRET_KEY.ToLower() + "/files/" + dirName + "/" + path + "/" +
                                              Path.GetFileName(fileToUpload.Name), backendlessFile.FileURL,
                                              "Server returned wrong url " + backendlessFile.FileURL );

                                            Backendless.Files.RemoveDirectory( dirName,
                                                                               new ResponseCallback<object>( this )
                                                                                 {
                                                                                   ResponseHandler = response => CountDown()
                                                                                 } );
                                          }
                                      } );
        } );
    }

    [TestMethod]
    public void TestDeleteNonExistingDirectory()
    {
      RunAndAwait(
        () =>
        Backendless.Files.RemoveDirectory( "foobarfoodir",
                                           new AsyncCallback<object>(
                                             response => Assert.Fail( "Server didn't throw an expected exception" ),
                                             fault => CheckErrorCode( 6000, fault ) ) ) );
    }

    [TestMethod]
    public void TestDeleteDirectoryWithFiles()
    {
      RunAndAwait( () =>
        {
          var fileToUpload = CreateRandomFile();
          string path = GetRandomPath();
          string dirName = "somedir";

          Backendless.Files.Upload( fileToUpload, dirName + "/" + path,
                                    new ResponseCallback<BackendlessFile>( this )
                                      {
                                        ResponseHandler = backendlessFile =>
                                          {
                                            Assert.IsNotNull( backendlessFile, "Server returned a null" );
                                            Assert.IsNotNull( backendlessFile.FileURL, "Server returned a null url" );
                                            Assert.AreEqual(
                                              "https://api.backendless.com/" + Defaults.TEST_APP_ID.ToLower() + "/" +
                                              Defaults.TEST_SECRET_KEY.ToLower() + "/files/" + dirName + "/" + path + "/" +
                                              Path.GetFileName(fileToUpload.Name), backendlessFile.FileURL,
                                              "Server returned wrong url " + backendlessFile.FileURL );

                                            Backendless.Files.RemoveDirectory( dirName,
                                                                               new ResponseCallback<object>( this )
                                                                                 {
                                                                                   ResponseHandler = response => CountDown()
                                                                                 } );
                                          }
                                      } );
        } );
    }
  }
}