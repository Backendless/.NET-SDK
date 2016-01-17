using System;
using System.Collections;
using System.Text.RegularExpressions;

using Weborb.Cloud;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Config.Configurators
{
  class AmazonConfigurator : LocalConfigurator
  {
    private const String USER_DATA_KEYVALUE_SEPARATOR = ":";
    public const String SUBSCRIBER_ID = "weborb.subscriber.id";
    public const String EMAIL_ADDRESS = "weborb.emailaddress";

    protected override IDictionary ConfigureHandler( ORBConfig config, ArrayList sectionsToConfig )
    {
      IDictionary configObject = base.ConfigureHandler( config, sectionsToConfig );
      InitializeCloudBilling();

      return configObject;
    }

    public void InitializeCloudBilling()
    {
      String emailAddress = GetData( EMAIL_ADDRESS );
      String subscriberIDStr = GetData( SUBSCRIBER_ID );
      int subscriberID = -1;

      if( subscriberIDStr != null )
        subscriberID = int.Parse( subscriberIDStr );

      if( emailAddress == null && subscriberIDStr == null )
      {
        emailAddress = CloudConfig.EmailAddress;
        subscriberID = CloudConfig.SubscriberID;
      }

      if( Log.isLogging( LoggingConstants.INFO ) )
        Log.log( LoggingConstants.INFO, "Initializing cloud billing with: subscriber ID - " + subscriberID + ", email address - " + emailAddress );

      CloudBillingClient.GetInstance().Initialize( subscriberID, emailAddress );
    }

    internal static String GetData( String name )
    {
      try
      {
        String userDataStr = AmazonUtil.GetUserData();

        if (Log.isLogging(LoggingConstants.INFO))
          Log.log(LoggingConstants.INFO, "EC2 instance user data: " + userDataStr);

        String[] userData = Regex.Split(userDataStr, "\\s+\\|\\s+");

        for (int i = 0; i < userData.Length; i++)
        {
          String data = userData[i].Trim();

          if (Log.isLogging(LoggingConstants.INFO))
            Log.log(LoggingConstants.INFO, "EC2 instance user data element " + data);

          if (data.StartsWith(name))
          {
            if (Log.isLogging(LoggingConstants.INFO))
              Log.log(LoggingConstants.INFO, "EC2 instance user data element matched " + name);

            return data.Substring(data.IndexOf(USER_DATA_KEYVALUE_SEPARATOR) + 1).Trim();
          }
        }
      }
      catch (Exception e)
      {
        if (Log.isLogging(LoggingConstants.ERROR))
          Log.log(LoggingConstants.ERROR, "Error on user data receive on Amazon EC2 server");
        if (Log.isLogging(LoggingConstants.EXCEPTION))
          Log.log(LoggingConstants.EXCEPTION,
                  "Error on user data receive on Amazon EC2 server with message: " + e.Message, e);
      }

      return null;
    }
  }
}
