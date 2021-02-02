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

      Person actual = Backendless.Data.Of<Person>().FindLast();

      Assert.NotNull( actual );
      Assert.NotNull( actual.objectId );
      Assert.NotEmpty( actual.objectId );
      Assert.True( Comparer.IsEqual( person.age, actual.age ), "The actual field 'age' is not equal expected" );
      Assert.Equal( person.name, actual.name );
    }

    [Fact]
    public void FindLast_Callback_Class()
    {
      Backendless.Data.Of<Person>().Save( person );

      Backendless.Data.Of<Person>().FindLast( new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.NotNull( actual.objectId );
        Assert.NotEmpty( actual.objectId );
        Assert.True( Comparer.IsEqual( person.age, actual.age ), "The actual field 'age' is not equal expected" );
        Assert.Equal( person.name, actual.name );
      },
      fault =>
      {
        Assert.True( false, "An error appeared durring the execution operation" );
      } ) );
    }

    [Fact]
    public async void FindLast_Async_Class()
    {
      Backendless.Data.Of<Person>().Save( person );

      Person actual = await Backendless.Data.Of<Person>().FindLastAsync();

      Assert.NotNull( actual );
      Assert.NotNull( actual.objectId );
      Assert.NotEmpty( actual.objectId );
      Assert.True( Comparer.IsEqual( person.age, actual.age ), "The actual field 'age' is not equal expected" );
      Assert.Equal( person.name, actual.name );
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