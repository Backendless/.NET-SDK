using System.Collections.Generic;
using Backendless.Test;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class LoginTest : TestsFrame
  {
    [TestMethod]
    public void TestLoginWithNullLogin()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( null, response.Password,
                                               new AsyncCallback<BackendlessUser>(
                                                 user => FailCountDownWith( "UserService accepted null login" ),
                                                 fault => CheckErrorCode( ExceptionMessage.NULL_LOGIN, fault ) ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithNullPassword()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), null,
                                               new AsyncCallback<BackendlessUser>(
                                                 user => FailCountDownWith( "UserService accepted null password" ),
                                                 fault => CheckErrorCode( ExceptionMessage.NULL_PASSWORD, fault ) ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithWrongCredentials()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( response.GetProperty( LOGIN_KEY ) + "foobar", response.Password + "foobar",
                                               new AsyncCallback<BackendlessUser>(
                                                 user => FailCountDownWith( "Server accepted wrong credentials" ),
                                                 fault => CheckErrorCode( 3003, fault ) ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithDisabledAppLogin()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v2_nata" );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), response.Password,
                                               new AsyncCallback<BackendlessUser>(
                                                 user => FailCountDownWith( "Server accepted login" ),
                                                 fault => CheckErrorCode( 3034, fault ) ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithProperCredentials()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), response.Password,
                                               new ResponseCallback<BackendlessUser>( this )
                                                 {
                                                   ResponseHandler =
                                                     user =>
                                                     Backendless.UserService.DescribeUserClass(
                                                       new ResponseCallback<List<UserProperty>>( this ) )
                                                 } )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithExternalAuth()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v_nata6" );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), "password123",
                                               new AsyncCallback<BackendlessUser>(
                                                 user =>
                                                 FailCountDownWith(
                                                   "Server accepted registration for an invalid external Auth" ),
                                                 fault => CheckErrorCode( 3052, fault ) ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithoutFailedLoginsLock()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v3_nata" );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler =
                response =>
                Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), response.Password + "foo",
                                               new AsyncCallback<BackendlessUser>(
                                                 user => FailCountDownWith( "Server didn't locked login" ), fault =>
                                                   {
                                                     Backendless.UserService.Login(
                                                       (string) response.GetProperty( LOGIN_KEY ), response.Password,
                                                       new ResponseCallback<BackendlessUser>( this )
                                                         {
                                                           ResponseHandler = user => CountDown()
                                                         } );
                                                   } ) )
            } );
        } );
    }

    [TestMethod]
    public void TestLoginWithUserFromAnotherVersion()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          GetRandomRegisteredUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = response =>
                {
                  Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, "v3_nata" );
                  Backendless.UserService.Login( (string) response.GetProperty( LOGIN_KEY ), response.Password,
                                                 new AsyncCallback<BackendlessUser>(
                                                   user =>
                                                   FailCountDownWith(
                                                     "Server accepted login for the user from another version" ),
                                                   fault => CheckErrorCode( 3003, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestCurrentUserAfterLogin()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY, Defaults.TEST_VERSION );
          BackendlessUser notRegisteredUseruser = GetRandomNotRegisteredUser();
          string propertyKey = "property_key#" + Random.Next();
          string propertyValue = "property_value#" + Random.Next();
          notRegisteredUseruser.SetProperty( propertyKey, propertyValue );

          Backendless.UserService.Register( notRegisteredUseruser,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler = response =>
                                                  {
                                                    UsedProperties.Add( propertyKey );
                                                    Backendless.UserService.Login(
                                                      (string) response.GetProperty( LOGIN_KEY ), response.Password,
                                                      new ResponseCallback<BackendlessUser>( this )
                                                        {
                                                          ResponseHandler = user =>
                                                            {
                                                              Assert.IsNotNull( Backendless.UserService.CurrentUser,
                                                                                "Current user was null" );
                                                              foreach( string key in user.Properties.Keys )
                                                              {
                                                                if( key.Equals( "password" ) )
                                                                  continue;

                                                                Assert.IsTrue(
                                                                  Backendless.UserService.CurrentUser.Properties.ContainsKey(
                                                                    key ),
                                                                  "Current user didn`t contain expected property " + key );
                                                                Assert.AreEqual( user.GetProperty( key ),
                                                                                 Backendless.UserService.CurrentUser
                                                                                            .GetProperty( key ),
                                                                                 "UserService.register changed property " +
                                                                                 key );
                                                              }
                                                              CountDown();
                                                            }
                                                        } );
                                                  }
                                              } );
        } );
    }
  }
}