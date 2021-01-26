using System;
using TestProject.Tests.Utils;

namespace TestProject.Tests.Geolocation
{
  public class GeometryTestsInitializator : IDisposable
  {
    public GeometryTestsInitializator()
    {
      Test_sHelper.TestGeometrySetupData();
    }

    public void Dispose()
    {
      Test_sHelper.DeleteTable( "GeoData" );
    }
  }
}