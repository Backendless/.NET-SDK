using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;

namespace TestProject.Tests.UserService
{
  [Collection("Tests")]
  public class TestUpdateUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();
    public TestUpdateUser()
    {
      user.Email = "Testupdateuser@gmail.com";
      user.Password = "123234";
      user.Properties[ "Id" ] = "123";
      Backendless.UserService.Register( user );
      Backendless.UserService.Login( user.Email, user.Password );
    }

    public void Dispose()
    {
      Backendless.UserService.Logout();
      Backendless.Data.Of<BackendlessUser>().Remove( user );
    }

    [Fact]
    public void TestUpdateUser_BlockCall()
    {
      user.Properties[ "Id" ] = "321";
      var updUser = Backendless.UserService.Update( user );

      Assert.NotNull( user );
      Assert.Equal( user.Email, updUser.Email );
      Assert.Equal( user.Properties[ "Id" ], updUser.Properties[ "Id" ] );
    }

    [Fact]
    public void TestUpdateUserCallback()
    {
      user.Properties[ "Id" ] = "321";
      Backendless.UserService.Update( user, new AsyncCallback<BackendlessUser>(
      updUser=>
      {
        Assert.NotNull( user );
        Assert.Equal( user.Email, updUser.Email );
        Assert.Equal( user.Properties[ "Id" ], updUser.Properties[ "Id" ] );
      },
      fault=>
      {
        Assert.True( false, "Something went wrong during method execution" );
      } ) );
    }

    [Fact]
    public async void TestUpdateUserAsync()
    {
      user.Properties[ "Id" ] = "321";
      var updUser = await Backendless.UserService.UpdateAsync( user );

      Assert.NotNull( user );
      Assert.Equal( user.Email, updUser.Email );
      Assert.Equal( user.Properties[ "Id" ], updUser.Properties[ "Id" ] );
    }
  }
}
