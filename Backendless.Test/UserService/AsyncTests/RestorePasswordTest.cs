using System;
using Backendless.Test;
using BackendlessAPI.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class RestorePasswordTest : TestsFrame
  {
    [TestMethod]
    public void TestRestoreUserPassword()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.RestorePassword( (string) response.GetProperty( LOGIN_KEY ),
                                                         new ResponseCallback<object>( this ) )
            } );
        } );
    }

    [TestMethod]
    public void TestRestoreUserPasswordWithWrongLogin()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.RestorePassword( "fake_login_" + response.GetProperty( LOGIN_KEY ),
                                                         new AsyncCallback<object>(
                                                           o => FailCountDownWith( "Server accepted wrong login." ),
                                                           fault => CheckErrorCode( 3020, fault ) ) )
            } );
        } );
    }
  }
}