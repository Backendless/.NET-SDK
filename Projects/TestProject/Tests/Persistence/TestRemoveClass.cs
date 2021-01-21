using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestRemoveClass
  {
    Person person = new Person();
    public TestRemoveClass()
    {
      person.name = "Alexandra";
      person.age = 18;

      person.objectId = Backendless.Data.Of<Person>().Save( person ).objectId;
    }

    [Fact]
    public void TestRemoveEntityBlockCall()
    {
      Backendless.Data.Of<Person>().Remove( person );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveEntityCallback()
    {
      Backendless.Data.Of<Person>().Remove( person, new AsyncCallback<Int64>(
      count=>
      {
        IList<Person> actual = Backendless.Data.Of<Person>().Find();

        Assert.Empty( actual );
      },
      fault=>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ));
    }

    [Fact]
    public void TestRemoveClause()
    {
      Backendless.Data.Of<Person>().Remove( "age = '18'" );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public void TestRemoveClauseCallback()
    {
      Backendless.Data.Of<Person>().Remove( "age = '18'", new AsyncCallback<Int32>(
      count =>
      {
        IList<Person> actual = Backendless.Data.Of<Person>().Find();

        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "Something went wrong during the 'Remove' operation" );
      } ) );
    }

    [Fact]
    public async void TestRemoveEntityAsync()
    {
      await Backendless.Data.Of<Person>().RemoveAsync( person );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }

    [Fact]
    public async void TestRemoveClauseAsync()
    {
      await Backendless.Data.Of<Person>().RemoveAsync( "age = '18'" );

      IList<Person> actual = Backendless.Data.Of<Person>().Find();

      Assert.Empty( actual );
    }
  }
}