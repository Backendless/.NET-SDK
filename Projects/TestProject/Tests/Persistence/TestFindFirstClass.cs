using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

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
    public void FindFirst_BlockCall_Class()
    {
      Backendless.Data.Of<Person>().Save( person );

      Person actual = Backendless.Data.Of<Person>().FindFirst();

      Assert.NotNull( actual );
      Assert.NotNull( actual.objectId );
      Assert.NotEmpty( actual.objectId );
      Assert.True( Comparer.IsEqual( actual.age, person.age ) );
      Assert.Equal( person.name, actual.name );
    }

    [Fact]
    public void FindFirst_Callback_Class()
    {
      Backendless.Data.Of<Person>().Save( person );

      Backendless.Data.Of<Person>().FindFirst( new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.NotNull( actual.objectId );
        Assert.NotEmpty( actual.objectId );
        Assert.True( Comparer.IsEqual( person.age, actual.age ) );
        Assert.Equal( person.name, actual.name );
      },
      fault =>
      {
        Assert.True( false, "The expected object didn't equal actual" );
      } ) );
    }

    [Fact]
    public async void FindFirst_Async_Class()
    {
      Backendless.Data.Of<Person>().Save( person );

      Person actual = await Backendless.Data.Of<Person>().FindFirstAsync();

      Assert.NotNull( actual );
      Assert.NotNull( actual.objectId );
      Assert.NotEmpty( actual.objectId );
      Assert.True( Comparer.IsEqual( person.age, actual.age ) );
      Assert.Equal( person.name, actual.name );
    }

    [Fact]
    public void FindFirstWrongTableName_BlockCall_Class()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Area>().FindFirst() );
    }

    [Fact]
    public void FindFirstWrongTableName_Callback_Class()
    {
      Backendless.Data.Of<Area>().FindFirst( new AsyncCallback<Area>(
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
    public void FindFirstWrongTableName_Async_Class()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Area>().FindFirstAsync() );
    }

    [Fact]
    public void FindFirstEmptyTable_BlockCall_Class()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Person>().FindFirst() );
    }

    [Fact]
    public void FindFirstEmptyTable_Callback_Class()
    {
      Backendless.Data.Of<Person>().FindFirst( new AsyncCallback<Person>(
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
    public void FindFirstEmptyTable_Async_Class()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Person>().FindFirstAsync() );
    }
  }
}