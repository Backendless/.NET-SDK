using System;

namespace TestProject.Tests.UserServiceTests
{
  public class UserServiceInitializator : IDisposable
  {
    public void Dispose()
    {
      BackendlessAPI.Backendless.UserService.Logout();
    }
  }
}
