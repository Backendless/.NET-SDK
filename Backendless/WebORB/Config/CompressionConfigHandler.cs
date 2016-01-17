using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Weborb.Util.Logging;
using Weborb.Util.License;

namespace Weborb.Config
{
  public class CompressionConfigHandler : ORBConfigHandler
  {
    internal int threshold = -1;
    internal String algorithm = "gzip";

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      XmlAttribute enabledAttribute = section.Attributes[ "enable" ];

      if( enabledAttribute != null )
      {
        string enabled = enabledAttribute.Value.ToLower().Trim();

        if( enabled.Equals( "yes" ) || enabled.Equals( "true" ) || enabled.Equals( "1" ) )
        {
          XmlNode thresholdNode = ( (XmlElement) section ).GetElementsByTagName( "threshold" )[ 0 ];
          threshold = int.Parse( thresholdNode.InnerText );

          XmlNodeList algorithNodes = ( (XmlElement) section ).GetElementsByTagName( "algorithm" );

          if( algorithNodes != null && algorithNodes.Count > 0 )
          {
            XmlNode algorithmNode = algorithNodes[ 0 ];
            algorithm = algorithmNode.InnerText.Trim().ToLower();
          }
        }
      }

      return this;
    }
  }
}
