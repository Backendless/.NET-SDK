using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
  [TestClass]
  public class ExcludePropertiesTests
  {
    [ClassInitialize]
    public static void TestExcludeSetupData( TestContext context )
    {
      try
      {
        Backendless.Data.Describe( "A" );
        Backendless.Data.Describe( "B" );
      }
      catch
      {
        ////////////Сreate of the parent table "A"////////////

        Dictionary<String, Object> data = new Dictionary<String, Object>();
        data.Add( "name", "Joe" );
        data.Add( "age", "23" );

        Dictionary<String, Object> dataIdParent_1 = Backendless.Data.Of( "A" ).Save( data );//First object in the "A" table
        //////////////////////////////////////////////////////////////////////////////////////

        data.Clear();
        data.Add( "name", "Tom" );
        data.Add( "age", 20 );

        Dictionary<String, Object> dataIdParent_2 = Backendless.Data.Of( "A" ).Save( data );//Second object in the "A" table
        //////////////////////////////////////////////////////////////////////////////////////

        ////////////Сreate of the children table "B"////////////

        data.Clear();
        data.Add( "adress", "Curse Street" );

        Dictionary<String, Object> dataIdChildren_1 = Backendless.Data.Of( "B" ).Save( data );//First object in the "B" table
        ////////////////////////////////////////////////////////////////////////////////////////

        data.Clear();
        data.Add( "adress", "Tom Street" );

        Dictionary<String, Object> dataIdChildren_2 = Backendless.Data.Of( "B" ).Save( data );//Second object int the "B" table
        ////////////////////////////////////////////////////////////////////////////////////////

        ///Сreating a connection between the objects "Order" and "Area"///

        Object[] children = new Object[] { dataIdChildren_1 };

        Backendless.Data.Of( "A" ).SetRelation( dataIdParent_1, "table_B:B:n", children );//First relation
        ////////////////////////////////////////////////////////////////////////////////////

        children = new Object[] { dataIdChildren_2 };

        Backendless.Data.Of( "A" ).SetRelation( dataIdParent_2, "table_B:B:n", children );//Second relation
      }
    }

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
