using Xunit;
using System;
using BackendlessAPI;
using System.Collections.Generic;
using BackendlessAPI.Async;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestSaveDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();

    public TestSaveDictionary()
    {
      person[ "name" ] = "Elizabeth";
      person[ "age" ] = 20;
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestSaveBlockCall()
    {
      Dictionary<String, Object> actual = Backendless.Data.Of( "Person" ).Save( person );


      Assert.NotNull( actual );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }

    [Fact]
    public void TestSaveCallback()
    {
      Backendless.Data.Of( "Person" ).Save( person, new AsyncCallback<Dictionary<String, Object>>(
        actual =>
        {
          Assert.NotNull( actual );
          Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
          Assert.Equal( person[ "name" ], actual[ "name" ] );
        },
        fault =>
        {
          Assert.True( false, "Something went wrong during the execution of the 'Save' method" );
        } ) );
    }

    [Fact]
    public async void TestSaveAsync()
    {
      Dictionary<String, Object> actual = await Backendless.Data.Of( "Person" ).SaveAsync( person );

      Assert.NotNull( person );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
    }
  }
}
