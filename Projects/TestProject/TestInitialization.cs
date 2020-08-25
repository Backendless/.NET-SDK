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
#endif
#if MARKENV
      public const String APP_API_KEY = "F632EF89-D87C-8E34-FF81-0E72A4F14600";
      private const String DOTNET_API_KEY = "A0C8F471-4974-6328-FFC9-B889D13DF100";
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
