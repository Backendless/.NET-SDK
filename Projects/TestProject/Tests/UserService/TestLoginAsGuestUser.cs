using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Tests.UserService
{
  [Collection( "Tests" )]
  public class TestLoginAsGuestUser : IDisposable
  {
    public void Dispose()
    {
      Backendless.UserService.Logout();
    }

    [Fact]
    public void TestLoginAsGuestBlockCall()
    {
      var user = Backendless.UserService.LoginAsGuest();
      var currentUser = Backendless.UserService.CurrentUser;

      Assert.NotNull( currentUser );
      Assert.NotNull( user );
      Assert.IsType<BackendlessUser>( user );
      Assert.Equal( user, currentUser );

      Backendless.Data.Of<BackendlessUser>().Remove( currentUser );
    }

    [Fact]
    public void TestLoginAsGuestCallback()
    {
      Backendless.UserService.LoginAsGuest( new AsyncCallback<BackendlessUser>(
      user =>
      {
        var currentUser = Backendless.UserService.CurrentUser;

        Assert.NotNull( currentUser );
        Assert.NotNull( user );
        Assert.IsType<BackendlessUser>( user );
        Assert.Equal( user, currentUser );

        Backendless.Data.Of<BackendlessUser>().Remove( currentUser );
      },
      fault =>
      {
        Assert.True( false, "An error ocurred while executing the method" );
      } ) );
    }

    [Fact]
    public async void TestLoginAsGuestAsync()
    {
      var user = await Backendless.UserService.LoginAsGuestAsync();
      var currentUser = Backendless.UserService.CurrentUser;

      Assert.NotNull( currentUser );
      Assert.NotNull( user );
      Assert.IsType<BackendlessUser>( user );
      Assert.Equal( user, currentUser );

      Backendless.Data.Of<BackendlessUser>().Remove( currentUser );
    }
  }
}
