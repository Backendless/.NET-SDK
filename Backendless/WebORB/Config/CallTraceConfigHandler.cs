using System;
using System.Xml;
using System.Configuration;
using Weborb;
using Weborb.Calltrace;

namespace Weborb.Config
{
	/// <summary>
	/// Summary description for CallTraceConfigHandler.
	/// </summary>
	public class CallTraceConfigHandler : ORBConfigHandler
	{
		private CallTraceEventDispatcher dispatcher;
        private bool isEnabled = false;

		public override object Configure( object parent, object configContext, XmlNode section )
		{
            //if( Weborb.Util.License.LicenseManager.GetInstance().IsStandardLicense() )
            //    return null;

			isEnabled = section.Attributes.GetNamedItem( ORBConstants.ENABLE ).InnerText.ToLower().Equals( ORBConstants.YES );

			int queueFlushThreshold = 0;
			int queueCheckWaitTime = 0;
			string callStoreFolder = null;
			int callStoreBufferSize;
			int maxCallsPerCallStoreFile = 0;
			
			foreach( XmlNode node in section.ChildNodes )
			{
				switch( node.Name )
				{
					case( ORBConstants.QUEUE_FLUSH_THRESHOLD ):
						queueFlushThreshold = Convert.ToInt32( node.InnerText ); 
						break;
					case( ORBConstants.QUEUE_CHECK_WAIT_TIME ):
						queueCheckWaitTime = Convert.ToInt32( node.InnerText ); 
						break;
					case( ORBConstants.CALL_STORE_FOLDER ):
						callStoreFolder = node.InnerText;
						break;
					case( ORBConstants.CALL_STORE_BUFFER_SIZE ):
						callStoreBufferSize = Convert.ToInt32( node.InnerText ); 
						break;
					case( ORBConstants.MAX_CALLS_PER_FILE ):
						maxCallsPerCallStoreFile = Convert.ToInt32( node.InnerText );
						break;
					default:
						break;
				}
			}

			if( queueFlushThreshold > 0 )
				CallTraceEventDispatcher.setQueueFlushThreshold( queueFlushThreshold );

			if( queueCheckWaitTime > 0 )
				CallTraceEventDispatcher.setQueueCheckTime( queueCheckWaitTime );

			if( maxCallsPerCallStoreFile > 0 )
				CallTraceStore.setMaxCallsPerCallStoreFile( maxCallsPerCallStoreFile );

			if( callStoreFolder != null )
				CallTraceStore.setCallTraceStoreDirectory( AppDomain.CurrentDomain.BaseDirectory + callStoreFolder );

            if( isEnabled )
            {
                dispatcher = CallTraceEventDispatcher.getInstance();
                dispatcher.addListener( CallTraceStore.getStore() );
                dispatcher.start();
            }

			return this;
		}

        public CallTraceEventDispatcher getDispatcher()
        {
            return dispatcher;
        }

        public void enable()
        {
            if( IsEnabled )
                return;

            XmlNode configNode = GetConfigNode();
            configNode.Attributes[ ORBConstants.ENABLE ].Value = ORBConstants.YES;
            isEnabled = true;
            dispatcher = CallTraceEventDispatcher.getInstance();
            dispatcher.addListener( CallTraceStore.getStore() );
            dispatcher.start();
            SaveConfig();
        }

        public void disable()
        {
            if( !IsEnabled )
                return;

            XmlNode configNode = GetConfigNode();
            configNode.Attributes[ ORBConstants.ENABLE ].Value = ORBConstants.NO;
            isEnabled = false;
            dispatcher.removeListener( CallTraceStore.getStore() );
            dispatcher.stop();
            SaveConfig();
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
        }
	}
}
