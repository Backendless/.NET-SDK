using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;

using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Handler;
using Weborb.Management.ServiceBrowser;

namespace Weborb.Config
  {
  public class ServiceBrowserConfig : ORBConfigHandler
    {
    private List<IServiceBrowser> serviceBrowsers = new List<IServiceBrowser>();
    private Dictionary<string, Dictionary<string, List<string>>> browsersToProperties =
      new Dictionary<string, Dictionary<string, List<string>>>();

    public override object Configure( object parent, object configContext, System.Xml.XmlNode section )
      {
      XmlNodeList serviceBrowserNodeList = ( (XmlElement)section ).GetElementsByTagName( "serviceBrowser" );

      foreach ( XmlNode node in serviceBrowserNodeList )
        {
        try
          {
          XmlElement serviceBrowserNode = (XmlElement)node;
          String className = serviceBrowserNode.SelectNodes( "className" )[ 0 ].InnerText.Trim();

          if ( className == "Weborb.Management.ServiceBrowser.Browser.NHibernateBrowser"
               && !NHibernateHandler.NHibernateIsInstalled() )
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "NHibernate.dll wasn't found. NHibernate Service Browser " + 
                                                                                "and Handler won't be working" );
            continue;
            }

          if ( className == "Weborb.Management.ServiceBrowser.Browser.WCFBrowser" &&
              ORBUtil.GetMajorNETVersionOfWeborb() < 3 )
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "This version of .NET doesn't support WCF services, so WCF Service "+
                                                                            "Browser and Handler will be turned off" );
            continue;
            }

          if ( className == "Weborb.Management.ServiceBrowser.Browser.WCFRIABrowser" &&
              ORBUtil.GetMajorNETVersionOfWeborb() < 4 )
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "This version of .NET doesn't support to use WCF RIA services, so "+
                                                              "WCF RIA Service Browser and Handler will be turned off" );
            continue;
            }

          XmlNodeList properties = serviceBrowserNode.SelectNodes( "properties/property" );
          Dictionary<String, List<String>> props = null;

          if ( properties != null )
            {
            browsersToProperties[ className ] = new Dictionary<String, List<String>>();
            props = browsersToProperties[ className ];

            foreach ( XmlNode propNode in properties )
              {
              string propName = propNode.Attributes[ "name" ].Value;
              string propValue = propNode.InnerText.Trim();
              List<String> values;

              if ( !props.ContainsKey( propName ) )
                {
                values = new List<string>();
                props[ propName ] = values;
                }
              else
                {
                values = props[ propName ];
                }

              values.Add( propValue );
              }
            }

          Type serviceBrowserType = TypeLoader.LoadType( className );

          if ( serviceBrowserType != null )
            {
              try
              {
                IServiceBrowser serviceBrowser = CreateServiceBrowser( className, props );
                serviceBrowsers.Add( serviceBrowser );
              }
              catch( Exception exception )
              {
                if( Log.isLogging( LoggingConstants.ERROR ) )
                  Log.log( LoggingConstants.ERROR, String.Format( "Unable to create service browser {0} due to error {1}", className, exception.Message ) );
              }
            }
          else
            {
            if ( Log.isLogging( LoggingConstants.INFO ) )
              Log.log( LoggingConstants.INFO, "Unable to locate service browser type for " + className );
            }
          }
        catch ( Exception e )
          {
          if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
            Log.log( LoggingConstants.EXCEPTION, "Cann't add service browser", e );
          }
        }

      return this;
      }

    private IServiceBrowser CreateServiceBrowser( String className, Dictionary<String, List<String>> properties )
      {
      IServiceBrowser serviceBrowser = (IServiceBrowser)ObjectFactories.CreateServiceObject( className );

      if ( properties != null )
        serviceBrowser.SetInitProperties( properties );

      return serviceBrowser;
      }

    public Dictionary<string, Dictionary<string, List<string>>> BrowsersToProperties
      {
      get { return browsersToProperties; }
      }

    public IList<IServiceBrowser> ServiceBrowsers
      {
      get { return serviceBrowsers; }
      }
    }
  }
