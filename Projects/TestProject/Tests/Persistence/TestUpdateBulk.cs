using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestUpdateBulk : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestUpdateBulk()
    {
      person[ "age" ] = 18;
      person[ "name" ] = "Alexandra";
      Backendless.Data.Of( "Person" ).Save( person );
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestBulkUpdateBlockCall_ClassImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Backendless.Data.Of<Person>().Update( "age = '18'", person );
      Person updPerson = Backendless.Data.Of<Person>().Find()[ 0 ];

      Assert.NotNull( updPerson );
      Assert.True( (String) person[ "name" ] == updPerson.name );
      Assert.True( (Int32) person[ "age" ] == updPerson.age );
    }

    [Fact]
    public void TestBulkUpdateCallback_ClassImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Backendless.Data.Of<Person>().Update( "age = '18'", person, new AsyncCallback<Int32>(
      count =>
      {
        Person updPerson = Backendless.Data.Of<Person>().Find()[ 0 ];

        Assert.NotNull( updPerson );
        Assert.True( (String) person[ "name" ] == updPerson.name );
        Assert.True( (Int32) person[ "age" ] == updPerson.age );
      },
      fault =>
      {
        Assert.True( false, "Received an error while executing the 'Update' method" );
      } ) );
    }

    [Fact]
    public async void TestBulkUpdateAsync_ClassImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      await Backendless.Data.Of<Person>().UpdateAsync( "age = '18'", person );
      Person updPerson = Backendless.Data.Of<Person>().Find()[ 0 ];

      Assert.NotNull( updPerson );
      Assert.True( (String) person[ "name" ] == updPerson.name );
      Assert.True( (Int32) person[ "age" ] == updPerson.age );
    }

    [Fact]
    public void TestBulkUpdateBlockCall_DictionaryImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Backendless.Data.Of( "Person" ).Update( "age = '18'", person );
      Dictionary<String, Object> updPerson = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( updPerson );
      Assert.IsType<String>( updPerson[ "name" ] );
      Assert.IsType<Double>( updPerson[ "age" ] );
      Assert.True( person[ "name" ].Equals( updPerson[ "name" ] ) );
      Assert.True( Comparer.IsEqual( person[ "age" ], updPerson[ "age" ] ) );
    }

    [Fact]
    public void TestBulkUpdateCallback_DictionaryImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Backendless.Data.Of( "Person" ).Update( "age = 18", person, new AsyncCallback<Int32>(
      count =>
      {
        Dictionary<String, Object> updPerson = Backendless.Data.Of( "Person" ).Find()[ 0 ];

        Assert.NotNull( updPerson );
        Assert.IsType<String>( updPerson[ "name" ] );
        Assert.IsType<Double>( updPerson[ "age" ] );
        Assert.True( person[ "name" ].Equals( updPerson[ "name" ] ) );
        Assert.True( Comparer.IsEqual( person[ "age" ], updPerson[ "age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "Received an error while execution the 'Update' method" );
      } ) );
    }

    [Fact]
    public async void TestBulkUpdateAsync_DictionaryImpl()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      await Backendless.Data.Of( "Person" ).UpdateAsync( "age = '18'", person );
      Dictionary<String, Object> updPerson = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( updPerson );
      Assert.IsType<String>( updPerson[ "name" ] );
      Assert.IsType<Double>( updPerson[ "age" ] );
      Assert.True( person[ "name" ].Equals( updPerson[ "name" ] ) );
      Assert.True( Comparer.IsEqual( person[ "age" ], updPerson[ "age" ] ) );
    }
  }
}
