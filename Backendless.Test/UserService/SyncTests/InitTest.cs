using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class InitTest: TestsFrame
  {
    [TestMethod]
    public void TestUserServiceInitialized()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
    }

    [TestMethod]
    public void TestCurrentUserIsEmpty()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      Assert.IsTrue( Backendless.UserService.CurrentUser == null, "Current user was not empty" );
    }
  }
}