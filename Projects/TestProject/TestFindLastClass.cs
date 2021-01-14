using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestFindLastClass : IDisposable
  {
    Person person = new Person();

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    public TestFindLastClass()
    {
      person.age = 16;
      person.name = "Alexandra";
    }

    [Fact]
    public void FLClassWithoutParameters()
    {
      Backendless.Data.Of<Person>().Save( person );
      Person receivedPerson = Backendless.Data.Of<Person>().FindLast();

      if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
      {
        Assert.True( receivedPerson.age == person.age, "Actual field 'age' is not equal expected" );
        Assert.True( receivedPerson.name == person.name, "Actual field 'name' is not equal expected" );
      }
      else
        Assert.True( false, "Person is null" );
    }

    [Fact]
    public void FLClassAsyncCallback()
    {
      Backendless.Data.Of<Person>().Save( person );
      Backendless.Data.Of<Person>().FindLast( new AsyncCallback<Person>(
      callback =>
      {
        if( !String.IsNullOrEmpty( callback.objectId ) )
        {
          Assert.True( callback.age == person.age, "Actual field 'age' is not equal expected" );
          Assert.True( callback.name == person.name, "Actual field 'name' is not equal expected" );
        }
        else
          Assert.True( false, "Person's objectId is null" );
      },
      fault =>
      {
        Assert.True( false, "Person is null" );
      } ) );
    }

    [Fact]
    public void FLClassAsyncMethod()
    {
      Backendless.Data.Of<Person>().Save( person );
      Task.Run( async () =>
       {
         Person receivedPerson = await Backendless.Data.Of<Person>().FindLastAsync();

         if( !String.IsNullOrEmpty( receivedPerson.objectId ) )
         {
           Assert.True( receivedPerson.age == person.age, "Actual field 'age' is not equal expected" );
           Assert.True( receivedPerson.name == person.name, "Actual field 'name' is not equal expected" );
         }
         else
           Assert.True( false, "Person object is null" );
       } );
    }
  }
}