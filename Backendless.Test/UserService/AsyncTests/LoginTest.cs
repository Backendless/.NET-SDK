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

    [Ignore]
    [TestMethod]
    public void TestLoginWithDisabledAppLogin()
    {
      RunAndAwait( () =>
        {
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
          BackendlessUser reguser = GetRandomRegisteredUser( null ); 
          Backendless.UserService.Login( (string) reguser.GetProperty( LOGIN_KEY ), reguser.Password,
                                               new ResponseCallback<BackendlessUser>( this )
                                                 {
                                                   ResponseHandler =
                                                     user =>
                                                     Backendless.UserService.DescribeUserClass(
                                                       new ResponseCallback<List<UserProperty>>( this ) )
                                                 } );
        });
    }


    [TestMethod]
    public void TestCurrentUserAfterLogin()
    {
      RunAndAwait( () =>
        {
          BackendlessUser notRegisteredUseruser = GetRandomNotRegisteredUser();
          string propertyKey = "propertykey" + Random.Next();
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