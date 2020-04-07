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

      foreach ( Dictionary<string, object> kvp in res )
        if ( !kvp.ContainsKey( "name" ) )
          Assert.IsTrue( true );
        else
          Assert.IsTrue( false );
    }

    [TestMethod]
    public void TestExcludeName_Find_WithStar()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.AddProperty( "trim( name )" );
      queryBuilder.ExcludeProperty( "name" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );
      foreach ( Dictionary<string, object> kvp in res )
        if ( !kvp.ContainsKey( "name" ) )
          Assert.IsTrue( true );
        else
          Assert.IsTrue( false );
    }

    [TestMethod]
    public void TestExcludeNameAndLocation_Find_Related()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "table_B.adress", "TIME(created)" );
      queryBuilder.ExcludeProperties( "name", "location" );

      IList<Dictionary<string, object>> res = Backendless.Data.Of( "A" ).Find( queryBuilder );

      foreach ( Dictionary<string, object> kvp in res )
        if ( ( String )kvp["adress"] == "Tom Street" || ( String ) kvp["adress"] == "Curse Street" )
        {
          if ( kvp.ContainsKey( "Time" ) )
            if ( !kvp.ContainsKey( "name" ) )
              Assert.IsTrue( true );
        }
        else
          Assert.IsTrue( false );
    }
  }
}
