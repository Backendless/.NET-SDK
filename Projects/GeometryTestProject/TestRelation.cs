using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using BackendlessAPI.Async;

namespace GeometryTestProject
{
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
    public void TestRelationsPageSize()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( 15);
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );
    }

    [TestMethod]
    public void TestRelationDepthNULL()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsDepth( null );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );

      Assert.IsFalse( res[ 0 ].ContainsKey( "Related" ) );
      Assert.IsFalse( res[ 1 ].ContainsKey( "Related" ) );
    }

    [TestMethod]
    public void TestRelationPageSizeNULL()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.SetRelationsPageSize( null );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Order" ).Find( qb );
    }
  }
}