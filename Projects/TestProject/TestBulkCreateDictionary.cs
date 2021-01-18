using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;

namespace TestProject
{
  [Collection( "Tests" )]
  public class TestBulkCreateDictionary : IDisposable
  {
    List<Dictionary<String, Object>> people = new List<Dictionary<String, Object>>();
    Dictionary<String, Object> person_1 = new Dictionary<String, Object>();
    Dictionary<String, Object> person_2 = new Dictionary<String, Object>();
    public TestBulkCreateDictionary()
    {
      person_1[ "name" ] = "Elizabeth";
      person_1[ "age" ] = 20;
      person_2[ "name" ] = "Joe";
      person_2[ "age" ] = 18;
      people.Add( person_1 );
      people.Add( person_2 );
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age > '0'" );
    }

    [Fact]
    public void TestBulkCreateBlockCall()
    {
      Backendless.Data.Of( "Person" ).Create( people );

      IList<Dictionary<String, Object>> actual = Backendless.Data.Of( "Person" ).Find();

      Assert.NotNull( actual );
      Assert.NotEmpty( actual );
      Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 0 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 0 ][ "age" ] ) );
      Assert.True( person_1[ "name" ].Equals( actual[ 0 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 0 ][ "name" ] ) );
      Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 1 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 1 ][ "age" ] ) );
      Assert.True( person_1[ "name" ].Equals( actual[ 1 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 1 ][ "name" ] ) );
    }

    [Fact]
    public void TestBulkCreateCallback()
    {
      Backendless.Data.Of( "Person" ).Create( people, new AsyncCallback<IList<String>>(
      objectIds =>
      {
        IList<Dictionary<String, Object>> actual = Backendless.Data.Of( "Person" ).Find();

        Assert.NotNull( actual );
        Assert.NotEmpty( actual );
        Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 0 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 0 ][ "age" ] ) );
        Assert.True( person_1[ "name" ].Equals( actual[ 0 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 0 ][ "name" ] ) );
        Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 1 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 1 ][ "age" ] ) );
        Assert.True( person_1[ "name" ].Equals( actual[ 1 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 1 ][ "name" ] ) );
      },
      fault =>
       {
         Assert.True( false, "Something went wrong during the execution of the 'BulkCreate' operation" );
       } ) );
    }

    [Fact]
    public async void TestBulkCreateAsync()
    {
      await Backendless.Data.Of( "Person" ).CreateAsync( people );

      IList<Dictionary<String, Object>> actual = Backendless.Data.Of( "Person" ).Find();

      Assert.NotNull( actual );
      Assert.NotEmpty( actual );
      Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 0 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 0 ][ "age" ] ) );
      Assert.True( person_1[ "name" ].Equals( actual[ 0 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 0 ][ "name" ] ) );
      Assert.True( Comparer.IsEqual( person_1[ "age" ], actual[ 1 ][ "age" ] ) || Comparer.IsEqual( person_2[ "age" ], actual[ 1 ][ "age" ] ) );
      Assert.True( person_1[ "name" ].Equals( actual[ 1 ][ "name" ] ) || person_2[ "name" ].Equals( actual[ 1 ][ "name" ] ) );
    }
  }
}
