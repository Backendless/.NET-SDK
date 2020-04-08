#define DEV_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendlessAPI;

namespace GeometryTestProject
{
  [TestClass]
  class TestInitialization
  {
#if DEV_TEST
      private const String APP_API_KEY = "F8F82BF0-414F-36CB-FFE1-303FA538ED00";
      private const String DOTNET_API_KEY = "AB320716-358B-4BBF-AF9D-4F4B98F03363";
      private const String BKNDLSS_URL = "http://apitest.backendless.com";
#else
      private const String APP_API_KEY = "B5D20616-5565-2674-FF73-C5CAC72BD200";
      private const String DOTNET_API_KEY = "18BF3443-B8A8-48E1-90ED-2783F9AF2D40";
      private const String BKNDLSS_URL = "http://api.backendless.com";
#endif

    [AssemblyInitialize]
    public static void AssemblyInit( TestContext context )
    {
      Backendless.URL = BKNDLSS_URL;
      Backendless.InitApp( APP_API_KEY, DOTNET_API_KEY );
    }
  }
}
