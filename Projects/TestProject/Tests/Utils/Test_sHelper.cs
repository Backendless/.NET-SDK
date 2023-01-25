using System;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using BackendlessAPI;
using BackendlessAPI.Persistence;
using BackendlessAPI.RT.Data;
using System.Reflection;
using BackendlessAPI.Exception;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TestProject.Tests.Utils
{
  public static class Test_sHelper
  {
    internal static readonly String APP_API_KEY = File.ReadLines( path ).ToList()[ 0 ];
    //Environment.GetEnvironmentVariable( "TEST_APP_ID" );

    internal static readonly String DOTNET_API_KEY = File.ReadLines( path ).ToList()[ 1 ];
    //Environment.GetEnvironmentVariable( "TEST_DOTNET_KEY" );

    internal static HttpClient client;
    internal const String URL_BASE_ADRESS = "https://devtest.backendless.com";

    private const String path = @"/Users/default/visual_studio/authdata.txt";
    internal static readonly String Login = File.ReadLines( path ).ToList()[ 2 ];
    //Environment.GetEnvironmentVariable( "TEST_AUTH_LOGIN" );

    internal static readonly String Password = File.ReadLines( path ).ToList()[3];
    //Environment.GetEnvironmentVariable( "TEST_AUTH_PASSWORD" );

    private static String _auth_token;
    internal static String Auth_Token
    {
      get
      {
        if( _auth_token != null )
          return _auth_token;
        else
          return _auth_token = LoginAndGetToken();
      }
      private set => _auth_token = value;
    }
    public static IEventHandler<Dictionary<String, Object>> orderEventHandler;
    static Test_sHelper()
    {
      client = new HttpClient();
      client.BaseAddress = new Uri( URL_BASE_ADRESS );
      client.DefaultRequestHeaders.Add( "auth-key", Auth_Token );
    }

    internal static String LoginAndGetToken()
    {
      HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/console/home/login" );
      request.Content = new StringContent( "{\"login\":\"" + Test_sHelper.Login + "\",\"password\":\"" + Test_sHelper.Password + "\"}", Encoding.UTF8, "application/json" );

      return client.SendAsync( request ).GetAwaiter().GetResult().Headers.GetValues( "auth-key" ).ToArray()[ 0 ];
    }

    internal static void DeleteTable( String tableName )
    {
      try
      {
        Debug.WriteLine( "Start deleting table..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Delete, URL_BASE_ADRESS + $"/{APP_API_KEY}/console/data/tables/" + tableName );

        Task.WaitAll( client.SendAsync( request ) );
        Debug.WriteLine( "Table has been deleted." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( "Table has not been deleted: " + e.Message );
      }
    }

    internal static void CreateDefaultTable( String tableName )
    {
      try
      {
        Debug.WriteLine( "Start creating table..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/data/tables" );
        request.Content = new StringContent( "{\"name\":\"" + tableName + "\"}", Encoding.UTF8, "application/json" );
        Task.WaitAll( client.SendAsync( request ) );

        Debug.WriteLine( "Table has been created." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( "Table has not been created: " + e.Message );
      }
    }

    internal static void CreateJsonColumn( String tableName, String columnName )
    {
      try
      {
        Debug.WriteLine( "Start creating column..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/data/tables/{tableName}/columns" );
        request.Content = new StringContent( "{\"name\":\"" + columnName + "\", \"dataType\":\"JSON\"}", Encoding.UTF8, "application/json" );
        Task.WaitAll( client.SendAsync( request ) );

        Debug.WriteLine( "Column has been created." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( "Column has not been created" + e.Message );
      }
    }

    static void CreateColumn( String typeName, String columnName, bool TableIsCreated = true )
    {
      HttpRequestMessage requestMessage = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/" + APP_API_KEY + "/console/data/tables/GeoData/columns" );

      requestMessage.Content = new StringContent( "{\"metaInfo\":{\"srsId\":4326},\"name\":\"" + columnName + "\"," +
                        "\"dataType\":\"" + typeName + "\",\"required\":false,\"unique\":false,\"indexed\":false}", Encoding.UTF8, "application/json" );

      Task.WaitAll( client.SendAsync( requestMessage ) );
    }

    internal static void CreateRelationColumn( String parentTableName, String childTableName, String columnName, Boolean IsOneToMany = true )
    {
      try
      {
        String relationshipType = "ONE_TO_MANY";

        if( !IsOneToMany )
          relationshipType = "ONE_TO_ONE";

        Debug.WriteLine( "Start creating relationship column..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/data/tables/{parentTableName}/columns/relation" );
        request.Content = new StringContent( "{\"name\":\"" + columnName + "\", \"dataType\":\"DATA_REF\", " +
           "\"toTableName\":\"" + childTableName + "\", \"relationshipType\":\"" + relationshipType +"\"}", Encoding.UTF8, "application/json" );

        Task.WaitAll( client.SendAsync( request ) );

        Debug.WriteLine( "Relationship column has been created." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( "Relationship column has not been created" + e.Message );
      }
    }
    internal static void CreateDefaultColumn( String tableName, String columnName, String dataType )
    {
      try
      {
        dataType = dataType.ToUpper();
        Debug.WriteLine( $"Start creating {dataType} column..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/data/tables/{tableName}/columns" );
        request.Content = new StringContent( "{\"name\":\"" + columnName + "\", \"dataType\":\"" + dataType + "\"}", Encoding.UTF8, "application/json" );

        Task.WaitAll( client.SendAsync( request ) );

        Debug.WriteLine( $"{dataType} columns has been created." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( $"{dataType} column has not been created" + e.Message );
      }
    }

    public static void TestRelationSetupData()
    {
      ////////////Сreation of the parent table "Human"////////////
      Dictionary<String, Object> data = new Dictionary<String, Object>();
      data.Add( "age", 10 );
      data.Add( "name", "Nikita" );

      Dictionary<String, Object> dataIdParent_1 = Backendless.Data.Of( "Human" ).Save( data );//First object in the "Human" table
                                                                                              /////////////////////////////////////////////////////////////////////////////////////////
      data.Clear();
      data.Add( "age", 5 );
      data.Add( "name", "Tommy" );

      Dictionary<String, Object> dataIdParent_2 = Backendless.Data.Of( "Human" ).Save( data );//Second object in the "Human" table
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

      ///Сreating a connection between the objects "Human" and "Area"///

      Object[] children = new Object[] { dataIdChildren_1 };

      Backendless.Data.Of( "Human" ).SetRelation( dataIdParent_1, "Related:Area:n", children );//First relation
                                                                                               //////////////////////////////////////////////////////////////////////////////////////////

      children = new Object[] { dataIdChildren_2 };

      Backendless.Data.Of( "Human" ).SetRelation( dataIdParent_2, "Related:Area:n", children );//Second relations
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
      data.Add( "CCapital", "Kyiv" );
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

    public static void TestGeometrySetupData()
    {
      const String POINT_NAME = "POINT";
      const String LINESTRING_NAME = "LINESTRING";
      const String POLYGON_NAME = "POLYGON";
      const String GEOMETRY_NAME = "GEOMETRY";

      Dictionary<String, Object> data = new Dictionary<String, Object>();
      data.Add( "GeoDataName", "Geo data name" );
      Dictionary<String, Object> deleteObject = Backendless.Data.Of( "GeoData" ).Save( data );
      Backendless.Data.Of( "GeoData" ).Remove( deleteObject );

      CreateColumn( POINT_NAME, "P1" );

      data.Add( "P1", new Point().SetX( 40.41 ).SetY( -3.706 ) );

      CreateColumn( POINT_NAME, "pickupLocation" );

      CreateColumn( LINESTRING_NAME, "LineValue" );

      List<Point> list = new List<Point>();
      list.Add( new Point().SetX( 30.1 ).SetY( 10.05 ) );
      list.Add( new Point().SetX( 30.2 ).SetY( 10.04 ) );

      LineString finalLine = new LineString( list );
      data.Add( "LineValue", finalLine );

      CreateColumn( POLYGON_NAME, "PolyValue" );

      List<Point> tempList = new List<Point>();

      tempList.Add( new Point().SetX( -77.05786152 ).SetY( 38.87261877 ) );
      tempList.Add( new Point().SetX( -77.0546978 ).SetY( 38.87296123 ) );
      tempList.Add( new Point().SetX( -77.05317431 ).SetY( 38.87061405 ) );
      tempList.Add( new Point().SetX( -77.0555883 ).SetY( 38.86882611 ) );
      tempList.Add( new Point().SetX( -77.05847435 ).SetY( 38.87002898 ) );
      tempList.Add( new Point().SetX( -77.05786152 ).SetY( 38.87261877 ) );

      List<Point> tempList2 = new List<Point>();

      tempList2.Add( new Point().SetX( -77.05579215 ).SetY( 38.87026286 ) );
      tempList2.Add( new Point().SetX( -77.05491238 ).SetY( 38.87087264 ) );
      tempList2.Add( new Point().SetX( -77.05544882 ).SetY( 38.87170794 ) );
      tempList2.Add( new Point().SetX( -77.05669337 ).SetY( 38.87156594 ) );
      tempList2.Add( new Point().SetX( -77.05684357 ).SetY( 38.87072228 ) );
      tempList2.Add( new Point().SetX( -77.05579215 ).SetY( 38.87026286 ) );

      LineString tempLines = new LineString( tempList2 );
      List<LineString> lines = new List<LineString>();
      lines.Add( tempLines );

      Polygon poly = new Polygon( tempList, lines );
      data.Add( "PolyValue", poly );

      CreateColumn( GEOMETRY_NAME, "GeoValue" );

      data.Add( "GeoValue", new Point().SetX( 10.2 ).SetY( 48.5 ) );

      Backendless.Data.Of( "GeoData" ).Save( data );
    }

    internal static Dictionary<String, Object> ConvertInstanceToMap<E>( E instance )
    {
      var convertedDictionary = HandleConversionToDictionary( instance );

      if( convertedDictionary.ContainsKey( "ObjectId" ) )
        ChangeKey( convertedDictionary, "ObjectId", "objectId" );

      return ( from kv in convertedDictionary where kv.Value != null select kv ).ToDictionary( prop => prop.Key, prop => prop.Value );   
    }

    private static Dictionary<String, Object> HandleConversionToDictionary<E>( E instance )
    {
      if( instance == null )
        throw new ArgumentException( ExceptionMessage.NULL_INSTANCE );

      return instance.GetType()
                     .GetProperties( BindingFlags.Instance | BindingFlags.Public )
                     .ToDictionary( prop => prop.Name, prop => prop.GetValue( instance, null ) );
    }

    internal static void ChangeKey<TKey, TValue>( this IDictionary<TKey, TValue> dictionary,
                                      TKey fromKey, TKey toKey )
    {
      TValue value = dictionary[ fromKey ];
      dictionary.Remove( fromKey );
      dictionary[ toKey ] = value;
    }

    internal static String CreateRole( String roleName )
    {
      Debug.WriteLine( "Start creating new role..." );

      HttpRequestMessage requestMessage = new HttpRequestMessage( HttpMethod.Put, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/security/roles/{roleName}" );
      JObject tempJson = JObject.Parse( client.SendAsync( requestMessage ).Result.Content.ReadAsStringAsync().Result );
      Dictionary<String, String> dictionary = JObject.FromObject( tempJson ).ToObject<Dictionary<String, String>>();
      return dictionary[ "roleId" ];
    }

    internal static void AssignRole( String roleId, String roleName, String userId )
    {
      Debug.WriteLine( "Start assigning role to user..." );

      HttpRequestMessage requestMessage = new HttpRequestMessage( HttpMethod.Put, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/security/assignedroles" );

      requestMessage.Content = new StringContent( "{\"roles\": [{\"roleId\": \""+ roleId +"\", \"roleName\": \"" + roleName + "\", \"status\": \"ALL\"}], \"users\": [\"" + userId +"\"]}", Encoding.UTF8, "application/json" );

      Task.WaitAll( client.SendAsync( requestMessage ) );
    }

    internal static void DeleteRole( String roleId )
    {
      Debug.WriteLine( "Start deleting role..." );

      HttpRequestMessage requestMessage = new HttpRequestMessage( HttpMethod.Delete, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/security/roles/{roleId}" );

      Task.WaitAll( client.SendAsync( requestMessage ) );
    }
  }
}