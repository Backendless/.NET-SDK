using Xunit;
using BackendlessAPI;
using BackendlessAPI.Async;

namespace TestProject.Tests.UserService
{
  [Collection( "Tests" )]
  public class TestRegisterUser
  {
    BackendlessUser user = new BackendlessUser();
    public TestRegisterUser()
    {
      user.Email = "TestEmail@gmail.com";
      user.Password = "123123";
    }

    [Fact]
    public void TestRegister_BlockCall()
    {
      var receivedUser = Backendless.UserService.Register( user );

      Assert.NotNull( receivedUser );
      Assert.IsType<BackendlessUser>( receivedUser );
      Assert.Equal( user.Email, receivedUser.Email );

      Backendless.Data.Of<BackendlessUser>().Remove( receivedUser );
    }

    [Fact]
    public void TestRegisterCallback()
    {
      Backendless.UserService.Register( user, new AsyncCallback<BackendlessUser>(
      receivedUser =>
      {
        Assert.NotNull( receivedUser );
        Assert.IsType<BackendlessUser>( receivedUser );
        Assert.Equal( user.Email, receivedUser.Email );

        Backendless.Data.Of<BackendlessUser>().Remove( receivedUser );
      },
      fault =>
      {
        Assert.True( false, "An error occurred while performing the operation" );
      } ) );
    }

    [Fact]
    public async void TestRegisterAsync()
    {
      var receivedUser = await Backendless.UserService.RegisterAsync( user );

      Assert.NotNull( receivedUser );
      Assert.IsType<BackendlessUser>( receivedUser );
      Assert.Equal( user.Email, receivedUser.Email );

      Backendless.Data.Of<BackendlessUser>().Remove( receivedUser );
    }
  }
}
