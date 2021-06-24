using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BackendlessAPI;
using BackendlessAPI.Exception;
using System.Linq;

namespace TestProject.Tests.UserService
{
  [Collection("Tests")]
  public class TestReloadCurrentUser : IDisposable
  {
    BackendlessUser user = new BackendlessUser();
    public TestReloadCurrentUser()
    {
      user.Email = "ReloadCurrentUser@test.com";
      user.Password = "123234";
      user.ObjectId = Backendless.UserService.Register( user ).ObjectId;
      Backendless.UserService.Login( user.Email, user.Password );
    }

    public void Dispose()
    {
      Backendless.UserService.Logout();
      Backendless.Data.Of<BackendlessUser>().Remove( user );
    }

    [Fact]
    public void TestReloadCurrentUser_BlockCall()
    {
      user = Backendless.Data.Of<BackendlessUser>().FindById( user.ObjectId );   
      var currentUser = Backendless.UserService.CurrentUser;

      if( !Comparer.IsEqual<String, Object>( user.Properties, currentUser.Properties ) )
        Assert.False( true, "Users are not equal" );

      user.Properties[ "name" ] = "Updated";
      Backendless.UserService.Update( user );
      Backendless.UserService.ReloadCurrentUserData();
      currentUser = Backendless.UserService.CurrentUser;

      Assert.False( Comparer.IsEqual<String, Object>( user.Properties, currentUser.Properties ) );
      Assert.False( String.IsNullOrEmpty( currentUser.Properties[ "name" ].ToString() ), "Current user's property 'name' is empty or null" );
      Assert.True( currentUser.Properties[ "name" ].ToString() == "Updated", "Actual current user's property is not equal expected" );
    }

    [Fact]
    public async void TestReloadCurrentUser_AsyncCall()
    {
      user = Backendless.Data.Of<BackendlessUser>().FindById( user.ObjectId );
      var currentUser = Backendless.UserService.CurrentUser;

      if( !Comparer.IsEqual<String, Object>( user.Properties, currentUser.Properties ) )
        Assert.False( true, "Users are not equal" );

      user.Properties[ "name" ] = "Updated";
      Backendless.UserService.Update( user );
      await Backendless.UserService.ReloadCurrentUserDataAsync();
      currentUser = Backendless.UserService.CurrentUser;

      Assert.False( Comparer.IsEqual<String, Object>( user.Properties, currentUser.Properties ) );
      Assert.False( String.IsNullOrEmpty( currentUser.Properties[ "name" ].ToString() ), "Current user's property 'name' is empty or null" );
      Assert.True( currentUser.Properties[ "name" ].ToString() == "Updated", "Actual current user's property is not equal expected" );
    }

    [Fact]
    public void TestReloadCurrentUser_NullCurrentUser()
    {
      Backendless.UserService.Logout();
      Assert.Throws<ArgumentNullException>( () => Backendless.UserService.ReloadCurrentUserData() );
    }

    [Fact]
    public void TestReloadCurrentUser_NullObjectId()
    {
      Backendless.UserService.CurrentUser.ObjectId = null;
      Assert.Throws<ArgumentNullException>( () => Backendless.UserService.ReloadCurrentUserData() );
    }
  }
}
