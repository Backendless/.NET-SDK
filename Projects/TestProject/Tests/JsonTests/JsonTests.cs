using Xunit;
using System;
using BackendlessAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using BackendlessAPI.Persistence;

namespace TestProject
{
  [Collection( "Tests" )]
  public class JsonTests : IDisposable
  {
    public JsonTests()
    {
      Test_sHelper.CreateDefaultColumn( "Table1", "json", "json" );
    }

    public void Dispose()
    {
      Backendless.Data.Of( "Table1" ).Remove( "'json' != null" );
    }

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
        { "description", " is an \"Example\"." },
        { "timeMarks", new Dictionary<String, Object>
                                 {
                                    { "time", "12:18:29.000000" },
                                    { "date", "2015-07-29" },
                                    { "date_time", "2015-07-29 12:18:29.000000" }
                                 }
        }
      };

      map[ "json" ] = JsonConvert.SerializeObject( json );
      return Backendless.Data.Of( "Table1" ).Save( map );
    }


    [Fact]
    public void JN1()
    {
      Dictionary<String, Object> map = GenerateDefaultJsonData();

      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).FindFirst()[ "objectId" ];
      map[ "json" ] = "{}";
      Dictionary<String, Object> newMap = Backendless.Data.Of( "Table1" ).Save( map );
      map[ "json" ] = new Dictionary<Object, Object>();

      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ).Count == ( (Dictionary<Object, Object>) map[ "json" ] ).Count );
    }

    [Fact]
    public void JN2()
    {
      GenerateDefaultJsonData();
      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.decimals[0]' < 13" );

      Dictionary<String, Object> map = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.True( map[ "json" ] != null && map[ "json" ] != new Dictionary<Object, Object>() );
      Assert.True( ( (Dictionary<Object, Object>) map[ "json" ] ).Count == 7 );
      Assert.Contains( "objectId", map.Keys );
    }

    [Fact]
    public void JN3()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.SetWhereClause( "json->'$.timeMarks.date' = '2015-07-29'" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ).Count == 7 );
      Assert.Contains( "objectId", newMap.Keys );
    }

    [Fact]
    public void JN4()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.time' as time" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];
      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( newMap.Keys.Count == 3 );
      Assert.Contains( "objectId", newMap.Keys );
      Assert.Contains( "___class", newMap.Keys );
      Assert.Contains( "time", newMap.Keys );
      Assert.True( newMap[ "time" ] != null );
    }

    [Fact]
    public void JN5()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.timeMarks.*' as allTimeMarks" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( newMap.Keys.Count == 3 );
      Assert.Contains( "objectId", newMap.Keys );
      Assert.Contains( "___class", newMap.Keys );
      Assert.Contains( "allTimeMarks", newMap.Keys );
      Assert.True( newMap[ "allTimeMarks" ] != null );
    }

    [Fact]
    public void JN6()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.*[1]' as allSecondValuesFromArray" );
      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( newMap.Keys.Count == 3 );
      Assert.Contains( "objectId", newMap.Keys );
      Assert.Contains( "___class", newMap.Keys );
      Assert.Contains( "allSecondValuesFromArray", newMap.Keys );
      Assert.True( newMap[ "allSecondValuesFromArray" ] != null );
    }

    [Fact]
    public void JN7()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.letter' as jsonLetter" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( newMap.Keys.Count == 3 );
      Assert.Contains( "objectId", newMap.Keys );
      Assert.Contains( "___class", newMap.Keys );
      Assert.Contains( "jsonLetter", newMap.Keys );
      Assert.True( newMap[ "jsonLetter" ] != null );
    }

    [Fact]
    public void JN8()
    {
      GenerateDefaultJsonData();

      DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
      queryBuilder.AddProperties( "json->'$.status' as jsonStatus" );

      var newMap = Backendless.Data.Of( "Table1" ).Find( queryBuilder )[ 0 ];

      Assert.True( newMap != null && newMap != new Dictionary<String, Object>() );
      Assert.True( newMap.Keys.Count == 3 );
      Assert.Contains( "objectId", newMap.Keys );
      Assert.Contains( "___class", newMap.Keys );
      Assert.Contains( "jsonStatus", newMap.Keys );
      Assert.True( newMap[ "jsonStatus" ] != null );
    }

    [Fact]
    public void JN9()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      map[ "json" ] = "{}";
      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];

      map[ "json" ] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument( "$.innerObject", new Dictionary<String, Object> { { "a", "b" } } )
                                           .AddArgument( "$.innerArray", new Object[] { 1, 2, 3 } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap.Keys.Count == 6 );
      Assert.True( newMap[ "json" ] != null );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ).Count == 5 );
    }

    [Fact]
    public void JN10()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      map[ "json" ] = "{}";
      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];

      map[ "json" ] = JSONUpdateBuilder.SET()
                                           .AddArgument( "$.letter", "b" )
                                           .AddArgument( "$.number", 36 )
                                           .AddArgument( "$.state", true )
                                           .AddArgument( "$.colours[0]", null )
                                           .AddArgument( "$.innerValue", new Dictionary<String, Object> { { "value", "value" } } )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap.Keys.Count == 6 );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );
    }

    [Fact]
    public void JN11()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      map[ "json" ] = "{}";
      map[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];

      map[ "json" ] = JSONUpdateBuilder.INSERT()
                                           .AddArgument( "$.state", "on" )
                                           .AddArgument( "$.number", 11 )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap.Keys.Count == 6 );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );
    }

    [Fact]
    public void JN12()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
      Dictionary<String, Object> serializeMap = new Dictionary<String, Object>
      {
        { "letter", "a"},
        { "number", 10 }
      };
      map[ "json" ] = JsonConvert.SerializeObject( serializeMap );


      serializeMap[ "objectId" ] = Backendless.Data.Of( "Table1" ).Save( map )[ "objectId" ];
      map.Clear();
      map[ "objectId" ] = serializeMap[ "objectId" ];
      map[ "json" ] = JSONUpdateBuilder.REPLACE()
                                           .AddArgument( "$.number", 11 )
                                           .AddArgument( "$.colours", "red" )
                                           .Create();

      var newMap = Backendless.Data.Of( "Table1" ).Save( map );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );
    }

    [Fact]
    public void JN13()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
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
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );
    }

    [Fact]
    public void JN14()
    {
      Dictionary<String, Object> map = new Dictionary<String, Object>();
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
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( newMap[ "objectId" ].ToString() == map[ "objectId" ].ToString() );
      Assert.True( ( (Dictionary<Object, Object>) newMap[ "json" ] ) != null );
    }
  }
}