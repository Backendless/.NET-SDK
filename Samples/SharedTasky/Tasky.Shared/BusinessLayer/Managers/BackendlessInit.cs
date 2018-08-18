using System;
namespace Tasky.Shared.BusinessLayer.Managers
{
  public class BackendlessInit
  {
    private static readonly string APPLICATIONID = "";
    private static readonly string APIKEY = "";

    public static void Init()
    {
      if( APPLICATIONID.Length == 0 || APIKEY.Length == 0 )
        throw new Exception( "Please make sure to enter your ApplicationID and API Key in BackendlessInit.cs. The file is located in the Tasky.Shared project (BusinessLayer/Managers/BackendlessInit.cs)" );

      // initialize app
      BackendlessAPI.Backendless.InitApp( APPLICATIONID, APIKEY );
    }
  }
}
