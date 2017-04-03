using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class LogoutTest : TestsFrame
  {
    [TestMethod]
    public void TestUserLogout()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response => Backendless.UserService.Logout( new ResponseCallback<object>( this )
                  {
                    ResponseHandler = o =>
                      {
                        Assert.IsNull( Backendless.UserService.CurrentUser, "Current user was not empty" );
                        CountDown();
                      }
                  } )
            } );
        } );
    }
  }
}