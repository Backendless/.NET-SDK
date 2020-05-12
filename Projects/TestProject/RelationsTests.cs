using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using System;
using System.Collections.Generic;

namespace TestProject
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
  public class RelationsTests
  {
    [ClassInitialize]
    public static void TestRealtionSetupData( TestContext context )
    {
      try
      {
        Backendless.Data.Describe( "Order" );
        Backendless.Data.Describe( "Area" );
        Backendless.Data.Describe( "CountryLanguage" );
        Backendless.Data.Describe( "Country" );
        Backendless.Data.Describe( "Capital" );
      }
      catch
      {
        ////////////Сreation of the parent table "Order"////////////

        Dictionary<String, Object> data = new Dictionary<String, Object>();
        data.Add( "age", 10 );
        data.Add( "name", "Nikita" );

        Dictionary<String, Object> dataIdParent_1 = Backendless.Data.Of( "Order" ).Save( data );//First object in the "Order" table
        /////////////////////////////////////////////////////////////////////////////////////////
        data.Clear();
        data.Add( "age", 5 );
        data.Add( "name", "Tommy" );

        Dictionary<String, Object> dataIdParent_2 = Backendless.Data.Of( "Order" ).Save( data );//Second object in the "Order" table
        /////////////////////////////////////////////////////////////////////////////////////////

        ////////////Creation of the children table "Area"////////////

        data.Clear();
        data.Add( "AreaA", "Munich" );
        data.Add( "Categories", false );
        data.Add( "UserId", 3 );

        Dictionary<String, Object> dataIdChildren_1 = Backendless.Data.Of( "Area" ).Save( data );//First object in the "Area" table
        //////////////////////////////////////////////////////////////////////////////////////////
        data.Clear();
        data.Add( "AreaA", "London" );
        data.Add( "Categories", true );
        data.Add( "UserId", 6 );

        Dictionary<String, Object> dataIdChildren_2 = Backendless.Data.Of( "Area" ).Save( data );//Second object in the "Area" table
        //////////////////////////////////////////////////////////////////////////////////////////

        ///Сreating a connection between the objects "Order" and "Area"///

        Object[] children = new Object[] { dataIdChildren_1 };

        Backendless.Data.Of( "Order" ).SetRelation( dataIdParent_1, "Related:Area:n", children );//First relation
        //////////////////////////////////////////////////////////////////////////////////////////

        children = new Object[] { dataIdChildren_2 };

        Backendless.Data.Of( "Order" ).SetRelation( dataIdParent_2, "Related:Area:n", children );//Second relations
        ///////////////////////////////////////////////////////////////////////////////////////////
        
        ////////////Сreation of the table "CountryLanguage"////////////
        
        data.Clear();
        data.Add( "Percentage", 30.2 );
        dataIdParent_1.Clear();
        dataIdParent_1 = Backendless.Data.Of( "CountryLanguage" ).Save( data );

        data.Clear();
        data.Add( "Percentage", 37.2 );
        dataIdParent_2.Clear();
        dataIdParent_2 = Backendless.Data.Of( "CountryLanguage" ).Save( data );

        data.Clear();
        data.Add( "Percentage", 30.5 );
        Dictionary<String, Object> dataIdParent_3 = Backendless.Data.Of( "CountryLanguage" ).Save( data );

        data.Clear();
        data.Add( "Percentage", 27.7 );
        Dictionary<String, Object> dataIdParent_4 = Backendless.Data.Of( "CountryLanguage" ).Save( data );
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        ////////////Сreation of the table "Country"////////////
        
        data.Clear();
        data.Add( "City", "Kyiv" );
        dataIdChildren_1.Clear();
        dataIdChildren_1 = Backendless.Data.Of( "Country" ).Save( data );

        data.Clear();
        data.Add( "City", "London" );
        dataIdChildren_2.Clear();
        dataIdChildren_2 = Backendless.Data.Of( "Country" ).Save( data );

        data.Clear();
        data.Add( "City", "Warsaw" );
        Dictionary<String, Object> dataIdChildren_3 = Backendless.Data.Of( "Country" ).Save( data );

        data.Clear();
        data.Add( "City", "Rome" );
        Dictionary<String, Object> dataIdChildren_4 = Backendless.Data.Of( "Country" ).Save( data );

        children = new Object[] { dataIdChildren_1 };
        Backendless.Data.Of( "CountryLanguage" ).SetRelation( dataIdParent_1, "Country:Country:1", children );

        children = new Object[] { dataIdChildren_2 };
        Backendless.Data.Of( "CountryLanguage" ).SetRelation( dataIdParent_2, "Country:Country:1", children );

        children = new Object[] { dataIdChildren_3 };
        Backendless.Data.Of( "CountryLanguage" ).SetRelation( dataIdParent_3, "Country:Country:1", children );

        children = new Object[] { dataIdChildren_4 };
        Backendless.Data.Of( "CountryLanguage" ).SetRelation( dataIdParent_4, "Country:Country:1", children );

        ////////////Сreation of the table "Capital"////////////

        data.Clear();
        data.Add( "CCapital","Kyiv" );
        Dictionary<String, Object> dIdC = Backendless.Data.Of( "Capital" ).Save( data );

        children = new Object[] { dIdC };
        Backendless.Data.Of( "Country" ).SetRelation( dataIdChildren_1, "Capital:Capital:1", children );

        data.Clear();
        data.Add( "CCapital", "London" );
        dIdC.Clear();
        dIdC = Backendless.Data.Of( "Capital" ).Save( data );

        children = new Object[] { dIdC };
        Backendless.Data.Of( "Country" ).SetRelation( dataIdChildren_2, "Capital:Capital:1", children );

        data.Clear();
        data.Add( "CCapital", "Warsaw" );
        dIdC.Clear();
        dIdC = Backendless.Data.Of( "Capital" ).Save( data );

        children = new Object[] { dIdC };
        Backendless.Data.Of( "Country" ).SetRelation( dataIdChildren_3, "Capital:Capital:1", children );

        data.Clear();
        data.Add( "CCapital", "Rome" );
        dIdC.Clear();
        dIdC = Backendless.Data.Of( "Capital" ).Save( data );

        children = new Object[] { dIdC };
        Backendless.Data.Of( "Country" ).SetRelation( dataIdChildren_4, "Capital:Capital:1", children );

      }
    }

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
      qb.SetRelated( new List<String> { "Country" } );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );

      if(res[ 0 ].ContainsKey( "Country" ) )
      {
        Object entry = res[ 0 ][ "Country" ];
        Assert.IsFalse( ( (Dictionary<Object, Object>) entry ).ContainsKey( "Capital" ) );
      }
      else
        Assert.IsFalse( true );
    }

    [TestMethod]
    public void TestGroupBy()
    {
      DataQueryBuilder qb = DataQueryBuilder.Create();
      qb.AddAllProperties();
      qb.AddGroupBy( "Percentage" );
      IList<Dictionary<String, Object>> res = Backendless.Data.Of( "CountryLanguage" ).Find( qb );
      foreach( Dictionary<String, Object> entry in res )
        Assert.IsTrue( entry.ContainsKey( "Percentage" ) );
    }
  }
}