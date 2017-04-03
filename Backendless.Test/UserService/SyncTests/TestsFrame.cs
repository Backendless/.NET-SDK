using System;
using System.Collections.Generic;
using Backendless.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Net;
using System.IO;

namespace BackendlessAPI.Test.UserService.SyncTests
{
  [Ignore]
  [TestClass]
  public class TestsFrame : ITest
  {
    public Random Random = new Random();
    public List<String> UsedProperties = new List<string>();

    public const string LOGIN_KEY = "email";
    public const string EMAIL_KEY = "email";
    public const string PASSWORD_KEY = "password";
    public const string ID_KEY = "id";

    public TestContext TestContext { get; set; }

    public BackendlessUser GetRandomNotRegisteredUser()
    {
      var timestamp = (DateTime.UtcNow.Ticks + Random.Next()).ToString();
      var result = new BackendlessUser();
      result.SetProperty( LOGIN_KEY, "bot" + timestamp );
      result.SetProperty( EMAIL_KEY, result.GetProperty( LOGIN_KEY ) + "@backendless.com" );
      result.Password = "somepass_" + timestamp;

      return result;
    }

    public BackendlessUser GetRandomRegisteredUser()
    {
      return Backendless.UserService.Register( GetRandomNotRegisteredUser() );
    }

    public BackendlessUser GetRandomLoggedInUser()
    {
      BackendlessUser user = GetRandomRegisteredUser();
      Backendless.UserService.Login( (string) user.GetProperty( LOGIN_KEY ), user.Password );

      return user;
    }

    public void LoginDeveloper()
    {
      String DATA = @"{""login"":""{0}"", ""password"":""{1}""}";

      HttpWebRequest request = (HttpWebRequest) WebRequest.Create( "http://" + Defaults.BACKENDLESS_HOST + ":" + Defaults.BACKENDLESS_CONSOLE_PORT +  Defaults.BACKENDLESS_CONSOLE_PATH );
      request.Method = "POST";
      request.ContentType = "application/json";
      StreamWriter requestWriter = new StreamWriter( request.GetRequestStream(), System.Text.Encoding.ASCII );
      String.Format( DATA, Defaults.DEVELOPER_LOGIN, Defaults.DEVELOPER_PASS );
      requestWriter.Write( DATA );
      requestWriter.Close();

      try
      {
        WebResponse webResponse = request.GetResponse();
        HttpWebResponse httpWebResponse = (HttpWebResponse) webResponse;

        if( httpWebResponse.StatusCode == HttpStatusCode.OK )
        {
          String authKey = httpWebResponse.Headers[ "auth-key" ];
          TestContext.Properties[ "authKey" ] = authKey;
        }
      }
      catch( System.Exception e )
      {
        Console.Out.WriteLine( "-----------------" );
        Console.Out.WriteLine( e.Message );
      }
    }

    public void DisableAppLogin()
    {
    }

    [TestInitialize]
    public void SetUp()
    {
      Backendless.URL = "http://" + Defaults.BACKENDLESS_HOST + ":9000";
      Backendless.InitApp( Defaults.TEST_APP_ID, Defaults.TEST_SECRET_KEY );
    }

    [TestCleanup]
    public void TearDown()
    {
      if( Backendless.UserService.CurrentUser != null )
        Backendless.UserService.Logout();
    }


  }
}