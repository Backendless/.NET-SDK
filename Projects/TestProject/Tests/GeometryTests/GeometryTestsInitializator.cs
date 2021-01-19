using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
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