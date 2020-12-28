using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using BackendlessAPI;
using BackendlessAPI.Utils;
using Newtonsoft.Json;
using BackendlessAPI.Persistence;
using System.Linq;
using System.Collections;
using System.Data;

namespace TestProject
{
  [TestClass]
  public class JsonTests
  {
    private Dictionary<String, Object> GenerateDefaultJsonData()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Dictionary<String, Object> json = new Dictionary<String, Object>
      {
        { "letter", "a"},
        { "number", 10 },
        { "decimals", new Double[]{ 12.3, 43.28, 56.89 } },
        { "colors", new String[]{ "red", "green", "blue" } },
        { "status", true },
        { "description", "Is is an \"Example\"." },
        { "timeMarks", new Dictionary<String, Object>
                                 {
                                    { "time", "12:18:29.000000" },
                                    { "date", "2015-07-29" },
                                    { "date_time", "2015-07-29 12:18:29.000000" }
                                 }
        }
      };

      map[ "json" ] = JsonConvert.SerializeObject( json );
      Backendless.UserService.Login("hdhdhd@gmail.com", "123234");
      return Backendless.Data.Of( "Table1" ).Save( map );
    }

    [ClassInitialize]
    public static void InitializeTable( TestContext context )
    {
      TestInitialization.CreateDefaultTable( "Table1" );
    }

    [TestInitialize]
    public void InitializeJsonColumn()
    {
      TestInitialization.CreateJsonColumn( "Table1", "json" );
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
      TestInitialization.DeleteTable( "Table1" );
    }

    [TestMethod]
    public void JN1()
    {
      Dictionary<String, Object> map = GenerateDefaultJsonData();

      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).FindFirst()[ "objectId" ];
      map[ "json" ] = "{}";
      Dictionary<String, Object> newMap = Backendless.Data.Of( "Table1" ).Save( map );
      map[ "json" ] = new Dictionary<Object, Object>();

      Assert.IsTrue( ((Dictionary<Object, Object>) newMap[ "json" ]).Count == ((Dictionary<Object, Object>) map[ "json" ]).Count );

      Backendless.Data.Of( "Table1" ).Remove( map );
    }

    [TestMethod]
    public void JN2()
    {
      Dictionary<String, Object> map = GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.decimals[0]' < 13" );

      map = Backendless.Data.Of("Table1").Find( queryBuilder )[ 0 ];

      Assert.IsTrue( map["json"] != null && map["json"] != new Dictionary<Object, Object>() );
      Assert.IsTrue( ((Dictionary<Object, Object>) map["json"]).Count == 7 );
      Assert.IsTrue( map.Keys.Contains( "objectId" ) );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN3()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.timeMarks.date' = '2015-07-29'" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( ((Dictionary<Object, Object>) newMap["json"]).Count == 7 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN4()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.time' as time" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "time" ) );
      Assert.IsTrue( newMap[ "time" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN5()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.*' as allTimeMarks" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "allTimeMarks" ) );
      Assert.IsTrue( newMap[ "allTimeMarks" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN6()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.*[1]' as allSecondValuesFromArray" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "allSecondValuesFromArray" ) );
      Assert.IsTrue( newMap[ "allSecondValuesFromArray" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN7()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.letter' as jsonLetter" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "jsonLetter" ) );
      Assert.IsTrue( newMap[ "jsonLetter" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }
    
    [TestMethod]
    public void JN8()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.status' as jsonStatus" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "jsonStatus" ) );
      Assert.IsTrue( newMap[ "jsonStatus" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN9()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      map[ "json" ] = "{}";
      map["json"] = Backendless.Data.Of( "Table1" ).Save( map )["objectId"];
      
      map["json"] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument( "$.innerObject", new Dictionary<String, Object> { { "a", "b" } } )
                                           .AddArgument( "$.innerArray", new Object[] { 1, 2, 3 } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 6 );
      Assert.IsTrue( newMap[ "json" ] != null );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "json" ] ).Count == 7 );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN10()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      map[ "json" ] = "{}";
      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];

      map["json"] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument("$.colours[0]", null)
                                           .AddArgument( "$.innerValue", new Dictionary<String, Object> { { "value", "value" } } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 6 );
      Assert.IsTrue( ((Dictionary<Object, Object>) newMap[ "json" ]) != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN11()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      map[ "json" ] = "{}";
      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];

      map[ "json" ] = JSONUpdateBuilder.INSERT()
                                           .AddArgument( "$.state", "on" )
                                           .AddArgument( "$.number", 11 )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 6 );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN12()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "letter", "a"},
        { "number", 10 }
      };
      map[ "json" ] = JsonConvert.SerializeObject( serializeMap );
      

      serializeMap["objectId"] = Backendless.Data.Of( "Table1" ).Save( map )["objectId"];
      map.Clear();
      map[ "objectId" ] = serializeMap[ "objectId" ];
      map[ "json" ] = JSONUpdateBuilder.REPLACE()
                                           .AddArgument("$.number", 11)
                                           .AddArgument("$.colours","red")
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN13()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "decimals", new Double[] { 12.5, 14 } },
        { "colours", new String[] { "green", "blue"} }
      };

      map[ "json" ] = JsonConvert.SerializeObject( serializeMap );

      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];
      map.Clear();
      map[ "objectId" ] = serializeMap[ "objectId" ];
      map[ "json" ] = JSONUpdateBuilder.REMOVE()
                                           .AddArgument( "$.decimals[0]" )
                                           .AddArgument( "$.colours" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN14()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "decimals", new Double[] { 12.5, 14 } },
        { "colours", new String[] { "green", "blue"} }
      };

      map[ "json" ] = JsonConvert.SerializeObject( serializeMap );

      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];
      map.Clear();
      map[ "objectId" ] = serializeMap[ "objectId" ];
      map[ "json" ] = JSONUpdateBuilder.ARRAY_APPEND()
                                           .AddArgument( "$.decimals", 432 )
                                           .AddArgument( "$.colours", "Yellow" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }
  }
}
