using System;
using Weborb.Util.Logging;

namespace BackendlessAPI.RT
{
  public class RTClientFactory
  {
    private static readonly IRTClient rtClient;

    static RTClientFactory()
    {
      IRTClient rt;

      try
      {
        rt = new AsyncRTClient();
      }
      catch( System.Exception e )
      {
        Log.log( Backendless.BACKENDLESSLOG, e );
        rt = new RTClientWithoutSocketIO();
      }

      rtClient = rt;
    }

    public static IRTClient Get()
    {
      return rtClient;
    }
  }
}
