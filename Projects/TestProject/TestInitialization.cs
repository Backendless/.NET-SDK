#define DEV_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
  [TestClass]
  public class TestInitialization
  {
#if DEV_TEST
      public const String APP_API_KEY = "";
      private const String DOTNET_API_KEY = "";
      private const String BKNDLSS_URL = "http://apitest.backendless.com";
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
