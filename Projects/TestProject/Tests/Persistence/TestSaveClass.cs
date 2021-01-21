using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;

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
  }
}
