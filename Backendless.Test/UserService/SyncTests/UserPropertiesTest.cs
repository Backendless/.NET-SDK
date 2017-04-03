using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Backendless.Test;
using BackendlessAPI.Property;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [TestClass]
  public class UserPropertiesTest : TestsFrame
  {
    [TestMethod]
    public void TestDescribeUserProperties()
    {
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
      BackendlessUser user = GetRandomNotRegisteredUser();
      string propertyKeySync = "property_key#Sync";
      string propertyKeyAsync = "property_key#Async";
      user.SetProperty( propertyKeySync, "porperty_value#" + Random.Next() );
      Backendless.UserService.Register( user );
      Backendless.UserService.Login((string) user.GetProperty(LOGIN_KEY), user.Password);
      List<UserProperty> userProperties = Backendless.UserService.DescribeUserClass();
      Assert.IsNotNull( userProperties, "Server returned null user properties" );
      Assert.IsTrue( userProperties.Count != 0, "Server returned empty user properties" );

      var properties = new List<string> {propertyKeySync, propertyKeyAsync, ID_KEY, LOGIN_KEY, PASSWORD_KEY, EMAIL_KEY};

      foreach( UserProperty userProperty in userProperties )
      {
        Assert.IsNotNull( userProperty, "User property was null" );
        Assert.IsTrue( properties.Contains( userProperty.Name ),
                       "User properties contained unexpected property " + userProperty.Name );
        Assert.IsNotNull( userProperty.Type, "User properties type was null" );
      }
    }
  }
}