using System;
using System.Configuration;
using System.Xml;
using Weborb.Util.Logging;
using Weborb.Reader.Dataset;

namespace Weborb.Config
{
	public class DatasetConfigPathHandler : ORBConfigHandler
	{
        private bool legacySerialization = false;

		public override object Configure( object parent, object configContext, XmlNode section )
		{
            /*if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            {
                if( Log.isLogging( LoggingConstants.INFO ) )
                    Log.log( LoggingConstants.INFO, "skipping dataset configuration, this functionality is not available in WebORB Standard Ediition" );

                return this;
            }*/

			foreach( XmlNode node in section.ChildNodes )
                if( node.Name.Equals( "defaultPageSize" ) )
                {
                    int pageSize;

                    try
                    {
                        pageSize = int.Parse( node.InnerText.Trim() );
                    }
                    catch
                    {
                        if( Log.isLogging( LoggingConstants.ERROR ) )
                            Log.log( LoggingConstants.ERROR, "Invalid configuration setting. Page size value is not a number. value = " + node.InnerText.Trim() );
                    }

                    RemotingDataSet.SetDefaultPageSize( int.Parse( node.InnerText.Trim() ) );
                }
                else if( node.Name.Equals( "legacySerialization" ) )
                {
                    legacySerialization = node.InnerText.Equals( ORBConstants.YES );                    
                }

			return this;
		}

        public bool LegacySerialization
        {
            get { return legacySerialization; }
        }
	}
}
