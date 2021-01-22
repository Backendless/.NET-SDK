using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;

namespace TestProject.Tests.Persistence
{
  [Collection("Tests")]
  public class TestSaveClass : IDisposable
  {
    Person person = new Person();
    public TestSaveClass()
    {
      person.age = 20;
      person.name = "Elizabeth";
    }
    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age > '0'" );
    }

    [Fact]
    public void TestSaveBlockCall()
    {
      Person actual = Backendless.Data.Of<Person>().Save( person );

      Assert.NotNull( actual );
      Assert.Equal( person.age, actual.age );
      Assert.Equal( person.name, actual.name );
    }

    [Fact]
    public void TestSaveCallback()
    {
      Backendless.Data.Of<Person>().Save( person, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( person.age, actual.age );
        Assert.Equal( person.name, actual.name );
      },
      fault =>
      {
        Assert.True( false, "Error during 'Save' operation" );
      } ) );
    }

    [Fact]
    public async void TestSaveAsync()
    {
      Person actual = await Backendless.Data.Of<Person>().SaveAsync( person );
      
      Assert.NotNull( actual );
      Assert.Equal( person.age, actual.age );
      Assert.Equal( person.name, actual.name );
    }

    [Fact]
    public void TestUpdateBlockCall()
    {
      Person expected = Backendless.Data.Of<Person>().Save( person );
      expected.age = 21;

      Person actual = Backendless.Data.Of<Person>().Save( expected );

      Assert.NotNull( actual );
      Assert.Equal( expected.objectId, actual.objectId );
      Assert.True( Comparer.IsEqual( expected.age, actual.age ) );
    }

    [Fact]
    public void TestUpdateCallback()
    {
      Person expected = Backendless.Data.Of<Person>().Save( person );
      expected.age = 21;

      Backendless.Data.Of<Person>().Save( expected, new AsyncCallback<Person>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( expected.objectId, actual.objectId );
        Assert.True( Comparer.IsEqual( expected.age, actual.age ) );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the execution operation" );
      } ) );
    }

    [Fact]
    public async void TestUpdateAsync()
    {
      Person expected = Backendless.Data.Of<Person>().Save( person );
      expected.age = 21;

      Person actual = await Backendless.Data.Of<Person>().SaveAsync( expected );

      Assert.NotNull( actual );
      Assert.Equal( expected.objectId, actual.objectId );
      Assert.True( Comparer.IsEqual( expected.age, actual.age ) );
    }

    [Fact]
    public void TestUpdateWithWrongObjectIdBlockCall()
    {
      Person wrongPerson = Backendless.Data.Of<Person>().Save( person );
      wrongPerson.objectId = "The-wrong-objectId";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of<Person>().Save( wrongPerson ) );
    }

    [Fact]
    public void TestUpdateWithWrongObjectIdCallback()
    {
      Person wrongPerson = Backendless.Data.Of<Person>().Save( person );
      wrongPerson.objectId = "The-wrong-objectId";

      Backendless.Data.Of<Person>().Save( wrongPerson, new AsyncCallback<Person>(
      error =>
      {
        Assert.True( false, "An error was expected, but it was not" );
      },
      fault =>
      {
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
        Assert.True( true );
      } ) );
    }

    [Fact]
    public void TestUpdateWithWrongObjectIdAsync()
    {
      Person wrongPerson = Backendless.Data.Of<Person>().Save( person );
      wrongPerson.objectId = "The-wrong-objectId";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of<Person>().SaveAsync( wrongPerson ) );
    }
  }
}
