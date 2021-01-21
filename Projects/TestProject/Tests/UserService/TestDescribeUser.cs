using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Property;
using System.Collections.Generic;

namespace TestProject.Tests.UserService
{
  [Collection("Tests")]
  public class TestDescribeUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();
    public TestDescribeUser()
    {
      user.Email = "Testdescribeuser@gmail.com";
      user.Password = "123234";
      user.SetProperty( "Id", "123" );

      Backendless.UserService.Register( user );
      Backendless.UserService.Login( user.Email, user.Password );
    }

    public void Dispose()
    {
      Backendless.UserService.Logout();
      Backendless.Data.Of<BackendlessUser>().Remove( user );
    }

    [Fact]
    public void TestDescribeBlockCall()
    {
      var properties = Backendless.UserService.DescribeUserClass();

      Assert.NotNull( properties );
      Assert.Equal( 10, properties.Count );
    }

    [Fact]
    public void TestDescribeCallback()
    {
      Backendless.UserService.DescribeUserClass( new AsyncCallback<List<UserProperty>>(
      properties =>
      {
        Assert.NotNull( properties );
        Assert.Equal( 10, properties.Count );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during methods execution" );
      } ) );
    }

    [Fact]
    public async void TestDecribeAsync()
    {
      var properties = await Backendless.UserService.DescribeUserClassAsync();

      Assert.NotNull( properties );
      Assert.Equal( 10, properties.Count );
    }
  }
}
