using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
  [Collection("Tests")]
  public class TestLogoutUser
  {
    const String Email = "hdhdhd@gmail.com";
    const String Password = "123234";
    public TestLogoutUser()
    {
      Backendless.UserService.Login( Email, Password );
    }

    [Fact]
    public void TestLogoutBlockCall()
    {
      var currentUserBeforeLogout = Backendless.UserService.CurrentUser;

      Backendless.UserService.Logout();
      var currentUserAfterLogout = Backendless.UserService.CurrentUser;

      Assert.NotNull( currentUserBeforeLogout );
      Assert.Equal( Email, currentUserBeforeLogout.Email );
      Assert.Null( currentUserAfterLogout );
    }

    [Fact]
    public void TestLogoutCallback()
    {
      var currentUserBeforeLogout = Backendless.UserService.CurrentUser;

      Backendless.UserService.Logout( new AsyncCallback<Object>(
      Null =>
      {
        var currentUserAfterLogout = Backendless.UserService.CurrentUser;

        Assert.NotNull( currentUserBeforeLogout );
        Assert.Equal( Email, currentUserBeforeLogout.Email );
        Assert.Null( currentUserAfterLogout );
      },
      fault =>
      {
        Assert.True( false, "An error occurred while executing the method" );
      } ) );
    }

    [Fact]
    public async void TestLogoutAsync()
    {
      var currentUserBeforeLogout = Backendless.UserService.CurrentUser;

      await Backendless.UserService.LogoutAsync();
      var currentUserAfterLogout = Backendless.UserService.CurrentUser;

      Assert.NotNull( currentUserBeforeLogout );
      Assert.Equal( Email, currentUserBeforeLogout.Email );
      Assert.Null( currentUserAfterLogout );
    }
  }
}
