using Xunit;
using System;
using BackendlessAPI;
using BackendlessAPI.Async;
using BackendlessAPI.Persistence;
using System.Collections.Generic;

namespace TestProject
{
  [Collection("Tests")]
  public class TestFindByIdDictionary : IDisposable
  {
    Dictionary<String, Object> person = new Dictionary<String, Object>();
    public TestFindByIdDictionary()
    {
      person[ "age" ] = 15;
      person[ "name" ] = "Alexandra";
    }

    public void Dispose()
    {
      Backendless.Data.Of<Person>().Remove( "age='15'" );
    }

    [Fact]
    public void TestFindById_StringId()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( person, queryBuilder );
      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ) );
      Assert.True( result.ContainsKey( "age" ) );
    }

    [Fact]
    public void TestFindById_StringId_Async()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Backendless.Data.Of( "Person" ).FindById( person, queryBuilder, new AsyncCallback<Dictionary<String, Object>>(
      callback =>
      {
        Assert.NotNull( callback );
        Assert.False( callback.ContainsKey( "name" ) );
        Assert.True( callback.ContainsKey( "age" ) );
      },
      fault =>
      {
        throw new ArgumentException( "Error" );
      } ) );
    }

    [Fact]
    public void TestFindById_Dictionary()
    {
      person[ "objectId" ] = Backendless.Data.Of( "Person" ).Save( person )[ "objectId" ];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Dictionary<String, Object> result = Backendless.Data.Of( "Person" ).FindById( person, queryBuilder );

      Assert.NotNull( result );
      Assert.False( result.ContainsKey( "name" ) );
      Assert.True( result.ContainsKey( "age" ) );
    }

    [Fact]
    public void TestFindById_Dictionary_Async()
    {
      person["objectId"] = Backendless.Data.Of( "Person" ).Save( person )["objectId"];

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "age" );
      Backendless.Data.Of( "Person" ).FindById( person, queryBuilder, new AsyncCallback<Dictionary<string, object>>(
      callback =>
      {
        Assert.NotNull( callback );
        Assert.False( callback.ContainsKey( "name" ) );
        Assert.True( callback.ContainsKey( "age" ) );
      },
      fault =>
      {
        Assert.True( false );
      } ) );
    }
  }
}