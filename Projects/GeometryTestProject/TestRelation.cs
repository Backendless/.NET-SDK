using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using BackendlessAPI.Async;

namespace GeometryTestProject
{
  public class Area
  {
    public int UserId{ get; set; }
    public string AreaA { get; set; }
    public bool Categories{ get; set; }
  }
  public class Order
  {
    public List<Area> Related{ get; set; }
    public string name{ get; set; }
    public int age { get; set; }
  }

  [TestClass]
  public class TestRelation
  {
    [TestMethod]
    public void TestRelations()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String>() { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );

      Assert.IsTrue( res[ 0 ].ContainsKey( "Related" ) );
      Assert.IsTrue( res[ 1 ].ContainsKey( "Related" ) );
    }

    [TestMethod]
    public void TestRelationDepthZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsDepth( 0 );
      qb.SetRelated( new List<String> { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );

      Assert.IsFalse( !res[ 0 ].ContainsKey( "Related" ) );
      Assert.IsFalse( !res[ 1 ].ContainsKey( "Related" ) );
    }

    [TestMethod]
    public void TestRelationsPageSizeOne()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( 1 );
      qb.SetRelated( new List<String> { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );
      Object[] rel = (Object[]) res[ 0 ][ "Related" ];

      Assert.IsTrue( rel.Length == 1 );
    }

    [TestMethod]
    public void TestRelationPageSizeZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( 0 );
      qb.SetRelated( new List<String> { "Related" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );
      Object[] rel = (Object[]) res[ 0 ][ "Related" ];

      Assert.IsTrue( rel.Length == 0 );
    }

    [TestMethod]
    public void TestRelationsWithClass()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String> { "Related" } );
      IList<Order> res = Backendless.Data.Of<Order>().Find( qb );

      Assert.IsTrue( res[0].Related.Count == 1 );
    }

    [TestMethod]
    public void TestSort()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelated( new List<String> { "Related" } );
      qb.SetSortBy( new List<String> { "age" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );

      Assert.IsTrue( (Double) res[ 0 ][ "age" ] < (Double) res[ 1 ][ "age" ] );
    }

    [TestMethod]
    public void TestWhereClauseZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetWhereClause( "name='Joe'" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );
      Assert.IsTrue( res.Count == 0 );
    }

    [TestMethod]
    public void TestWhereClauseNotZero()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetWhereClause( "Percentage > 30" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );

      foreach( Dictionary<String, Object> entry in res )
        Assert.IsTrue( (Double) entry[ "Percentage" ] > 30.0 );
    }

    [TestMethod]
    public void TestRelationsDepth()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsDepth( 1 );
      qb.SetRelated( new List<String> { "Country", "City" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );
      Object entry =  res[0]["Country"];
      Assert.IsFalse( ( (Dictionary<Object, Object>) entry ).ContainsKey( "Capital" ) );
    }

    [TestMethod]
    public void TestGroupBy()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.AddGroupBy( "Percentage" );
      Object gg = new Object();
      int i = 0;
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );
      foreach( Dictionary<String, Object> entry in res )
      {
        if( i == 0 )
        {
          gg = entry[ "Percentage" ];
          i++;
          continue;
        }
        Assert.IsTrue( (Double) gg < (Double) entry[ "Percentage" ] );
        gg = entry[ "Percentage" ];
      }
    }
  }
}