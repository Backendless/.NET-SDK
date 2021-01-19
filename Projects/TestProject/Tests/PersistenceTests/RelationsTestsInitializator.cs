using System;
namespace TestProject
{
  public class RelationsTestsInitializator : IDisposable
  {
    public RelationsTestsInitializator()
    {
      Test_sHelper.TestRelationSetupData();
    }

    public void Dispose()
    {
      Test_sHelper.DeleteTable( "Human" );
      Test_sHelper.DeleteTable( "Area" );
      Test_sHelper.DeleteTable( "CountryLanguage" );
      Test_sHelper.DeleteTable( "Country" );
      Test_sHelper.DeleteTable( "Capital" );
    }
  }
}