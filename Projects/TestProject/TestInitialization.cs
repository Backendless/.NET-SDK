using System;
using System.Threading.Tasks;
using BackendlessAPI;

namespace TestProject
{
  public class TestInitialization : IDisposable
  {
    private const String BKNDLSS_URL = "http://apitest.backendless.com";

    public TestInitialization()
    {
      Backendless.URL = BKNDLSS_URL;
      Backendless.InitApp( Test_sHelper.APP_API_KEY, Test_sHelper.DOTNET_API_KEY );
      Backendless.UserService.Login( "hdhdhd@gmail.com", "123234" );
      Test_sHelper.CreateDefaultTable( "Person" );
      Test_sHelper.CreateDefaultTable( "Order" );
      Test_sHelper.CreateDefaultTable( "Table1" );
      Test_sHelper.CreateDefaultColumn( "Person", "name", "string" );
      Test_sHelper.CreateDefaultColumn( "Person", "age", "int" );
      Test_sHelper.CreateRelationColumnOneToMany( "Person", "Order", "Surname" );
    }

    public void Dispose()
    {
      Test_sHelper.DeleteTable( "Person" );
      Test_sHelper.DeleteTable( "Location" );
      Test_sHelper.DeleteTable( "Order" );
      Test_sHelper.DeleteTable( "Table1" );
    }
  }
}