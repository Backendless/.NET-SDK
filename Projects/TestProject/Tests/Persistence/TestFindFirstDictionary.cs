using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Threading.Tasks;
using BackendlessAPI.Exception;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindFirstDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();

    public TestFindFirstDictionary()
    {
      person[ "age" ] = 16;
      person[ "name" ] = "Alexandra";
    }

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='16'" );
    }

    [Fact]
    public void FindFirstWithoutParameters_BlockCall_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Dictionary<String, Object> actual = Backendless.Data.Of( "Person" ).FindFirst();

      Assert.NotNull( actual );
      Assert.NotNull( actual[ "objectId" ] );
      Assert.NotEmpty( (String) actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ), "Actual field 'age' is not equal expected" );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }

    [Fact]
    public async void FindFirst_Async_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Dictionary<String, Object> actual = await Backendless.Data.Of( "Person" ).FindFirstAsync();

      Assert.NotNull( actual );
      Assert.NotNull( actual[ "objectId" ] );
      Assert.NotEmpty( (String) actual[ "objectId" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ), "Actual field 'age' is not equal expected" );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }

    [Fact]
    public void FindFirst_Callback_Dictionary()
    {
      Backendless.Data.Of( "Person" ).Save( person );

      Backendless.Data.Of( "Person" ).FindFirst( new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.NotNull( actual[ "objectId" ] );
        Assert.NotEmpty( (String) actual[ "objectId" ] );
        Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ), "Actual field 'age' is not equal expected" );
        Assert.Equal( person[ "name" ], actual[ "name" ] );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during execution operation" );
      } ) );
    }

    [Fact]
    public void FindFirstWrongTableName_BlockCall_Dictionary()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "WrongTableName" ).FindFirst() );
    }

    [Fact]
    public void FindFirstWrongTableName_Callback_Dictionary()
    {
      Backendless.Data.Of( "WrongTableName" ).FindFirst( new AsyncCallback<Dictionary<String, Object>>(
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
    public void FindFirstWrongTableName_Async_Dictionary()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "WrongTableName" ).FindFirstAsync() );
    }

    [Fact]
    public void FindFirstEmptyTable_BlockCall_Dictionary()
    {
      Assert.Throws<BackendlessException>( () => Backendless.Data.Of( "Person" ).FindFirst() );
    }

    [Fact]
    public void FindFirstEmptyTable_Callback_Dictionary()
    {
      Backendless.Data.Of( "Person" ).FindFirst( new AsyncCallback<Dictionary<String, Object>>(
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
    public void FindFirstEmptyTable_Async_Dictionary()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Person" ).FindFirstAsync() );
    }

  }
}