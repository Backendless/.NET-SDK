using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Threading.Tasks;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindLastClass : IDisposable
  {
    Person person = new Person();

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age>'0'" );
    }

    public TestFindLastClass()
    {
      person.age = 16;
      person.name = "Alexandra";
    }

    [Fact]
    public void FindLast_BlockCall_Class()
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
    public void FindLast_Callback_Class()
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
    public void FindLast_Async_Class()
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

    [Fact]
    public void FindLastWrongTableName_BlockCall_Class()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Area>().FindLast() );
    }

    [Fact]
    public void FindLastWrongTableName_Callback_Class()
    {
      Backendless.Data.Of<Area>().FindLast( new AsyncCallback<Area>(
      nullable =>
      {
        Assert.True( false, "The expected error didn't occur" );
      },
      fault =>
      {
        Assert.NotNull( fault );
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
      } ) );
    }

    [Fact]
    public void FindLastWrongTableName_Async_Class()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Area>().FindLastAsync() );
    }

    [Fact]
    public void FindLastEmptyTable_BlockCall_Class()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Person>().FindLast() );
    }

    [Fact]
    public void FindLastEmptyTable_Callback_Class()
    {
      Backendless.Data.Of<Person>().FindLast( new AsyncCallback<Person>(
      nullable =>
      {
        Assert.True( false, "The expected error didn't occur" );
      },
      fault =>
      {
        Assert.NotNull( fault );
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
      } ) );
    }

    [Fact]
    public void FindLastEmptyTable_Async_Class()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Person>().FindLastAsync() );
    }
  }
}