using System;
using System.Collections.Generic;
using System.Xml;

namespace Weborb.Config
  {
  public class FlexMessagingDescriptionsConfig : ORBConfigHandler
    {
    private Dictionary<string, string> descriptions = new Dictionary<string, string>();

    public string getSectionWrapperName()
      {
      return "destinationDescriptions";
      }

    public override object Configure( object parent, object configContext, XmlNode section )
      {      
      XmlNodeList descriptionNodes =  section.SelectNodes( "description" );

      foreach ( XmlNode descriptionNode in descriptionNodes )
        {
        descriptions.Add( descriptionNode.Attributes[ "name" ].Value, descriptionNode.InnerText );
        }

      return this;
      }

    public Dictionary<string, string> GetDescriptions()
      {
      return descriptions;
      }
    }
  }