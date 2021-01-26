using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindFirstClass : IDisposable
  {
    Person person = new Person();

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    public TestFindFirstClass()
    {
      person.age = 16;
      person.name = "Alexandra";
    }

    [Fact]
    public void FindFirstWithoutParameters_BlockCall_Class()
    {
      Backendless.Data.Of<Person>().Save( person );
      Person receivedPerson = Backendless.Data.Of<Person>().FindFirst();

      if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
      {
        Assert.True( receivedPerson.age == person.age, "Received person object is not equal actual" );
        Assert.True( receivedPerson.name == person.name, "Received person object is not equal actual" );
      }
      else
        Assert.True( false, "Callback's objectId is null" );
    }

    [Fact]
    public void FindFirst_Callback_Class()
    {
      Backendless.Data.Of<Person>().Save( person );
      Backendless.Data.Of<Person>().FindFirst( new AsyncCallback<Person>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback.objectId ) )
        {
          Assert.True( callback.age == person.age, "Received person object is not equal actual" );
          Assert.True( callback.name == person.name, "Received person object is not equal actual" );
        }
        else
          Assert.True( false, "Callback's objectId is null" );
      },
      fault =>
      {
        Assert.True( false, "Discrepancy between the expected and the actual" );
      } ) );
    }

    [Fact]
    public void FindFirst_Async_Class()
    {
      Backendless.Data.Of<Person>().Save( person );
      Task.Run( async () =>
      {
        Person receivedPerson = await Backendless.Data.Of<Person>().FindFirstAsync();

        if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
        {
          Assert.True( receivedPerson.age == person.age, "Received person object is not equal actual" );
          Assert.True( receivedPerson.name == person.name, "Received person object is not equal actual" );
        }
        else
          Assert.True( false, "Actual Person object is not equal expected" );
      } );
    }
  }
}