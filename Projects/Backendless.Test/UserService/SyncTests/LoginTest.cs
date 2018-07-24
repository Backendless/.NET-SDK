using System;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class LoginTest : TestsFrame
  {
    [TestMethod]
    public void TestLoginWithNullLogin()
    {
      try
      {
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
        Backendless.UserService.Login( (string) GetRandomRegisteredUser().GetProperty( LOGIN_KEY ), null );
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
        var user = GetRandomRegisteredUser();
        Backendless.UserService.Login( user.GetProperty( LOGIN_KEY ) + "foobar", user.Password + "foobar" );
        Assert.Fail( "Server accepted wrong credentials" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3003, t );
      }
    }

    [Ignore]
    [TestMethod]
    public void TestLoginWithDisabledAppLogin()
    {
      try
      {
        LoginDeveloper();
        DisableAppLogin();
        BackendlessUser user = GetRandomRegisteredUser();
        Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );
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
        BackendlessUser user = GetRandomRegisteredUser();
        Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );
        Backendless.UserService.DescribeUserClass();
      }
      catch( System.Exception t )
      {
        Assert.Fail( t.Message );
      }
    }


    [TestMethod]
    public void TestLoginWithoutFailedLoginsLock()
    {
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
    public void TestCurrentUserAfterLogin()
    {
      try
      {
        BackendlessUser notRegisteredUseruser = GetRandomNotRegisteredUser();
        string propertyKey = "propertykey" + Random.Next();
        string propertyValue = "property_value#" + Random.Next();
        notRegisteredUseruser.SetProperty( propertyKey, propertyValue );

        BackendlessUser user = Backendless.UserService.Register( notRegisteredUseruser );
        UsedProperties.Add( propertyKey );

        user = Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );

        Assert.IsNotNull( Backendless.UserService.CurrentUser, "Current user was null" );

        foreach( string key in user.Properties.Keys )
        {
          if( key.Equals( "password" ) )
          {
            continue;
          }

          Assert.IsTrue( Backendless.UserService.CurrentUser.Properties.ContainsKey( key ), "Current user didn`t contain expected property " + key );
          Assert.AreEqual( user.GetProperty( key ), Backendless.UserService.CurrentUser.GetProperty( key ), "UserService.register changed property " + key );
        }
      }
      catch( System.Exception t )
      {
        Assert.Fail( t.Message );
      }
    }
  }
}
