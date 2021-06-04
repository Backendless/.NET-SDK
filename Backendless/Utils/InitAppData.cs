using System;
using System.Collections.Generic;
using System.Text;

namespace BackendlessAPI
{
  public class InitAppData
  {
    public String FULL_QUERY_URL;

    internal InitAppData()
    {
      FULL_QUERY_URL = Backendless.URL + "/" + Backendless.AppId + "/" + Backendless.APIKey;
    }

    internal InitAppData( String customDomain )
    {
      FULL_QUERY_URL = customDomain + "/api";
    }
  }
}
