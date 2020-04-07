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
    public void TestExcludeName_Find()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddAllProperties();
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( !res[ 0 ].ContainsKey( "name" ) && !res[ 1 ].ContainsKey( "name" ) );
    }

    [TestMethod]
    public void TestExcludeTwoFields()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.ExcludeProperties( "name", "age" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      Assert.IsTrue( !res[ 0 ].ContainsKey( "name" ) && !res[ 0 ].ContainsKey( "age" ), "First object is contains keys 'name' or 'age'" );
      Assert.IsTrue( !res[ 1 ].ContainsKey( "name" ) && !res[ 1 ].ContainsKey( "age" ), "Second object is contains keys 'name' or 'age'" );
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
