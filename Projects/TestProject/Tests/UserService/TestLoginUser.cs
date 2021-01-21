using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Tests.UserService
{
  [Collection( "Tests" )]
  public class TestLoginUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();
    public TestLoginUser()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
    }

    public void Dispose()
    {
      Backendless.UserService.Logout();
    }

    [Fact]
    public void TestLoginBlockCall()
    {
      var receivedUser = Backendless.UserService.Login( user.Email, user.Password );

      Assert.NotNull( receivedUser );
      Assert.IsType<BackendlessUser>( receivedUser );
      Assert.Equal( user.Email, receivedUser.Email );
    }

    [Fact]
    public void TestLoginCallback()
    {
      Backendless.UserService.Login( user.Email, user.Password, new AsyncCallback<BackendlessUser>(
      receivedUser =>
      {
        Backendless.UserService.Logout();
        Assert.NotNull( receivedUser );
        Assert.IsType<BackendlessUser>( receivedUser );
        Assert.Equal( user.Email, receivedUser.Email );
      },
      fault =>
      {
        Assert.True( false, $"An error was received during method execution\n{fault.Message}" );
      } ) );
    }

    [Fact]
    public async void TestLoginAsync()
    {
      var receivedUser = await Backendless.UserService.LoginAsync( user.Email, user.Password );

      Assert.NotNull( receivedUser );
      Assert.IsType<BackendlessUser>( receivedUser );
      Assert.Equal( user.Email, receivedUser.Email );
    }
  }
}
