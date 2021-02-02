using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using System.Collections.Generic;
using BackendlessAPI.Exception;

namespace TestProject.Tests.Persistence
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
    public void TestBulkCreate_BlockCall()
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
    public void TestBulkCreate_Callback()
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
    public async void TestBulkCreate_Async()
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

    [Fact]
    public void TestBulkCreateEmptyObjects_BlockCall()
    {
      people.Clear();

      Backendless.Data.Of( "Person" ).Create( people );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of( "Person" ).Find();

      Assert.NotNull( actual );
      Assert.Empty( actual );
    }

    [Fact]
    public void TestBulkCreateEmptyObjects_Callback()
    {
      people.Clear();

      Backendless.Data.Of( "Person" ).Create( people, new AsyncCallback<IList<String>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Empty( actual );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestBulkCreateEmptyObjects_Async()
    {
      people.Clear();

      await Backendless.Data.Of( "Person" ).CreateAsync( people );
      IList<Dictionary<String, Object>> actual = Backendless.Data.Of( "Person" ).Find();

      Assert.NotNull( actual );
      Assert.Empty( actual );
    }

    [Fact]
    public void TestBulkCreateWrongTableName_BlockCall()
    {
      Assert.Throws<BackendlessException>( ()=> Backendless.Data.Of( "Wrong-table-name" ).Create( people ) );
    }

    [Fact]
    public void TestBulkCreateWrongTableName_Callback()
    {
      Backendless.Data.Of( "Wrong-table-name" ).Create( people, new AsyncCallback<IList<String>>(
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
    public void TestBulkCreateWrongTableName_Async()
    {
      Assert.ThrowsAsync<BackendlessException>( async () => await Backendless.Data.Of( "Wrong-table-name" ).CreateAsync( people ) );
    }

    [Fact]
    public void TestBulkCreateWrongFieldName_BlockCall()
    {
      person_1[ "New+_" ] = "wrong-field-name";

      Backendless.Data.Of( "Person" ).Create( people );
      var actual = Backendless.Data.Of( "Person" ).Find();

      Assert.False( actual[ 0 ].ContainsKey( "New+_" ) );
      Assert.False( actual[ 1 ].ContainsKey( "New+_" ) );
    }

    [Fact]
    public void TestbulkCreateWrongFieldName_Callback()
    {
      person_1[ "New+_" ] = "wrong-field-name";

      Backendless.Data.Of( "Person" ).Create( people, new AsyncCallback<IList<String>>(
      nullable =>
      {
        var actual = Backendless.Data.Of( "Person" ).Find();

        Assert.False( actual[ 0 ].ContainsKey( "New+_" ) );
        Assert.False( actual[ 1 ].ContainsKey( "New+_" ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the exectuion of the operation" );
      } ) );
    }

    [Fact]
    public async void TestBulkCreateWrongFieldName_Async()
    {
      person_1[ "New+_" ] = "wrong-field-name";

      Backendless.Data.Of( "Person" ).Create( people );
      var actual = await Backendless.Data.Of( "Person" ).FindAsync();

      Assert.False( actual[ 0 ].ContainsKey( "New+_" ) );
      Assert.False( actual[ 1 ].ContainsKey( "New+_" ) );
    }
  }
}
