using System;
using System.Collections.Generic;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [Ignore]
  [TestClass]
  public class TestsFrame : ITest
  {
    public Random Random = new Random();
    public List<String> UsedProperties = new List<string>();

    public const string LOGIN_KEY = "login";
    public const string EMAIL_KEY = "email";
    public const string PASSWORD_KEY = "password";
    public const string ID_KEY = "id";

    public BackendlessUser GetRandomNotRegisteredUser()
    {
      var timestamp = (DateTime.UtcNow.Ticks + Random.Next()).ToString();
      var result = new BackendlessUser();
      result.SetProperty( LOGIN_KEY, "bot" + timestamp );
      result.SetProperty( EMAIL_KEY, result.GetProperty( LOGIN_KEY ) + "@gmail.com" );
      result.Password = "somepass_" + timestamp;

      return result;
    }

    public BackendlessUser GetRandomRegisteredUser()
    {
      return Backendless.UserService.Register( GetRandomNotRegisteredUser() );
    }

    public BackendlessUser GetRandomLoggedInUser()
    {
      BackendlessUser user = GetRandomRegisteredUser();
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );

      return user;
    }

    [TestCleanup]
    public void TearDown()
    {
      if( Backendless.UserService.CurrentUser != null )
        Backendless.UserService.Logout();
    }
  }
}