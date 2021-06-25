using Xunit;
using System;
using System.Collections.Generic;
using BackendlessAPI;
using TestProject.Tests.Utils;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;

namespace TestProject.Tests.UserService
{
  [Collection( "Tests" )]
  public class TestFindByRole : IDisposable
  {
    String roleName = "TestRole";
    String roleId;
    String userId;
    public TestFindByRole()
    {
      roleId = Test_sHelper.CreateRole( roleName );
      userId = Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" ).ObjectId;
      Test_sHelper.AssignRole( roleId, roleName, userId );
    }

    public void Dispose()
    {
      Test_sHelper.DeleteRole( roleId );
    }

    [Fact]
    public void TestFindByRole_BlockCall()
    {
      var listUsers = Backendless.UserService.FindByRole( roleName, false, null );

      Assert.NotNull( listUsers );
      Assert.Throws<KeyNotFoundException>( () => listUsers[ 0 ].Properties[ "roles" ] );
      Assert.Equal( 1.0, (Double) listUsers.Count );
    }

    [Fact]
    public async void TestFindByRole_AsyncCall()
    {
      var listUsers = await Backendless.UserService.FindByRoleAsync( roleName, false, null );

      Assert.NotNull( listUsers );
      Assert.Throws<KeyNotFoundException>( () => listUsers[ 0 ].Properties[ "roles" ] );
      Assert.Equal( 1.0, (Double) listUsers.Count );
    }

    [Fact]
    public void TestFindByRole_Callback()
    {
      Backendless.UserService.FindByRole( roleName, false, null, new AsyncCallback<IList<BackendlessUser>>(
      response =>
      {
        Assert.NotNull( response );
        Assert.Throws<KeyNotFoundException>( () => response[ 0 ].Properties[ "roles" ] );
        Assert.Equal( 1.0, (Double) response.Count );
      },
      fault =>
      {
        Assert.True( false, "Server reported an error..." );
      } ) );
    }

    [Fact]
    public void TestFindByRole_WithLoadRoles_BlockCall()
    {
      var listUsers = Backendless.UserService.FindByRole( roleName, true, null );

      Assert.NotNull( listUsers );
      Assert.NotNull( listUsers[ 0 ].Properties[ "roles" ] );
      Assert.True( ( ( (String[]) listUsers[ 0 ].Properties[ "roles" ] ).Length ) == 1 );
      Assert.Equal( 1.0, (Double) listUsers.Count );
    }

    [Fact]
    public async void TestFindByRole_WithLoadRoles_AsyncCall()
    {
      var listUsers = await Backendless.UserService.FindByRoleAsync( roleName, true, null );

      Assert.NotNull( listUsers );
      Assert.NotNull( listUsers[ 0 ].GetProperty( "roles" ) );
      Assert.True( ( ( (String[]) listUsers[ 0 ].Properties[ "roles" ] ).Length ) == 1 );
      Assert.Equal( 1.0, (Double) listUsers.Count );
    }

    [Fact]
    public void TestFindByRole_WithLoadRoles_Callback()
    {
      Backendless.UserService.FindByRole( roleName, true, null, new AsyncCallback<IList<BackendlessUser>>(
      response =>
      {
        Assert.NotNull( response );
        Assert.NotNull( response[ 0 ].GetProperty( "roles" ) );
        Assert.True( ( ( (String[]) response[ 0 ].Properties[ "roles" ] ).Length ) == 1 );
        Assert.Equal( 1.0, (Double) response.Count );
      },
      fault =>
      {
        Assert.True( false, "Server reported an error..." );
      } ) );
    }
    [Fact]
    public void TestFindByRole_DataQueryBuilder_BlockCall()
    {
      BackendlessUser user = new BackendlessUser();
      user.Email = "FindByRoleDQB@test.com";
      user.Password = "123234";
      user.ObjectId = Backendless.UserService.Register( user ).ObjectId;
      Test_sHelper.AssignRole( roleId, roleName, user.ObjectId );
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create().SetPageSize( 1 );

      var response = Backendless.UserService.FindByRole( roleName, false, queryBuilder );
      var entityForRemove = new Dictionary<String, Object>();
      entityForRemove[ "objectId" ] = user.ObjectId;
      Backendless.Data.Of( "Users" ).Remove( entityForRemove );

      Assert.True( response.Count == 1 );
    }

    [Fact]
    public async void TestFindByRole_DataQueryBuilder_AsyncCall()
    {
      BackendlessUser user = new BackendlessUser();
      user.Email = "FindByRoleDQB@test.com";
      user.Password = "123234";
      user.ObjectId = Backendless.UserService.Register( user ).ObjectId;
      Test_sHelper.AssignRole( roleId, roleName, user.ObjectId );
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create().SetPageSize( 1 );

      var response = await Backendless.UserService.FindByRoleAsync( roleName, false, queryBuilder );
      var entityForRemove = new Dictionary<String, Object>();
      entityForRemove[ "objectId" ] = user.ObjectId;
      Backendless.Data.Of( "Users" ).Remove( entityForRemove );

      Assert.True( response.Count == 1 );
    }

    [Fact]
    public void TestFindByRole_DataQueryBuilder_Callback()
    {
      BackendlessUser user = new BackendlessUser();
      user.Email = "FindByRoleDQB@test.com";
      user.Password = "123234";
      user.ObjectId = Backendless.UserService.Register( user ).ObjectId;
      Test_sHelper.AssignRole( roleId, roleName, user.ObjectId );
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create().SetPageSize( 1 );

      Backendless.UserService.FindByRole( roleName, false, queryBuilder, new AsyncCallback<IList<BackendlessUser>>(
      response=>
      {
        var entityForRemove = new Dictionary<String, Object>();
        entityForRemove[ "objectId" ] = user.ObjectId;
        Backendless.Data.Of( "Users" ).Remove( entityForRemove );

        Assert.True( response.Count == 1 );
      },
      fault=>
      {
        Assert.True( false, "Server reported an error..." );
      } ) );
    }
  }
}
