using System;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class RegistrationTest : TestsFrame
  {
    [TestMethod]
    public void TestRegisterNewUserFromNull()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          Backendless.UserService.Register( null,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => FailCountDownWith( "UserService accepted a null user" ),
                                                ErrorHandler = fault =>
                                                {
                                                  Assert.IsTrue(fault.ToString().Contains("Value cannot be null"));
                                                  CountDown();
                                                }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserFromEmptyUser()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          Backendless.UserService.Register( new BackendlessUser(),
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => FailCountDownWith( "UserService accepted a null user" ),
                                                ErrorHandler = fault =>
                                                  {
                                                    Assert.IsTrue( fault.ToString().Contains( "Value cannot be null" ) );
                                                    CountDown();
                                                  }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithPartialFields()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser partialUser = new BackendlessUser();
          String login = "bot" + Random.Next();
          partialUser.SetProperty( LOGIN_KEY, login );
          partialUser.SetProperty( EMAIL_KEY, login + "@gmail.com" );
          partialUser.Password = "somepass";
          Backendless.UserService.Register( partialUser,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response =>
                                                  FailCountDownWith( "UserService accepted a user without required fields" ),
                                                ErrorHandler = fault => CheckErrorCode( 3012, fault )
                                              } );
        } );
    }

    [TestMethod]
    [ExpectedException( typeof( ArgumentNullException ) )]
    public void TestRegisterNewUserWithNulls()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser user = new BackendlessUser();
          user.SetProperty( LOGIN_KEY, null );
          user.SetProperty( EMAIL_KEY, null );
          user.Password = null;
          user.SetProperty( null, "foo" );
          user.SetProperty( "foo", null );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => FailCountDownWith( "UserService accepted null values" )
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithNullPropertyValue()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY);
          BackendlessUser user = GetRandomNotRegisteredUser();
          user.SetProperty( "gender", null );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response =>
                                                  FailCountDownWith( "UserService accepted null value for a property" ),
                                                ErrorHandler = fault => CheckErrorCode( 3041, fault )
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithEmptyPropertyValue()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser user = GetRandomNotRegisteredUser();
          user.SetProperty( "gender", "" );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response =>
                                                  FailCountDownWith( "UserService accepted empty value for a property" ),
                                                ErrorHandler = fault => CheckErrorCode( 3041, fault )
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithEmptyCredentials()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser user = new BackendlessUser();
          user.SetProperty( LOGIN_KEY, "" );
          user.SetProperty( EMAIL_KEY, "" );
          user.Password = "";
          user.SetProperty( "", "foo" );
          user.SetProperty( "foo", "" );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => FailCountDownWith( "BackendlessUser accepted empty values" ),
                                                ErrorHandler = fault =>
                                                {
                                                  Assert.IsTrue(fault.ToString().Contains("Value cannot be null"));
                                                  CountDown();
                                                }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUser()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser user = GetRandomNotRegisteredUser();
          string propertyKey = "property_key#" + Random.Next();
          string propertyValue = "property_value#" + Random.Next();
          user.SetProperty( propertyKey, propertyValue );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler = response =>
                                                  {
                                                    UsedProperties.Add( propertyKey );
                                                    Assert.IsNotNull( response.GetProperty( "id" ),
                                                                      "UserService.register didn't set user ID" );

                                                    foreach( String key in user.Properties.Keys )
                                                    {
                                                      Assert.IsTrue( response.Properties.ContainsKey( key ),
                                                                     "Registered user didn`t contain expected property " +
                                                                     key );
                                                      Assert.AreEqual( user.GetProperty( key ), response.GetProperty( key ),
                                                                       "UserService.register changed property " + key );
                                                    }

                                                    CountDown();
                                                  }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithDuplicateIdentity()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          Backendless.UserService.Register( GetRandomNotRegisteredUser(),
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler = response =>
                                                  {
                                                    BackendlessUser fakeUser = GetRandomNotRegisteredUser();
                                                    fakeUser.SetProperty( LOGIN_KEY, response.GetProperty( LOGIN_KEY ) );

                                                    Backendless.UserService.Register( fakeUser,
                                                                                      new ResponseCallback<BackendlessUser>(
                                                                                        this )
                                                                                        {
                                                                                          ResponseHandler =
                                                                                            user =>
                                                                                            FailCountDownWith(
                                                                                              "Server accepted a user with id value" ),
                                                                                          ErrorHandler =
                                                                                            fault =>
                                                                                            CheckErrorCode( 3033, fault )
                                                                                        } );
                                                  }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithId()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          Backendless.UserService.Register( GetRandomNotRegisteredUser(),
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler = response =>
                                                  {
                                                    BackendlessUser fakeUser = GetRandomNotRegisteredUser();
                                                    fakeUser.SetProperty( ID_KEY, response.GetProperty( ID_KEY ) );

                                                    Backendless.UserService.Register( fakeUser,
                                                                                      new ResponseCallback<BackendlessUser>(
                                                                                        this )
                                                                                        {
                                                                                          ResponseHandler =
                                                                                            user =>
                                                                                            FailCountDownWith(
                                                                                              "Server accepted a user with id value" ),
                                                                                          ErrorHandler =
                                                                                            fault =>
                                                                                            CheckErrorCode( 3039, fault )
                                                                                        } );
                                                  }
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserAtAppWithDisabledRegistration()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          Backendless.UserService.Register( GetRandomNotRegisteredUser(),
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response =>
                                                  FailCountDownWith(
                                                    "Server accepted registration for an application with disabled registration" ),
                                                ErrorHandler = fault => CheckErrorCode( 3009, fault )
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserAtAppWithDisabledDynamicProperties()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          BackendlessUser user = GetRandomNotRegisteredUser();
          user.SetProperty( "somedynamicpropertykey", "somedynamicpropertyvalue" );

          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response =>
                                                  FailCountDownWith(
                                                    "BackendlessUser accepted registration for an application with disabled dynamic properties" ),
                                                ErrorHandler = fault => CheckErrorCode( 3010, fault )
                                              } );
        } );
    }

    [TestMethod]
    public void TestRegisterNewUserWithoutIdentity()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          String timestamp = DateTime.Now.Ticks.ToString();
          BackendlessUser user = new BackendlessUser();
          user.SetProperty( EMAIL_KEY, "bot" + timestamp + "@gmail.com" );
          user.Password = "somepass_" + timestamp;

          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => FailCountDownWith( "Server accepted user without identity" ),
                                                ErrorHandler = fault => CheckErrorCode( 3013, fault )
                                              } );
        } );
    }
  }
}