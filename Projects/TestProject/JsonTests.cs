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
    private Dictionary<String, Object> CreateDefaultJson()
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

    [TestMethod]
    public void JN1()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).FindFirst()[ "objectId" ];
      map[ "json" ] = "{}";

      Dictionary<String, Object> newMap = Backendless.Data.Of( "Table1" ).Save( map );

      Assert.IsTrue( ((Dictionary<Object, Object>) newMap[ "json" ])[ "rawJsonString" ].ToString() == map[ "json" ].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( map );
    }

    [TestMethod]
    public void JN2()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.decimals[0]' < 13" );

      var newMap = Backendless.Data.Of("Table1").Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 7 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN3()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.timeMarks.date' = '2015-07-29'" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 7 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN4()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.time' as time" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "time" ) );
      Assert.IsTrue( newMap[ "time" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN5()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.*' as allTimeMarks" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "allTimeMarks" ) );
      Assert.IsTrue( newMap[ "allTimeMarks" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN6()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.*[1]' as allSecondValuesFromArray" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "allSecondValuesFromArray" ) );
      Assert.IsTrue( newMap[ "allSecondValuesFromArray" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN7()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.letter' as jsonLetter" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "jsonLetter" ) );
      Assert.IsTrue( newMap[ "jsonLetter" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }
    
    [TestMethod]
    public void JN8()
    {
      Dictionary<String, Object> map = CreateDefaultJson();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.status' as jsonStatus" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.IsTrue( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.IsTrue( newMap.Keys.Count == 3 );
      Assert.IsTrue( newMap.Keys.Contains( "objectId" ) );
      Assert.IsTrue( newMap.Keys.Contains( "___class" ) );
      Assert.IsTrue( newMap.Keys.Contains( "jsonStatus" ) );
      Assert.IsTrue( newMap[ "jsonStatus" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( newMap );
    }

    [TestMethod]
    public void JN9()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      json[ "jsonValue" ] = "{}";
      json["objectId"] = Backendless.Data.Of( "Table1" ).Save( json )["objectId"];

      json["jsonValue"] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument( "$.innerObject", new Dictionary<String, Object> { { "a", "b" } } )
                                           .AddArgument( "$.innerArray", new Object[] { 1, 2, 3 } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 7 );
      Assert.IsTrue( newMap[ "jsonValue" ] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN10()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      json[ "jsonValue" ] = "{}";
      json[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( json )[ "objectId" ];

      json["jsonValue"] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument("$.colours[0]", null)
                                           .AddArgument( "$.innerValue", new Dictionary<String, Object> { { "value", "value" } } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 7 );
      Assert.IsTrue( ((Dictionary<Object, Object>) newMap[ "jsonValue" ])["rawJsonString"] != null );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN11()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      json[ "jsonValue" ] = "{}";
      json[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( json )[ "objectId" ];

      json[ "jsonValue" ] = JSONUpdateBuilder.INSERT()
                                           .AddArgument( "$.state", "on" )
                                           .AddArgument( "$.number", 11 )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap.Keys.Count == 7 );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ] != null );

      Object expectedJson = "{\"state\": \"on\", \"number\": 11}";

      Assert.IsTrue( expectedJson.ToString() == ((Dictionary<Object, Object>) newMap["jsonValue"])["rawJsonString"].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN12()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "letter", "a"},
        { "number", 10 }
      };
      json[ "jsonValue" ] = JsonConvert.SerializeObject( serializeMap );
      

      serializeMap["objectId"] = Backendless.Data.Of( "Table1" ).Save( json )["objectId"];
      json.Clear();
      json[ "objectId" ] = serializeMap[ "objectId" ];
      json[ "jsonValue" ] = JSONUpdateBuilder.REPLACE()
                                           .AddArgument("$.number", 11)
                                           .AddArgument("$.colours","red")
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ] != null );

      Object expectedJson = "{\"letter\": \"a\", \"number\": 11}";
      Assert.IsTrue( expectedJson.ToString() == ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN13()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "decimals", new Double[] { 12.5, 14 } },
        { "colours", new String[] { "green", "blue"} }
      };

      json[ "jsonValue" ] = JsonConvert.SerializeObject( serializeMap );

      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( json )[ "objectId" ];
      json.Clear();
      json[ "objectId" ] = serializeMap[ "objectId" ];
      json[ "jsonValue" ] = JSONUpdateBuilder.REMOVE()
                                           .AddArgument( "$.decimals[0]" )
                                           .AddArgument( "$.colours" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ] != null );

      Object expectedJson = "{\"decimals\": [14]}";
      Assert.IsTrue( expectedJson.ToString() == ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN14()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "decimals", new Double[] { 12.5, 14 } },
        { "colours", new String[] { "green", "blue"} }
      };

      json[ "jsonValue" ] = JsonConvert.SerializeObject( serializeMap );

      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( json )[ "objectId" ];
      json.Clear();
      json[ "objectId" ] = serializeMap[ "objectId" ];
      json[ "jsonValue" ] = JSONUpdateBuilder.ARRAY_APPEND()
                                           .AddArgument( "$.decimals", 432 )
                                           .AddArgument( "$.colours", "Yellow" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ] != null );

      Object expectedJson = "{\"colours\": [\"green\", \"blue\", \"Yellow\"], \"decimals\": [12.5, 14, 432]}";
      Assert.IsTrue( expectedJson.ToString() == ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

    [TestMethod]
    public void JN15()
    {
      Dictionary<String, Object> json = new Dictionary<String, Object>();
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "decimals", new Double[] { 12.5, 14 } },
        { "colours", new String[] { "green", "blue"} }
      };

      json[ "jsonValue" ] = JsonConvert.SerializeObject( serializeMap );

      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( json )[ "objectId" ];
      json.Clear();
      json[ "objectId" ] = serializeMap[ "objectId" ];
      json[ "jsonValue" ] = JSONUpdateBuilder.ARRAY_INSERT()
                                           .AddArgument( "$.decimals[0]", 432 )
                                           .AddArgument( "$.colours[1]", "Yellow" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( json );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( newMap[ "objectId" ].ToString() == json[ "objectId" ].ToString() );
      Assert.IsTrue( ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ] != null );

      Object expectedJson = "{\"colours\": [\"green\", \"Yellow\", \"blue\"], \"decimals\": [432, 12.5, 14]}";
      Assert.IsTrue( expectedJson.ToString() == ( (Dictionary<Object, Object>) newMap[ "jsonValue" ] )[ "rawJsonString" ].ToString() );

      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }
  }
}
