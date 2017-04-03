using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class RestorePasswordTest : TestsFrame
  {
    [TestMethod]
    public void TestRestoreUserPassword()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
      BackendlessUser user = GetRandomLoggedInUser();
      Backendless.UserService.RestorePassword( (string) user.GetProperty( LOGIN_KEY ) );
    }

    [TestMethod]
    public void TestRestoreUserPasswordWithWrongLogin()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = GetRandomLoggedInUser();
        Backendless.UserService.RestorePassword( "fake_login_" + user.GetProperty( LOGIN_KEY ) );
        Assert.Fail( "Server accepted wrong login." );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3020, t );
      }
    }
  }
}