using Xunit;
using System;
using BackendlessAPI;

namespace TestProject
{
  [Collection("Tests")]
  public class TestStayLoggedInUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();
    public TestStayLoggedInUser()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
    }

    public void Dispose()
    {
      Backendless.UserService.Logout();
    }

    [Fact]
    public async void AsyncLoginTest()
    {
         BackendlessUser responseUser = await Backendless.UserService.LoginAsync( user.Email, user.Password, true );

         Assert.True( user.Email == responseUser.Email );
         Assert.True( Backendless.UserService.CurrentUser == responseUser );
    }

    [Fact]
    public void DefaultLoginTest()
    {
      BackendlessUser responseUser = Backendless.UserService.Login( user.Email, user.Password, true );

      Assert.True( user.Email == responseUser.Email );
      Assert.True( Backendless.UserService.CurrentUser == responseUser );
    }
  }
}