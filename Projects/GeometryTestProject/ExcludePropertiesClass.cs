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

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( !res[ 0 ].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.IsTrue( !res[ 0 ].ContainsKey( "age" ), "First object is contains key 'age'" );

      Assert.IsTrue( !res[ 1 ].ContainsKey( "name" ), "First object is contains key 'name'" );
      Assert.IsTrue( !res[ 1 ].ContainsKey( "age" ), "First object is contains key 'age'" );
    }

    [TestMethod]
    public void TestCreateFieldTIME()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "TIME(created)" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( res[ 0 ].ContainsKey( "Time" ), "First object does not contain 'Time' key" );
      Assert.IsTrue( res[ 1 ].ContainsKey( "Time" ), "Second object does not contain 'Time' key" );
    }

    [TestMethod]
    public void TestRelatedField()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( (String) res[ 0 ][ "adress" ] == "Tom Street", "First object does not contain 'adress' field" );
      Assert.IsTrue( (String) res[ 1 ][ "adress" ] == "Curse Street", "Second object does not contain 'adress' field" );
    }
  }
}
