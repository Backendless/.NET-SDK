using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestFindFirstClass
  {
    [TestMethod]
    public void FFClassWithoutParameters()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of<Person>().Save( person );
      Person receivedPerson = Backendless.Data.Of<Person>().FindFirst();

      if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
      {
        Assert.IsTrue( receivedPerson.age == person.age );
        Assert.IsTrue( receivedPerson.name == person.name );
      }

      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [TestMethod]
    public void FFClassAsyncCallback()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of<Person>().Save( person );
      Backendless.Data.Of<Person>().FindFirst( new AsyncCallback<Person>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback.objectId ) )
        {
          Assert.IsTrue( callback.age == person.age );
          Assert.IsTrue( callback.name == person.name );
        }
      },
      fault =>
      {
        Assert.IsTrue( false );
      } ) );

      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [TestMethod]
    public void FFClassAsyncMethod()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.UserService.Logout();
      Backendless.Data.Of<Person>().Save( person );
      Task.Run( async () =>
      {
        Person receivedPerson = await Backendless.Data.Of<Person>().FindFirstAsync();

        if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
        {
          Assert.IsTrue( receivedPerson.age == person.age );
          Assert.IsTrue( receivedPerson.name == person.name );
        }
      } );
    }
  }
}
