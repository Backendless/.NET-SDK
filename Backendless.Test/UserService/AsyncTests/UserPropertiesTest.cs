using System;
using System.Collections.Generic;
using Backendless.Test;
using BackendlessAPI.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.AsyncTests
{
  [TestClass]
  public class UserPropertiesTest : TestsFrame
  {
    [TestMethod]
    public void TestDescribeUserProperties()
    {
      RunAndAwait( () =>
        {
          Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY);
          BackendlessUser user = GetRandomNotRegisteredUser();
          const string propertyKeySync = "property_key#Sync";
          const string propertyKeyAsync = "property_key#Async";
          user.SetProperty( propertyKeySync, "porperty_value#" + Random.Next() );
          Backendless.UserService.Register( user,
                                            new ResponseCallback<BackendlessUser>( this )
                                              {
                                                ResponseHandler =
                                                  response => {
                                                  Backendless.UserService.Login((string) user.GetProperty(LOGIN_KEY), user.Password);
                                                  Backendless.UserService.DescribeUserClass(
                                                    new ResponseCallback<List<UserProperty>>( this )
                                                      {
                                                        ResponseHandler = userProperties =>
                                                          {
                                                            Assert.IsNotNull( userProperties,
                                                                              "Server returned null user properties" );
                                                            Assert.IsTrue( userProperties.Count != 0,
                                                                           "Server returned empty user properties" );

                                                            var properties = new List<string>
                                                              {
                                                                propertyKeySync,
                                                                propertyKeyAsync,
                                                                ID_KEY,
                                                                LOGIN_KEY,
                                                                PASSWORD_KEY,
                                                                EMAIL_KEY
                                                              };

                                                            foreach( UserProperty userProperty in userProperties )
                                                            {
                                                              Assert.IsNotNull( userProperty, "User property was null" );
                                                              Assert.IsTrue( properties.Contains( userProperty.Name ),
                                                                             "User properties contained unexpected property " +
                                                                             userProperty.Name );
                                                              Assert.IsNotNull( userProperty.Type,
                                                                                "User properties type was null" );
                                                            }

                                                            CountDown();
                                                          }
                                                      } ); }
                                              } );
        } );
    }
  }
}