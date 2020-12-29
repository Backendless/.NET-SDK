#define DEV_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using Weborb.Util.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestProject
{
  [TestClass]
  public class TestInitialization
  {
#if DEV_TEST
    public const String APP_API_KEY = "";
    private const String DOTNET_API_KEY = "";
    private const String BKNDLSS_URL = "http://apitest.backendless.com";

    internal static HttpClient client;
    internal const String URL_BASE_ADRESS = "https://devtest.backendless.com";

    private const String path = @"f:\specialproject\authdata.txt";
    internal static readonly String Login = File.ReadLines( path ).First(); //login in this file must be the first line
    internal static readonly String Password = File.ReadLines( path ).Last();//password - as second line

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

    static TestInitialization()
    {
      client = new HttpClient();
      client.BaseAddress = new Uri( URL_BASE_ADRESS );
      client.DefaultRequestHeaders.Add( "auth-key", Auth_Token );
    }

    internal static String LoginAndGetToken()
    {
      HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/console/home/login" );
      request.Content = new StringContent( "{\"login\":\"" + TestInitialization.Login + "\",\"password\":\"" + TestInitialization.Password + "\"}", Encoding.UTF8, "application/json" );

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

    internal static void CreateRelationColumnOneToMany( String parentTableName, String childTableName, String columnName )
    {
      try
      {
        Debug.WriteLine( "Start creating relationship column..." );

        HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post, $"{URL_BASE_ADRESS}/{APP_API_KEY}/console/data/tables/{parentTableName}/columns/relation" );
        request.Content = new StringContent( "{\"name\":\"" + columnName + "\", \"dataType\":\"DATA_REF\", " +
           "\"toTableName\":\"" + childTableName + "\", \"relationshipType\":\"ONE_TO_MANY\"}", Encoding.UTF8, "application/json" );

        Task.WaitAll( client.SendAsync( request ) );

        Debug.WriteLine( "Relationship column has been created." );
      }
      catch( Exception e )
      {
        Debug.WriteLine( "Relationship column has not been created" + e.Message );
      }
    }

#elif MARKENV
    public const String APP_API_KEY = "";
      private const String DOTNET_API_KEY = "";
      private const String BKNDLSS_URL = "http://api.backendless.com";
#else
      public const String APP_API_KEY = "";
      private const String DOTNET_API_KEY = "";
      private const String BKNDLSS_URL = "http://api.backendless.com";
#endif

    [AssemblyInitialize]
    public static void AssemblyInit_SetupDatabaseData( TestContext context )
    {
      Backendless.URL = BKNDLSS_URL;
      Backendless.InitApp( APP_API_KEY, DOTNET_API_KEY );
    }
  }
}
