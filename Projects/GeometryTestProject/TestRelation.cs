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
    public void TestRelationDepth()
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
    public void TestRelationsDepth()
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

      Assert.IsTrue( (Double) res[ 0 ][ "age" ] == 5.0 );
      Assert.IsTrue( (Double) res[ 1 ][ "age" ] == 10.0 );
    }
  }
}