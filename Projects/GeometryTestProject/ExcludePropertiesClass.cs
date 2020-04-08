using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeometryTestProject
{
  [TestClass]
  public class ExcludePropertiesClass
  {
    [TestMethod]
    public void TestExcludeTwoFields()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.ExcludeProperties( "name", "age" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsFalse( res[ 0 ].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.IsFalse( res[ 0 ].ContainsKey( "age" ), "First object is contains key 'age'" );

      Assert.IsFalse( res[ 1 ].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.IsFalse( res[ 1 ].ContainsKey( "age" ), "First object is contains key 'age'" );
    }

    [TestMethod]
    public void TestCreateFieldTIME()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "TIME(created) as myTime" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( res[ 0 ].ContainsKey( "myTime" ), "First object does not contain 'myTime' key" );
      Assert.IsTrue( res[ 1 ].ContainsKey( "myTime" ), "Second object does not contain 'myTime' key" );
    }

    [TestMethod]
    public void TestRelatedField()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( res[ 0 ].ContainsKey("adress"), "First object does not contain 'adress' field" );
      Assert.IsTrue( res[ 1 ].ContainsKey("adress"), "Second object does not contain 'adress' field" );
    }
  }
}
