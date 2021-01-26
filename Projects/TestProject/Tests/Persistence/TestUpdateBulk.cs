using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;

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
    public void TestBulkUpdate_BlockCall_Class()
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
    public void TestBulkUpdate_Callback_Class()
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
    public async void TestBulkUpdateAsync_Class()
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
    public void TestBulkUpdate_BlockCall_Dictionary()
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
    public void TestBulkUpdate_Callback_Dictionary()
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
    public async void TestBulkUpdateAsync_Dictionary()
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

    [Fact]
    public void TestBulkUpdateWrongTableName_BlockCall_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "PersonWrong" ).Update( "age='18'", person ) );
      Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( changes );
      Assert.NotEmpty( changes );
      Assert.Equal( "Alexandra", changes[ "name" ] );
      Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
    }
    
    [Fact]
    public void TestBulkUpdateWrongTableName_Callback_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Backendless.Data.Of( "PersonWrong" ).Update( "age='18'", person, new AsyncCallback<Int32>(
      nullable =>
      {
        Assert.True( false, "Expected error didn't occur" );
      },
      fault =>
      {
        Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];

        Assert.NotNull( fault );
        Assert.NotNull( fault.Message );
        Assert.NotEmpty( fault.Message );
        Assert.Equal( "Alexandra", changes[ "name" ] );
        Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
      } ) );
    }

    [Fact]
    public void TestBulkUpdateWrongTableNameAsync_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "PersonWrong" ).UpdateAsync( "age='18'", person ) );
      Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( changes );
      Assert.NotEmpty( changes );
      Assert.Equal( "Alexandra", changes[ "name" ] );
      Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
    }

    [Fact]
    public void TestBulkUpdateWrongFieldName_BlockCall_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";
      person[ "Wrong_+" ] = "Wrong-column-name";

      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).Update( "age='18'", person ) );
      Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( changes );
      Assert.NotEmpty( changes );
      Assert.Equal( "Alexandra", changes[ "name" ] );
      Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
    }

    [Fact]
    public void TestBulkUpdateWrongFieldName_Callback_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";
      person[ "Wrong_+" ] = "Wrong-column-name";

      Backendless.Data.Of( "Person" ).Update( "age='18'", person, new AsyncCallback<Int32>(
      nullable =>
      {
        Assert.True( false, "Expected error didn't occur" );
      },
      fault =>
      {
        Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];
        Assert.NotNull( changes );
        Assert.NotEmpty( changes );
        Assert.Equal( "Alexandra", changes[ "name" ] );
        Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
      } ) ); 
    }

    [Fact]
    public void TestBulkUpdateWrongFieldNameAsync_Dictionary()
    {
      person[ "age" ] = 20;
      person[ "name" ] = "Elizabeth";
      person[ "Wrong_+" ] = "Wrong-column-name";

      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).UpdateAsync( "age='18'", person ) );
      Dictionary<String, Object> changes = Backendless.Data.Of( "Person" ).Find()[ 0 ];

      Assert.NotNull( changes );
      Assert.NotEmpty( changes );
      Assert.Equal( "Alexandra", changes[ "name" ] );
      Assert.True( Comparer.IsEqual( 18, changes[ "age" ] ) );
    }

    [Fact]
    public void TestBulkUpdateWithObjectId_Dictionary()
    {
    }
  }
}
