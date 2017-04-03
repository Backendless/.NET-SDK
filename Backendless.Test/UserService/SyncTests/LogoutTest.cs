using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class LogoutTest : TestsFrame
  {
    [TestMethod]
    public void TestUserLogout()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
      BackendlessUser user = GetRandomLoggedInUser();
      Backendless.UserService.Logout();

      Assert.IsTrue( Backendless.UserService.CurrentUser == null, "Current user was not empty" );
    }
  }
}