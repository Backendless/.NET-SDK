using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

using Weborb.Util.Logging;
using Weborb.Messaging;

namespace Weborb.Config
{
  public class WebSocketConfigHandler : ORBConfigHandler
  {
    private List<int> _ports = new List<int>();

    public override object Configure( object parent, object configContext, XmlNode section )
    {
      XmlNodeList portNodes = section.SelectNodes( "server-port" );

      foreach( XmlNode portNode in portNodes )
      {
        try
        {
          int port = int.Parse( portNode.InnerText.Trim() );
          AddPort( port, true );
        }
        catch( Exception exception )
        {
          String error = "Unable to register WebSocket port - " + portNode.InnerText;

          if( Log.isLogging( LoggingConstants.ERROR ) )
            Log.log( LoggingConstants.ERROR, error );

          if( Log.isLogging( LoggingConstants.EXCEPTION ) )
            Log.log( LoggingConstants.EXCEPTION, error, exception );
        }
      }

      return this;
    }

    public static WebSocketConfigHandler GetInstance()
    {
      return (WebSocketConfigHandler) ORBConfig.GetInstance().GetConfig( "weborb/websockets" );
    }

    public void AddPort( int port, Boolean startServer )
    {
      _ports.Add( port );

      if( startServer )
      {
        if( Log.isLogging( LoggingConstants.INFO ) )
          Log.log( LoggingConstants.INFO, "Starting Web Socket server on port " + port );

        RTMPServer server = new RTMPServer( "WS-" + port, port, 500, ORBConfig.GetInstance(), true );
        server.start();
      }
    }

    public void removePort( int port )
    {
      _ports.Remove( port );
      RTMPServer.GetWSServers()[ port ].shutdown();
    }
    public static int[] Ports
    {
      get { return GetInstance()._ports.ToArray(); }
    }
  }
}
