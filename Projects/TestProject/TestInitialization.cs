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
      public const String APP_API_KEY = "F8F82BF0-414F-36CB-FFE1-303FA538ED00";
      private const String DOTNET_API_KEY = "AB320716-358B-4BBF-AF9D-4F4B98F03363";
      private const String BKNDLSS_URL = "http://apitest.backendless.com";
#else
      public const String APP_API_KEY = "CF7398E8-7F3D-64F9-FF3B-FB3CE8893200";
      private const String DOTNET_API_KEY = "9808682C-32C2-EC87-FFF5-CAB709BE2400";
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
