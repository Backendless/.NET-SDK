using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestStayLoggedIn
  {
#if !(NET_35 || NET_40)
    [TestMethod]
    public void AsyncLoginTest()
    {
      BackendlessUser user = new BackendlessUser();
      user.Email = "testcall@gmail.com";
      user.Password = "12345678";
      //Backendless.UserService.Register( user );
      Task.Run( async () =>
       {
         BackendlessUser responseUser = await Backendless.UserService.LoginAsync( user.Email, user.Password, true );
         Assert.IsTrue( user.Email == responseUser.Email );
         Assert.IsTrue( Backendless.UserService.CurrentUser == responseUser );
       } );
    }
#endif

    [TestMethod]
    public void DefaultLoginTest()
    {
      BackendlessUser user = new BackendlessUser();
      user.Email = "testcall@gmail.com";
      user.Password = "12345678";
      BackendlessUser responseUser = Backendless.UserService.Login( user.Email, user.Password, true );
      Assert.IsTrue( user.Email == responseUser.Email );
      Assert.IsTrue( Backendless.UserService.CurrentUser == responseUser );
    }
  }
}
