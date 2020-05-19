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
      private const String APP_API_KEY = "F8F82BF0-414F-36CB-FFE1-303FA538ED00";
      private const String DOTNET_API_KEY = "AB320716-358B-4BBF-AF9D-4F4B98F03363";
      private const String BKNDLSS_URL = "http://apitest.backendless.com";
#else
      public const String APP_API_KEY = "C624ABB6-8F31-8FBD-FFFA-6EABD753F900";
      private const String DOTNET_API_KEY = "868227AE-2485-4D14-A1E5-AA8F3619CEC0";
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
