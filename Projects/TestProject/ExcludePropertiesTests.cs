using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;

namespace TestProject
{
  [TestClass]
  public class ExcludePropertiesTests
  {
    [ClassInitialize]
    public static void TestExcludeSetupData( TestContext context )
    {
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );

      ////////////Сreate of the parent table "Person"////////////

      Dictionary<String, Object> data = new Dictionary<String, Object>();
      data.Add( "name", "Joe" );
      data.Add( "age", 23 );

      Dictionary<String, Object> dataIdParent_1 = Backendless.Data.Of( "Person" ).Save( data );//First object in the "Person" table
                                                                                               //////////////////////////////////////////////////////////////////////////////////////

      data.Clear();
      data.Add( "name", "Tom" );
      data.Add( "age", 20 );

      Dictionary<String, Object> dataIdParent_2 = Backendless.Data.Of( "Person" ).Save( data );//Second object in the "Person" table
                                                                                               //////////////////////////////////////////////////////////////////////////////////////

      ////////////Сreate of the children table "Location"////////////

      data.Clear();
      data.Add( "adress", "Curse Street" );

      Dictionary<String, Object> dataIdChildren_1 = Backendless.Data.Of( "Location" ).Save( data );//First object in the "Location" table
                                                                                                   ////////////////////////////////////////////////////////////////////////////////////////

      data.Clear();
      data.Add( "adress", "Tom Street" );

      Dictionary<String, Object> dataIdChildren_2 = Backendless.Data.Of( "Location" ).Save( data );//Second object int the "Location" table
                                                                                                   ////////////////////////////////////////////////////////////////////////////////////////

      ///Сreating a connection between the objects "Order" and "Area"///

      Object[] children = new Object[] { dataIdChildren_1 };

      Backendless.Data.Of( "Person" ).SetRelation( dataIdParent_1, "Location:Location:n", children );//First relation
                                                                                                     ////////////////////////////////////////////////////////////////////////////////////

      children = new Object[] { dataIdChildren_2 };

      Backendless.Data.Of( "Person" ).SetRelation( dataIdParent_2, "Location:Location:n", children );//Second relation
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
      TestInitialization.DeleteTable( "Person" );
      TestInitialization.DeleteTable( "Location" );
    }

    [TestMethod]
    public void TestExcludeTwoFields()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*" );
      queryBuilder.ExcludeProperties( "name", "age" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

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

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

      Assert.IsTrue( res[ 0 ].ContainsKey( "myTime" ), "First object does not contain 'myTime' key" );
      Assert.IsTrue( res[ 1 ].ContainsKey( "myTime" ), "Second object does not contain 'myTime' key" );
    }

    [TestMethod]
    public void TestRelatedField()
    {
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "*", "Location.adress" );

      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "Person" ).Find( queryBuilder );

      Assert.IsTrue( res[ 0 ].ContainsKey( "adress" ), "First object does not contain 'adress' field" );
      Assert.IsTrue( res[ 1 ].ContainsKey( "adress" ), "Second object does not contain 'adress' field" );
    }
  }
}
