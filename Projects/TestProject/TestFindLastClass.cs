using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestFindLastClass
  {
    [ClassInitialize]
    public static void ClassInitialize( TestContext context )
    {
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
      TestInitialization.DeleteTable( "Person" );
    }

    [TestCleanup]
    public void TestCleanup()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [TestMethod]
    public void FLClassWithoutParameters()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.Data.Of<Person>().Save( person );
      Person receivedPerson = Backendless.Data.Of<Person>().FindLast();

      if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
      {
        Assert.IsTrue( receivedPerson.age == person.age );
        Assert.IsTrue( receivedPerson.name == person.name );
      }
    }

    [TestMethod]
    public void FLClassAsyncCallback()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.Data.Of<Person>().Save( person );
      Backendless.Data.Of<Person>().FindLast( new AsyncCallback<Person>(
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
    }

#if !(NET_35 || NET_40)
    [TestMethod]
    public void FLClassAsyncMethod()
    {
      Person person = new Person();
      person.age = 16;
      person.name = "Alexandra";

      Backendless.Data.Of<Person>().Save( person );
      Task.Run( async () =>
       {
         Person receivedPerson = await Backendless.Data.Of<Person>().FindLastAsync();

         if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
         {
           Assert.IsTrue( receivedPerson.age == person.age );
           Assert.IsTrue( receivedPerson.name == person.name );
         }
       } );
    }
#endif
  }
}
