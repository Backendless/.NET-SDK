//#define DEV_TEST
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
      internal const String APP_API_KEY = "CF7398E8-7F3D-64F9-FF3B-FB3CE8893200";
      internal const String DOTNET_API_KEY = "9808682C-32C2-EC87-FFF5-CAB709BE2400";
      internal const String BKNDLSS_URL = "http://apitest.backendless.com";
#else
      internal const String APP_API_KEY = "C624ABB6-8F31-8FBD-FFFA-6EABD753F900";
      internal const String DOTNET_API_KEY = "868227AE-2485-4D14-A1E5-AA8F3619CEC0";
      internal const String BKNDLSS_URL = "http://api.backendless.com";
#endif

    [AssemblyInitialize]
    public static void AssemblyInit_SetupDatabaseData( TestContext context )
    {
      Backendless.URL = BKNDLSS_URL;
      Backendless.InitApp( APP_API_KEY, DOTNET_API_KEY );
    }
  }
}
