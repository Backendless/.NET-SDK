using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Weborb.Config
{
  internal class CrossOriginConfigHandler : ORBConfigHandler
  {
    private Dictionary<String, OriginInfo> originInfoMap = new Dictionary<string, OriginInfo>();

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      XmlNodeList originElements = section.SelectNodes( "origin" );

      foreach( XmlNode originNode in originElements )
      {
        XmlNode hostNode = originNode.SelectSingleNode( "host" );
        XmlNode methodsNode = originNode.SelectSingleNode( "methods" );
        XmlNode allowHeadersNode = originNode.SelectSingleNode( "allow-headers" );
        XmlNode maxAgeNode = originNode.SelectSingleNode( "max-age" );

        OriginInfo originInfo = new OriginInfo();
        originInfo.Host = hostNode.InnerText.Trim();
        originInfo.Methods = methodsNode.InnerText.Trim();

        if( allowHeadersNode != null )
          originInfo.Headers = allowHeadersNode.InnerText.Trim();
        
        originInfo.MaxAge = Convert.ToInt32( maxAgeNode.InnerText.Trim() );
        originInfoMap[ originInfo.Host ] = originInfo;
      }

      return this;
    }

    internal Dictionary<String, OriginInfo> OriginInfoMap
    {
      get
      {
        return new Dictionary<string, OriginInfo>( originInfoMap );
      }
    }
  }

  internal class OriginInfo
  {
    public String Host { get; set; }
    public String Methods { get; set; }
    public String Headers { get; set; }
    public int MaxAge { get; set; }
  }
}
