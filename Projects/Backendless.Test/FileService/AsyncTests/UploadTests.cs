using System.IO;
using Backendless.Test;
using BackendlessAPI.File;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.FileService.AsyncTests
{
  [TestClass]
  public class UploadTests : TestsFrame
  {
    [TestMethod]
    public void TestUploadSingleFile()
    {
      RunAndAwait( () =>
        {
          var fileToUpload = CreateRandomFile();
          var path = GetRandomPath() + "/" + GetRandomPath();

          Backendless.Files.Upload( fileToUpload, path,
                                    new ResponseCallback<BackendlessFile>( this )
                                      {
                                        ResponseHandler = backendlessFile =>
                                          {
                                            Assert.IsNotNull( backendlessFile, "Server returned a null" );
                                            Assert.IsNotNull( backendlessFile.FileURL, "Server returned a null url" );
                                            Assert.AreEqual(
                                              "https://api.backendless.com/" + Defaults.TEST_APP_ID.ToLower() + "/" +
                                              Defaults.TEST_SECRET_KEY.ToLower() + "/files/" + path + "/" + Path.GetFileName( fileToUpload.Name ),
                                              backendlessFile.FileURL,
                                              "Server returned wrong url " + backendlessFile.FileURL );

                                            CountDown();
                                          }
                                      } );
        } );
    }

    [TestMethod]
    public void TestUploadInvalidPath()
    {
      RunAndAwait( () =>
        {
          var fileToUpload = CreateRandomFile();
          var path = "9!@%^&*(){}[]/?|`~";

          Backendless.Files.Upload( fileToUpload, path,
                                    new ResponseCallback<BackendlessFile>( this )
                                      {
                                        ResponseHandler = backendlessFile =>
                                          {
                                            Assert.IsNotNull( backendlessFile, "Server returned null result" );
                                            var expected = "https://api.backendless.com/" + Defaults.TEST_APP_ID.ToLower() +
                                                           "/" + Defaults.TEST_SECRET_KEY.ToLower() + "/files/" + path + "/" +
                                                           Path.GetFileName(fileToUpload.Name);
                                            Assert.AreEqual( expected, backendlessFile.FileURL,
                                                             "Server returned wrong file url" );

                                            CountDown();
                                          }
                                      } );
        } );
    }
  }
}