using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
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
    public void FFClassWithoutParameters()
    {
      Backendless.Data.Of<Person>().Save( person );
      Person receivedPerson = Backendless.Data.Of<Person>().FindFirst();

      if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
      {
        Assert.True( receivedPerson.age == person.age );
        Assert.True( receivedPerson.name == person.name );
      }
      else
        Assert.True( false );
    }

    [Fact]
    public void FFClassAsyncCallback()
    {
      Backendless.Data.Of<Person>().Save( person );
      Backendless.Data.Of<Person>().FindFirst( new AsyncCallback<Person>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback.objectId ) )
        {
          Assert.True( callback.age == person.age );
          Assert.True( callback.name == person.name );
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
    public void FFClassAsyncMethod()
    {
      Backendless.Data.Of<Person>().Save( person );
      Task.Run( async () =>
      {
        Person receivedPerson = await Backendless.Data.Of<Person>().FindFirstAsync();

        if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
        {
          Assert.True( receivedPerson.age == person.age );
          Assert.True( receivedPerson.name == person.name );
        }
        else
          Assert.True( false );
      } );
    }
  }
}
