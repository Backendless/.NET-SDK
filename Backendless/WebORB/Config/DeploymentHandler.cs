using System;
using System.Configuration;
using System.Xml;
using Weborb.Util.Logging;
using Weborb.Reader.Dataset;

namespace Weborb.Config
{
  public class DeploymentHandler : ORBConfigHandler
  {
    private bool _enableConsoleUpload = false;

    public bool EnableConsoleUpload
    {
      get { return _enableConsoleUpload; }
    }

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      foreach( XmlNode node in section.ChildNodes )
      {
        string nodeValue = node.InnerText.Trim().ToLower();

        switch( node.Name )
        {
          case "enableConsoleUpload":
            _enableConsoleUpload = nodeValue == "true" || nodeValue == "yes" || nodeValue == "1";
            break;
        }
      }

      return this;
    }
  }
}
