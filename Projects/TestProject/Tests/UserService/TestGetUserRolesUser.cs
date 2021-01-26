using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;

namespace TestProject.Tests.UserService
{
  [Collection("Tests")]
  public class TestGetUserRolesUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();

    public void Dispose()
    {
      Backendless.UserService.Logout();
    }

    [Fact]
    public void TestGetUserRoles_BlockCall()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
      Backendless.UserService.Login( user.Email, user.Password );

      IList<String> roles = Backendless.UserService.GetUserRoles();

      Assert.NotNull( roles );
      Assert.Contains( "DotNetUser", roles );
      Assert.Contains( "AuthenticatedUser", roles );
      Assert.Equal( 2, roles.Count );
    }

    [Fact]
    public void TestGetUserRolesCallback()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
      Backendless.UserService.Login( user.Email, user.Password );

      Backendless.UserService.GetUserRoles( new AsyncCallback<IList<String>>(
      roles =>
      {
        Assert.NotNull( roles );
        Assert.Contains( "DotNetUser", roles );
        Assert.Contains( "AuthenticatedUser", roles );
        Assert.Equal( 2, roles.Count );
        Backendless.UserService.Logout();
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during method execution" );
      } ) );
    }

    [Fact]
    public async void TestGetUserRolesAsync()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
      Backendless.UserService.Login( user.Email, user.Password );

      IList<String> roles = await Backendless.UserService.GetUserRolesAsync();

      Assert.NotNull( roles );
      Assert.Contains( "DotNetUser", roles );
      Assert.Contains( "AuthenticatedUser", roles );
      Assert.Equal( 2, roles.Count );
    }

    [Fact]
    public void TestGetUserRoles_WithGuestUser()
    {
      var guestUser = Backendless.UserService.LoginAsGuest();

      IList<String> roles = Backendless.UserService.GetUserRoles();

      Assert.NotNull( roles );
      Assert.Contains( "DotNetUser", roles );
      Assert.Contains( "GuestUser", roles );
      Assert.Equal( 2, roles.Count );
      Backendless.Data.Of<BackendlessUser>().Remove( guestUser );
    }
  }
}
