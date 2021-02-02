using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Exception;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindLastDictionary
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestFindLastDictionary()
    {
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";
    }

    [Fact]
    public void FindLast_BlockCall_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Dictionary<String, Object> actual = Backendless.Data.Of( "Person" ).FindLast();

      Assert.NotNull( actual );
      Assert.NotNull( actual[ "objectId" ] );
      Assert.NotEmpty( (String) actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( actual[ "age" ], person[ "age" ] ), "Actual field 'age' is not equal expected" );
      Assert.True( actual[ "name" ].ToString() == person[ "name" ].ToString(), "Actual field 'name' is not equal expected" );

      Backendless.Data.Of<Person>().Remove( "age>'0'" );
    }

    [Fact]
    public void FindLast_Callback_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Backendless.Data.Of( "Person" ).FindLast( new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.NotNull( actual[ "objectId" ] );
        Assert.NotEmpty( (String) actual[ "objectId" ] );
        Assert.True( Comparer.IsEqual( actual[ "age" ], person[ "age" ] ), "Actual field 'age' is not equal expected" );
        Assert.Equal( actual[ "name" ], person[ "name" ] );

        Backendless.Data.Of<Person>().Remove( "age>'0'" );
      },
      fault =>
      {
        Assert.True( false, "Person is null" );
      } ) );
    }

    [Fact]
    public async void FindLast_Async_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Dictionary<String, Object> actual = await Backendless.Data.Of( "Person" ).FindLastAsync();

      Assert.NotNull( actual );
      Assert.NotNull( actual[ "objectId" ] );
      Assert.NotEmpty( (String) actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( actual[ "age" ], person[ "age" ] ), "Actual field 'age' is not equal expected" );
      Assert.Equal( actual[ "name" ], person[ "name" ] );

      Backendless.Data.Of<Person>().Remove( "age>'0'" );
    }

    [Fact]
    public void FindLastEmptyTable_BlockCall_Dictionary()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).FindLast() );
    }

    [Fact]
    public void FindLastEmptyTable_Callback_Dictionary()
    {
      Backendless.Data.Of( "Person" ).FindLast( new AsyncCallback<Dictionary<String, Object>>(
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
    public void FindLastEmptyTable_Async_Dictionary()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).FindLastAsync() );
    }

    [Fact]
    public void FindLastWrongTableName_BlockCall_Dictionary()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Wrong-table-name" ).FindLast() );
    }

    [Fact]
    public void FindLastWrongTableName_Callback_Dictionary()
    {
      Backendless.Data.Of( "Wrong-table-name" ).FindLast( new AsyncCallback<Dictionary<String, Object>>(
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
    public void FindLastWrongTableName_Async_Dictionary()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Wrong-table-name" ).FindLastAsync() );
    }
  }
}