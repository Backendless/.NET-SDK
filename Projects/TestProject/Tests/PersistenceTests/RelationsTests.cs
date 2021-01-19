using Xunit;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;

namespace TestProject
{
  [Collection( "Tests" )]
  public class RelationsTests : IClassFixture<RelationsTestsInitializator>
  {
    [Fact]
    public void TestRelations()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String>() { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );

      Assert.True( res[ 0 ].ContainsKey( "Related" ) );
      Assert.True( res[ 1 ].ContainsKey( "Related" ) );
    }

    [Fact]
    public void TestRelationDepthZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsDepth( 0 );
      qb.SetRelated( new List<String> { "Related" } );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );

      Assert.False( !res[ 0 ].ContainsKey( "Related" ) );
      Assert.False( !res[ 1 ].ContainsKey( "Related" ) );
    }

    [Fact]
    public void TestRelationsPageSizeOne()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( 1 );
      qb.SetRelated( new List<String> { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );

      Object[] rel = (Object[]) res[ 0 ][ "Related" ];

      Assert.True( rel.Length == 1 );
    }

    [Fact]
    public void TestRelationPageSizeZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( 0 );
      qb.SetRelated( new List<String> { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );
      Object[] rel = (Object[]) res[ 0 ][ "Related" ];

      Assert.True( rel.Length == 0 );
    }

    [Fact]
    public void TestRelationsWithClass()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String> { "Related" } );
      IList<Human> res = Backendless.Data.Of<Human>().Find( qb );

      Assert.True( res[ 0 ].Related.Count == 1 );
    }

    [Fact]
    public void TestSort()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String> { "Related" } );
      qb.SetSortBy( new List<String> { "age" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );

      Assert.True( (Double) res[ 0 ][ "age" ] < (Double) res[ 1 ][ "age" ] );
    }

    [Fact]
    public void TestWhereClauseZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetWhereClause( "name='Joe'" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Human" ).Find( qb );

      Assert.True( res.Count == 0 );
    }

    [Fact]
    public void TestWhereClauseNotZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetWhereClause( "Percentage > 30" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );

      foreach( Dictionary<String, Object> entry in res )
        Assert.True( (Double) entry[ "Percentage" ] > 30.0 );
    }

    [Fact]
    public void TestRelationsDepth()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsDepth( 1 );
      qb.SetRelated( new List<String> { "Country" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );

      if( res[ 0 ].ContainsKey( "Country" ) )
      {
        Object entry = res[ 0 ][ "Country" ];
        Assert.False( ( (Dictionary<Object, Object>) entry ).ContainsKey( "Capital" ) );
      }
      else
        Assert.False( true );
    }

    [Fact]
    public void TestGroupBy()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.AddGroupBy( "Percentage" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );
      foreach( Dictionary<String, Object> entry in res )
        Assert.True( entry.ContainsKey( "Percentage" ) );
    }
  }
}