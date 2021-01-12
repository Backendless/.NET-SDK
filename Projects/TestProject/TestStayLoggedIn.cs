using Xunit;
using BackendlessAPI;
using System.Threading.Tasks;

namespace TestProject
{
  [Collection("Tests")]
  public class TestStayLoggedIn
  {
    BackendlessUser user = new BackendlessUser();
    public TestStayLoggedIn()
    {
      user.Email = "hdhdhd@gmail.com";
      user.Password = "123234";
    }

    [Fact]
    public void AsyncLoginTest()
    {
      Task.Run( async () =>
       {
         BackendlessUser responseUser = await Backendless.UserService.LoginAsync( user.Email, user.Password, true );

         Assert.True( user.Email == responseUser.Email );
         Assert.True( Backendless.UserService.CurrentUser == responseUser );
       } );
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