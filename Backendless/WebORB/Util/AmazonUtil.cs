using System;
using System.Collections;
using System.Net;
using System.IO;

namespace Weborb.Util
{
  class AmazonUtil
  {
    private const String META_DATA = "meta-data";
    private const String USER_DATA = "user-data";
    private const String AMAZON_REQUEST_URL = "http://169.254.169.254";

    public static String GetMetaData( String metadataItem )
    {
      return GetData( META_DATA + "/" + metadataItem );
    }

    public static String GetUserData()
    {
      return GetData( USER_DATA );
    }

    private static String GetData( String dataItem )
    {
      HttpWebRequest request = (HttpWebRequest) WebRequest.Create( AMAZON_REQUEST_URL + "/latest/" + dataItem );
      request.Method = "GET";
      request.Timeout = 5000;
      WebResponse myResponse = null;
      StreamReader reader = null;

      try
      {
        myResponse = request.GetResponse();
        reader = new StreamReader( myResponse.GetResponseStream(), System.Text.Encoding.UTF8 );
        return reader.ReadToEnd();
      }
      catch
      {
      }
      finally
      {
        if( reader != null )
          reader.Close();

        if( myResponse != null )
          myResponse.Close();
      }

      return null;
    }
  }
}
  
