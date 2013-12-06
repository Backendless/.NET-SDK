using System;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class LoginTest: TestsFrame
  {
    [TestMethod]
  public void TestLoginWithNullLogin()
  {
      try
      {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      Backendless.UserService.Login( null, GetRandomRegisteredUser().Password );
      Backendless.UserService.Logout();
      Assert.Fail( "UserService accepted null Login" );
    }
    catch( System.Exception t )
    {
     CheckErrorCode( Exception.ExceptionMessage.NULL_LOGIN, t );
    }
  }

  [TestMethod]
  public void TestLoginWithNullPassword() 
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      Backendless.UserService.Login((string) GetRandomRegisteredUser().GetProperty(LOGIN_KEY), null);
      Assert.Fail( "UserService accepted null password" );
    }
    catch( System.Exception t )
    {
      CheckErrorCode( BackendlessAPI.Exception.ExceptionMessage.NULL_PASSWORD, t );
    }
  }

  [TestMethod]
  public void TestLoginWithWrongCredentials()
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      var user = GetRandomRegisteredUser();
      Backendless.UserService.Login(user.GetProperty(LOGIN_KEY) + "foobar", user.Password + "foobar");
      Assert.Fail( "Server accepted wrong credentials" );
    }
    catch( System.Exception t )
    {
     CheckErrorCode( 3003, t );
    }
  }

    [TestMethod]
  public void TestLoginWithDisabledAppLogin()
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v2_nata" );
      BackendlessUser user = GetRandomRegisteredUser();
      Backendless.UserService.Login((string) user.GetProperty(LOGIN_KEY), user.Password);
      Assert.Fail( "Server accepted Login" );
    }
    catch( System.Exception t )
    {
      CheckErrorCode( 3034, t );
    }
  }

  [TestMethod]
  public void TestLoginWithProperCredentials() 
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      BackendlessUser user = GetRandomRegisteredUser();
      Backendless.UserService.Login((string) user.GetProperty(LOGIN_KEY), user.Password);
      Backendless.UserService.DescribeUserClass();
    }
    catch( System.Exception t )
    {
      Assert.Fail( t.Message );
    }
  }

  [TestMethod]
  public void TestLoginWithExternalAuth() 
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v_nata6" );
      BackendlessUser user = GetRandomRegisteredUser();
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), "password123" );

      Assert.Fail( "Server accepted registration for an invalid external Auth" );
    }
    catch( System.Exception t )
    {
     CheckErrorCode( 3052, t );
    }
  }

  [TestMethod]
  public void TestLoginWithoutFailedLoginsLock() 
  {
    Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v3_nata" );
    BackendlessUser user = GetRandomRegisteredUser();

    try
    {
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password + "foo" );
    }
    catch( System.Exception t )
    {
    }

    try
    {
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );
    }
    catch( System.Exception t )
    {
      Assert.Fail( t.Message );
    }
  }

  [TestMethod]
  public void TestLoginWithUserFromAnotherVersion() 
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      BackendlessUser user = GetRandomRegisteredUser();

      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v3_nata" );
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );

      Assert.Fail( "Server accepted Login for the user from another version" );
    }
    catch( System.Exception t )
    {
      CheckErrorCode( 3003, t );
    }
  }

  [TestMethod]
  public void TestCurrentUserAfterLogin() 
  {
    try
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
      BackendlessUser notRegisteredUseruser = GetRandomNotRegisteredUser();
      string propertyKey = "property_key#" + Random.Next();
      string propertyValue = "property_value#" + Random.Next();
      notRegisteredUseruser.SetProperty( propertyKey, propertyValue );

      BackendlessUser user = Backendless.UserService.Register( notRegisteredUseruser );
      UsedProperties.Add( propertyKey );

      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );

      Assert.IsNotNull( Backendless.UserService.CurrentUser,  "Current user was null" );

      foreach( string key in user.Properties.Keys )
      {
        if( key.Equals( "password" ) )
        {
          continue;
        }

        Assert.IsTrue(Backendless.UserService.CurrentUser.Properties.ContainsKey(key), "Current user didn`t contain expected property " + key);
        Assert.AreEqual(user.GetProperty(key), Backendless.UserService.CurrentUser.GetProperty(key), "UserService.register changed property " + key);
      }
    }
    catch( System.Exception t )
    {
      Assert.Fail( t.Message );
    }
  }
  }
}
