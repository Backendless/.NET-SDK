using System;
using System.Collections.Generic;
using Backendless.Test;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using BackendlessAPI.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class UpdateTest : TestsFrame
  {
    [TestMethod]
    public void TestUpdateUserWithEmptyCredentials()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( LOGIN_KEY, "" );
                  user.Password = "";

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response => FailCountDownWith( "User with empty credentials accepted" ),
                                                    fault => CheckErrorCode( ExceptionMessage.NULL_PASSWORD, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserWithNullCredentials()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( LOGIN_KEY, null );
                  user.Password = null;

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response => FailCountDownWith( "User with null credentials accepted" ),
                                                    fault => CheckErrorCode( ExceptionMessage.NULL_PASSWORD, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserWithNullUserId()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( ID_KEY, null );

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response => FailCountDownWith( "User with null id accepted" ),
                                                    fault => CheckErrorCode( ExceptionMessage.WRONG_USER_ID, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserWithEmptyUserId()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( ID_KEY, "" );

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response => FailCountDownWith( "User with empty id accepted" ),
                                                    fault => CheckErrorCode( ExceptionMessage.WRONG_USER_ID, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserWithWrongUserId()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( ID_KEY, "foobar" );

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response => FailCountDownWith( "User with wrong id accepted" ),
                                                    fault => CheckErrorCode( 3029, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserForVersionWithDisabledDynamicPropertis()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp(Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY);
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.SetProperty( "somePropertyKey", "somePropertyValue" );

                  Backendless.UserService.Update( user,
                                                  new AsyncCallback<BackendlessUser>(
                                                    response =>
                                                    FailCountDownWith(
                                                      "Server updated user with a dynamic property for a version with disabled dynamic properties." ),
                                                    fault => CheckErrorCode( 3031, fault ) ) );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateUserForVersionWithEnabledDynamicPropertis()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  string propertyKey = "somePropertyKey" + Random.Next();
                  string propertyValue = "somePropertyValue" + Random.Next();
                  user.SetProperty( propertyKey, propertyValue );

                  foreach( String usedProperty in UsedProperties )
                    user.SetProperty( usedProperty, "someValue" );

                  Backendless.UserService.Update( user,
                                                  new ResponseCallback<BackendlessUser>( this )
                                                    {
                                                      ResponseHandler = response =>
                                                        {
                                                          UsedProperties.Add( propertyKey );
                                                          Backendless.UserService.DescribeUserClass(
                                                            new ResponseCallback<List<UserProperty>>( this )
                                                              {
                                                                ResponseHandler = userProperties =>
                                                                  {
                                                                    Assert.IsNotNull( userProperties,
                                                                                      "Server returned null user properties" );
                                                                    Assert.IsTrue( userProperties.Count != 0,
                                                                                   "Server returned empty user properties" );

                                                                    bool flag = false;
                                                                    foreach( UserProperty userProperty in userProperties )
                                                                    {
                                                                      if( userProperty.Name.Equals( propertyKey ) )
                                                                      {
                                                                        flag = true;
                                                                        Assert.IsTrue(
                                                                          userProperty.Type.Equals( DateTypeEnum.STRING ),
                                                                          "Property had wrong type" );
                                                                      }
                                                                    }

                                                                    Assert.IsTrue( flag, "Expected property was not found" );
                                                                    CountDown();
                                                                  }
                                                              } );
                                                        }
                                                    } );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateRegisteredUserEmailAndPassword()
    {
      RunAndAwait( () =>
        {
          string newpassword = "some_new_password";
          string newemail = "some_new_email@gmail.com";
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
          GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
            {
              ResponseHandler = user =>
                {
                  user.Password = newpassword;
                  user.SetProperty( EMAIL_KEY, newemail );

                  Backendless.UserService.Update( user,
                                                  new ResponseCallback<BackendlessUser>( this )
                                                    {
                                                      ResponseHandler = response =>
                                                        {
                                                          Assert.AreEqual( newpassword, user.Password,
                                                                           "Updated used has a wrong password" );
                                                          Assert.AreEqual( newemail, user.GetProperty( EMAIL_KEY ),
                                                                           "Updated used has a wrong email" );
                                                          CountDown();
                                                        }
                                                    } );
                }
            } );
        } );
    }

    [TestMethod]
    public void TestUpdateRegisteredUserIdentity()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
      GetRandomLoggedInUser( new ResponseCallback<BackendlessUser>( this )
        {
          ResponseHandler = user =>
            {
              user.SetProperty( LOGIN_KEY, "some_new_login_" + user.GetProperty( LOGIN_KEY ) );

              Backendless.UserService.Update( user, new ResponseCallback<BackendlessUser>( this ) );
            }
        } );
    }
  }
}