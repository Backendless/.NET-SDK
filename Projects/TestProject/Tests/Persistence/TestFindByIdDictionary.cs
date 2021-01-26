using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using System.Collections.Generic;

namespace TestProject.Tests.Persistence
{
  [Collection( "Tests" )]
  public class TestFindByIdDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestFindByIdDictionary()
    {
      person[ "age" ] = 18;
      person[ "name" ] = "Joe";
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Person" ).Remove( "age>'0'" );
    }

    [Fact]
    public void TestFindById_BlockCall_Dictionary()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      var actual = Backendless.Data.Of( "Person" ).FindById( person );

      Assert.NotNull( actual );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestFindById_Callback_Dictionary()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of( "Person" ).FindById( person, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( person[ "name" ], actual[ "name" ] );
        Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_Dictionary()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      var actual = await Backendless.Data.Of( "Person" ).FindByIdAsync( person );

      Assert.NotNull( actual );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestFindById_BlockCall_StringId()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      var actual = Backendless.Data.Of( "Person" ).FindById( id );

      Assert.NotNull( actual );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestFindById_Callback_StringId()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      Backendless.Data.Of( "Person" ).FindById( id, new AsyncCallback<Dictionary<String, Object>>(
      actual =>
      {
        Assert.NotNull( actual );
        Assert.Equal( person[ "name" ], actual[ "name" ] );
        Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
      },
      fault =>
      {
        Assert.True( false, "An error appeared during the execution of the operation" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_StringId()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      var actual = await Backendless.Data.Of( "Person" ).FindByIdAsync( id );

      Assert.NotNull( actual );
      Assert.Equal( person[ "name" ], actual[ "name" ] );
      Assert.True( Comparer.IsEqual( person[ "age" ], actual[ "age" ] ) );
    }

    [Fact]
    public void TestFindById_BlockCall_StringId_DQB()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( id, queryBuilder );

      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ), "Person is not contain 'name' key" );
      Assert.True( result.ContainsKey( "age" ), "Person is not contain 'age' key" );
    }

    [Fact]
    public void TestFindById_Callback_StringId_DQB()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Backendless.Data.Of( "Person" ).FindById( id, queryBuilder, new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        Assert.NotNull( callback );
        Assert.False( callback.ContainsKey( "name" ), "Person is not contain 'name' key" );
        Assert.True( callback.ContainsKey( "age" ), "Person is not contain 'age' key" );
      },
      fault =>
      {
        Assert.True( false, "Person not found" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_StringId_DQB()
    {
      String id = (String) Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Dictionary<String, Object> result = await Backendless.Data.Of( "Person" ).FindByIdAsync( id, queryBuilder );

      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ), "Person is not contain 'name' key" );
      Assert.True( result.ContainsKey( "age" ), "Person is not contain 'age' key" );
    }

    [Fact]
    public void TestFindById_BlockCall_Dictionary_DQB()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( person, queryBuilder );

      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ), "Person is not contain 'name' key" );
      Assert.True( result.ContainsKey( "age" ), "Person is not contain 'age' key" );
    }

    [Fact]
    public void TestFindById_Callback_Dictionary_DQB()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Backendless.Data.Of( "Person" ).FindById( person, queryBuilder, new AsyncCallback<Dictionary<string, object>>(
      callback =>
      {
        Assert.NotNull( callback );
        Assert.False( callback.ContainsKey( "name" ), "Person is not contain 'name' key" );
        Assert.True( callback.ContainsKey( "age" ), "Person is not contain 'age' key" );
      },
      fault =>
      {
        Assert.True( false, "Person not found" );
      } ) );
    }

    [Fact]
    public async void TestFindById_Async_Dictionary_DQB()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );

      Dictionary<String, Object> result = await Backendless.Data.Of( "Person" ).FindByIdAsync( person, queryBuilder );

      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ), "Person is not contain 'name' key" );
      Assert.True( result.ContainsKey( "age" ), "Person is not contain 'age' key" );
    }
  }
}