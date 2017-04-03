using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backendless.Test;
using BackendlessAPI.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class RegistrationTests : TestsFrame
  {
    [TestMethod]
    public void TestRegisterNewUserFromNull()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        Backendless.UserService.Register( null );
        Assert.Fail( "UserService accepted a null user" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( ExceptionMessage.NULL_USER, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserFromEmptyUser()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        Backendless.UserService.Register( new BackendlessUser() );
        Assert.Fail( "UserService accepted a null user" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( ExceptionMessage.NULL_PASSWORD, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithPartialFields()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );

        BackendlessUser partialUser = new BackendlessUser();
        String login = "bot" + Random.Next();
        partialUser.SetProperty( LOGIN_KEY, login );
        partialUser.SetProperty( EMAIL_KEY, login + "@gmail.com" );
        partialUser.Password = "somepass";
        Backendless.UserService.Register( partialUser );
        Assert.Fail( "UserService accepted a user without required fields" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3012, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithNulls()
    {
      try
      {
        Backendless.InitApp(Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY);
        BackendlessUser user = new BackendlessUser();
        user.SetProperty(LOGIN_KEY, null);
        user.SetProperty(EMAIL_KEY, null);
        user.Password = null;
        user.SetProperty(null, "foo");
        user.SetProperty("foo", null);

        Backendless.UserService.Register(user);
        Assert.Fail("UserService accepted null values");
      }
      catch( ArgumentNullException )
      {
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithNullPropertyValue()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = GetRandomNotRegisteredUser();
        user.SetProperty( "gender", null );

        Backendless.UserService.Register( user );

        Assert.Fail( "UserService accepted null value for a property" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3041, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithEmptyPropertyValue()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = GetRandomNotRegisteredUser();
        user.SetProperty( "gender", "" );

        Backendless.UserService.Register( user );

        Assert.Fail( "UserService accepted empty value for a property" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3041, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithEmptyCredentials()
    {
      try
      {
        Backendless.InitApp(Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY);
        BackendlessUser user = new BackendlessUser();
        user.SetProperty(LOGIN_KEY, "");
        user.SetProperty(EMAIL_KEY, "");
        user.Password = "";
        user.SetProperty("", "foo");
        user.SetProperty("foo", "");

        Backendless.UserService.Register(user);
        Assert.Fail("BackendlessUser accepted empty values");
      }
      catch( ArgumentNullException)
      {
      }
    }

    [TestMethod]
    public void TestRegisterNewUser()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
      BackendlessUser user = GetRandomNotRegisteredUser();
      String propertyKey = "property_key#" + Random.Next();
      String propertyValue = "property_value#" + Random.Next();
      user.SetProperty( propertyKey, propertyValue );
      BackendlessUser registeredUser = Backendless.UserService.Register( user );

      UsedProperties.Add( propertyKey );

      Assert.IsNotNull( registeredUser.GetProperty( "id" ), "UserService.register didn't set user ID" );

      foreach( string key in user.Properties.Keys )
      {
        Assert.IsTrue( registeredUser.Properties.ContainsKey( key ),
                       "Registered user didn`t contain expected property " + key );
        Assert.AreEqual( user.GetProperty( key ), registeredUser.GetProperty( key ),
                         "UserService.register changed property " + key );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithDuplicateIdentity()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = Backendless.UserService.Register( GetRandomNotRegisteredUser() );

        BackendlessUser fakeUser = GetRandomNotRegisteredUser();
        fakeUser.SetProperty( LOGIN_KEY, user.GetProperty( LOGIN_KEY ) );

        Backendless.UserService.Register( fakeUser );

        Assert.Fail( "Server accepted a user with id value" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3033, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithId()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = Backendless.UserService.Register( GetRandomNotRegisteredUser() );

        BackendlessUser fakeUser = GetRandomNotRegisteredUser();
        fakeUser.SetProperty( ID_KEY, user.GetProperty( ID_KEY ) );

        Backendless.UserService.Register( fakeUser );
        Assert.Fail( "Server accepted a user with id value" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3039, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserAtAppWithDisabledRegistration()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );

        Backendless.UserService.Register( GetRandomNotRegisteredUser() );

        Assert.Fail( "Server accepted registration for an application with disabled registration" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3009, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserAtAppWithDisabledDynamicProperties()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        BackendlessUser user = GetRandomNotRegisteredUser();
        user.SetProperty( "somedynamicpropertykey", "somedynamicpropertyvalue" );

        Backendless.UserService.Register( user );

        Assert.Fail( "BackendlessUser accepted registration for an application with disabled dynamic properties" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3010, t );
      }
    }

    [TestMethod]
    public void TestRegisterNewUserWithoutIdentity()
    {
      try
      {
        Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
        String timestamp = DateTime.Now.Ticks.ToString();
        BackendlessUser user = new BackendlessUser();
        user.SetProperty( EMAIL_KEY, "bot" + timestamp + "@gmail.com" );
        user.Password = "somepass_" + timestamp;

        Backendless.UserService.Register( user );

        Assert.Fail( "Server accepted user without identity" );
      }
      catch( System.Exception t )
      {
        CheckErrorCode( 3013, t );
      }
    }
  }
}