//#define DEV_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;
using System.Collections.Generic;
using System.Linq;

namespace GeometryTestProject
{
  [TestClass]
  class TestInitialization
  {
#if DEV_TEST
      private const String APP_API_KEY = "F8F82BF0-414F-36CB-FFE1-303FA538ED00";
      private const String DOTNET_API_KEY = "AB320716-358B-4BBF-AF9D-4F4B98F03363";
      private const String BKNDLSS_URL = "http://apitest.backendless.com";
#else
      private const String APP_API_KEY = "B5D20616-5565-2674-FF73-C5CAC72BD200";
      private const String DOTNET_API_KEY = "18BF3443-B8A8-48E1-90ED-2783F9AF2D40";
      private const String BKNDLSS_URL = "http://api.backendless.com";
#endif

    [AssemblyInitialize]
    public static void AssemblyInit( TestContext context )
    {
      Backendless.URL = BKNDLSS_URL;
      Backendless.InitApp( APP_API_KEY, DOTNET_API_KEY );
    }

    [AssemblyInitialize]
    public static void SetupDatabaseData( TestContext context )
    {
      ////////////Сreation of the parent table "Order"////////////

      Dictionary<String, Object> data = new Dictionary<String, Object>();
      data.Add( "age", 10 );
      data.Add( "name", "Nikita" );

      Dictionary<String, Object> dataIdParent_1 = Backendless.Data.Of( "Order" ).Save( data );//////////First object in the "Order" table
      /////////////////////////////////////////////
      data.Clear();
      data.Add( "age", 5 );
      data.Add( "name", "Tommy" );

      Dictionary<String, Object> dataIdParent_2 = Backendless.Data.Of( "Order" ).Save( data );//////////Second object in the "Order" table
                                                  /////////////////////////////////////////////

      ////////////Creation of the children table "Area"////////////

      data.Clear();
      data.Add( "AreaA", "Munich" );
      data.Add( "Categories", false );
      data.Add( "UserId", 3 );

      Dictionary<String, Object> dataIdChildren_1 = Backendless.Data.Of( "Area" ).Save( data );//////////First object in the "Area" table
      ////////////////////////////////////////////
      data.Clear();
      data.Add( "AreaA", "London" );
      data.Add( "Categories", true );
      data.Add( "UserId", 6 );

      Dictionary<String, Object> dataIdChildren_2 = Backendless.Data.Of( "Area" ).Save( data );//////////Second object in the "Area" table
                                                                                               ////////////////////////////////////////////

      ///Сreating a connection between the objects "Order" and "Area"///

      Object[] children = new Object[] { dataIdChildren_1 };

      Backendless.Data.Of( "Order" ).SetRelation( dataIdParent_1, "Related:Area:n", children );//First relation
      //////////////////////////////////////////////////////////////////////////////////////////
      
      children = new Object[] { dataIdChildren_2 };

      Backendless.Data.Of( "Order" ).SetRelation( dataIdParent_2, "Related:Area:n", children );//Second relations
      //////////////////////////////////////////////////////////////////////////////////////////
    }
  }
}
