using System;
using System.Configuration;
using System.Threading;
using System.Xml;
using System.Text;
using Weborb.Util;
using Weborb.Util.Logging;
using System.IO;

namespace Weborb.Config
{
  public abstract class ORBConfigHandler : IConfigurationSectionHandler
  {
    private XmlNode configNode;
    private ORBConfig config;

    #region IConfigurationSectionHandler Members

    public object Create( object parent, object configContext, XmlNode section )
    {
      this.config = (ORBConfig) configContext;
      this.configNode = section;
      return Configure( parent, configContext, section );
    }

    #endregion

    public void SaveConfig()
    {
      XmlTextWriter writer = null;

      try
      {
#if (CLOUD)
                writer = new XmlTextWriter(new MemoryStream(), Encoding.UTF8);
                writer.Formatting = Formatting.Indented;    
                GetConfigNode().OwnerDocument.WriteTo(writer);

                IExternalConfig extConfig = new AzureExternalConfig();
                extConfig.SaveConfig(writer);
#else
        string configFilePath = config.GetConfigFilePath();
        if( File.Exists( configFilePath ) )
        {
          int i = 0;
          while( !ORBUtil.IsFileReady( configFilePath ) && i < 1000 )
          {
            Thread.Sleep( 100 );
            i++;
          }
        }
        writer = new XmlTextWriter( configFilePath, Encoding.UTF8 );

        writer.Formatting = Formatting.Indented;
        GetConfigNode().OwnerDocument.WriteTo( writer );
#endif
      }
      catch( Exception exception )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "unable to update configuration file at " + config.GetConfigFilePath() + ". The file will be save in the alternate location - " + config.GetAlternateConfigFilePath() + "\n", exception );

        writer = new XmlTextWriter( config.GetAlternateConfigFilePath(), Encoding.UTF8 );
        writer.Formatting = Formatting.Indented;
        GetConfigNode().OwnerDocument.WriteTo( writer );
      }
      finally
      {
        if( writer != null )
          writer.Close();
      }
    }

    public XmlNode GetConfigNode()
    {
      return configNode;
    }

    public ORBConfig getORBConfig()
    {
      return config;
    }

    public void setORBConfig( ORBConfig config )
    {
      this.config = config;
    }

    public abstract object Configure( object parent, object configContext, XmlNode section );
  }
}
