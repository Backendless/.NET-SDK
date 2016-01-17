using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace Weborb.Config
{
  class CloudConfig : ORBConfigHandler
  {
    private int subscriberID;
    private String emailAddress;

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      XmlNode subscriberNode = section.SelectSingleNode( "descendant::weborb.subscriber.id" );
      XmlNode emailAddressNode = section.SelectSingleNode( "descendant::weborb.emailaddress" );

      if( subscriberNode != null && subscriberNode.InnerText != null &&
          subscriberNode.InnerText.Trim().Length != 0 )
        subscriberID = int.Parse( subscriberNode.InnerText.Trim() );

      if( emailAddressNode != null && emailAddressNode.InnerText != null &&
          emailAddressNode.InnerText.Trim().Length != 0 )
        emailAddress = emailAddressNode.InnerText.Trim();

      return this;
    }

    public static int SubscriberID
    {
      get
      {
        CloudConfig cloudConfig = (CloudConfig)ORBConfig.GetInitializedConfig().GetConfig( "weborb/cloud" );

        if( cloudConfig != null )
          return cloudConfig.subscriberID;
        else
          return -1;
      }
    }

    public static String EmailAddress
    {
      get
      {
        CloudConfig cloudConfig = (CloudConfig) ORBConfig.GetInitializedConfig().GetConfig( "weborb/cloud" );

        if( cloudConfig != null )
          return cloudConfig.emailAddress;
        else
          return null;
      }
    }
  }
}
