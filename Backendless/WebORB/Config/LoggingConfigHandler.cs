using System;
using System.Xml;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Weborb;
using Weborb.Util;
using Weborb.Util.Logging;
using System.Collections.Generic;

namespace Weborb.Config
{
	public class LoggingConfigHandler : ORBConfigHandler
	{
		private ILoggingPolicy currentPolicy;
		private Hashtable loggingPolices = new Hashtable();

		public override object Configure( object parent, object configContext, XmlNode section )
		  {
		  string currentPolicyName = null;
		  IList categories = new ArrayList();
		  Hashtable parameters = new Hashtable();

		  foreach( XmlNode node in section.ChildNodes )
		    {
		    switch( node.Name )
		      {
		      case ( ORBConstants.PARAMETER ):
		        processParameter( node, parameters );
		        break;
		      case ( ORBConstants.CURRENT_POLICY ):
		        currentPolicyName = node.InnerText;
		        break;
		      case ( ORBConstants.LOGGING_POLICY ):
		        processPolicy( node );
		        break;
		      case ( ORBConstants.LOG ):
		        if( node.Attributes.GetNamedItem( ORBConstants.ENABLE ).InnerText.Equals( ORBConstants.YES ) )
		          categories.Add( node.InnerText.Trim() );
		        break;
		      }
		    }

		  if( currentPolicyName != null )
		    {
		    currentPolicy = (ILoggingPolicy)loggingPolices[ currentPolicyName ];

		    if( currentPolicy != null )
		      {
		      if( Log.isLogging( LoggingConstants.DEBUG ) )
		        Log.log( LoggingConstants.DEBUG, "adding logger with current policy: " + currentPolicyName );

		      ILogger logger = currentPolicy.getLogger();
          try
            {
            if ( parameters.ContainsKey( ORBConstants.DATE_FORMATTER ) )
              logger.setLogDateTime( Boolean.Parse( (string) parameters[ ORBConstants.DATE_FORMATTER ] ), null );
            
            if( parameters.ContainsKey( ORBConstants.LOG_THREAD_NAME ) )
              logger.setLogThreadName( Boolean.Parse( (string)parameters[ ORBConstants.LOG_THREAD_NAME ] ) );
            }
          catch( Exception e )
            {
            if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
              Log.log( LoggingConstants.EXCEPTION,
                     "Cann't read " + ORBConstants.DATE_FORMATTER + " and/or " + ORBConstants.LOG_THREAD_NAME, e );
            }
		      Log.addLogger( Log.DEFAULTLOGGER, logger );
		      }
		    else
          if ( Log.isLogging( LoggingConstants.ERROR ) )
		        Log.log( LoggingConstants.ERROR,
		               "the current policy value of " + currentPolicy + " does not match any policyName elements" );
		    }

		  foreach( object category in categories )
		    Log.startLogging( (string)category );

		  return this;
		  }

	  private void processPolicy( XmlNode policyNode )
		{
			string policyName = null;
			string className = null;
			Hashtable initParams = new Hashtable( 2 );

			foreach( XmlNode node in policyNode.ChildNodes )
			{
				switch( node.Name )
				{
					case( ORBConstants.POLICY_NAME ):
						policyName = node.InnerText;
						break;
					case( ORBConstants.CLASS_NAME ):
						className = node.InnerText;
						break;
					case( ORBConstants.PARAMETER ):
						processParameter( node, initParams );
						break;
				}
			}

			if( policyName != null && className != null )
			{
				try
				{
					Type policyType = TypeLoader.LoadType( className );
					ConstructorInfo constructor = policyType.GetConstructor( new Type[] { typeof( Hashtable ) } );

					object policy = constructor.Invoke( new object[] { initParams } );
					
					if( policy is ILoggingPolicy )
						loggingPolices.Add( policyName, policy );
				}
				catch( Exception exception )
				{
					Log.startLogging( LoggingConstants.ERROR );
					Log.log( LoggingConstants.ERROR, "exception of type: " + exception.GetType() + " inner exception: " + exception.InnerException.StackTrace, exception );
				}

			}
		}

		private void processParameter( XmlNode parameterNode, Hashtable initParams )
		{
			string nameText = null;
			string valueText = null;

			foreach( XmlNode node in parameterNode.ChildNodes )
			{
				switch( node.Name )
				{
					case( ORBConstants.NAME ):
						nameText = node.InnerText;
						break;
					case( ORBConstants.VALUE ):
						valueText = node.InnerText;
						break;
				}
			}

			if( nameText != null && valueText != null )
				initParams.Add( nameText, valueText );
		}

		public ILoggingPolicy getCurrentPolicy()
		{
			return this.currentPolicy;
		}

        public String getLog(String fileName, int page)
        {
            LogReader readerLogger = LogReader.Instance;
            return readerLogger.getPage(fileName, page); 
        }

        public List<String> getFileList()
          {
          List<String> fileNames = new List<string>();
          foreach( ILogger logger in Log.getLoggers() )
            {
            if( logger is TraceLogger )
              {
              try
                {
                fileNames.AddRange( ( (TraceLogger)logger ).getFileNames() );
                }
              catch( ArgumentNullException )
                {
                }
              return fileNames;
              }
            }
          return fileNames;
          }

	  public Hashtable getFileParams(String fileName)
        {
            Hashtable hashtable = new Hashtable();          

            LogReader readerLogger = LogReader.Instance;
            hashtable.Add("numberOfPages", readerLogger.getNumberOfPages(fileName));

            return hashtable;
        }

		public Hashtable getLoggingPolicies()
		{
			Hashtable policyTable = new Hashtable();
			Hashtable policies = new Hashtable();
			policyTable.Add( "currentPolicy", currentPolicy.getPolicyName() );
			XmlNode configNode = GetConfigNode();
           
            foreach (XmlNode node in configNode.ChildNodes)
            {
                if (node.Name.Equals("parameter"))
                {
                    XmlElement paramContainer = (XmlElement)node;
                    string paramName = paramContainer.GetElementsByTagName("name")[0].InnerText.Trim();
                    string paramValue = paramContainer.GetElementsByTagName("value")[0].InnerText.Trim();
                    policyTable.Add(paramName, paramValue);
                }

                if (node.Name.Equals("loggingPolicy"))
                {
                    XmlElement policyContainer = (XmlElement)node;
                    string policyName = policyContainer.GetElementsByTagName("policyName")[0].InnerText.Trim();
                    Hashtable paramMap = new Hashtable();

                    foreach (XmlNode policyParam in policyContainer.ChildNodes)
                    {
                        if (policyParam.Name.Equals("parameter"))
                        {
                            string paramName = ((XmlElement)policyParam).GetElementsByTagName("name")[0].InnerText.Trim();
                            string paramValue = ((XmlElement)policyParam).GetElementsByTagName("value")[0].InnerText.Trim();
                            paramMap.Add(paramName, paramValue);
                        }
                    }

                    policies.Add(policyName, paramMap);
                }
            }

			policyTable.Add( "policies", policies );
			return policyTable;
		}

		public void setLoggingPolicy( string loggerName, ILogger logger )
		{
            long mask = Log.getLogger( Log.DEFAULTLOGGER ).getMask();
			logger.setMask( mask );
			Log.addLogger( loggerName, logger );
		}

        public void EnableCategory( string category, bool enabled )
        {
            XmlNode configNode = GetConfigNode();

            foreach( XmlNode node in configNode.ChildNodes )
                if( node.Name.Equals( ORBConstants.LOG ) && node.InnerText.Trim().Equals( category ) )
                {
                    node.Attributes.GetNamedItem( ORBConstants.ENABLE ).InnerText = enabled ? ORBConstants.YES : ORBConstants.NO;
                    SaveConfig();
                    return;
                }
        }

		public void setCurrentPolicy( ILoggingPolicy policy )
		  {
		  this.currentPolicy = policy;
		  string currentPolicyName = policy.getPolicyName();
		  Hashtable policyParams = policy.getPolicyParameters();
		  XmlNode configNode = GetConfigNode();
      string dateForamatter = policyParams[ ORBConstants.DATE_FORMATTER ].ToString();
      string logThreadName = policyParams[ ORBConstants.LOG_THREAD_NAME ].ToString();

		  policyParams.Remove( ORBConstants.DATE_FORMATTER );
		  policyParams.Remove( ORBConstants.LOG_THREAD_NAME );

		  foreach( XmlNode node in configNode.ChildNodes )
		    {
		    if( node.Name.Equals( "parameter" ) )
		      {
		      XmlElement paramContainer = (XmlElement)node;
		      string paramName = paramContainer.GetElementsByTagName( "name" )[ 0 ].InnerText.Trim();
		      XmlNode paramValueNode = paramContainer.GetElementsByTagName( "value" )[ 0 ];

          if( paramName.Equals( ORBConstants.DATE_FORMATTER ) )
		        paramValueNode.InnerText = dateForamatter;
          else if( paramName.Equals( ORBConstants.LOG_THREAD_NAME ) )
		        paramValueNode.InnerText = logThreadName;
		      }
		    else if( node.Name.Equals( "currentPolicy" ) )
		      node.InnerText = currentPolicyName;
		    else if( node.Name.Equals( "loggingPolicy" ) )
		      {
		      XmlElement policyContainer = (XmlElement)node;
		      string policyName = policyContainer.GetElementsByTagName( "policyName" )[ 0 ].InnerText.Trim();

		      if( policyName.Equals( currentPolicyName ) )
		        {
		        XmlNodeList list = policyContainer.GetElementsByTagName( "parameter" );

		        foreach( XmlNode paramElement in list )
		          {
		          string name = ( (XmlElement)paramElement ).GetElementsByTagName( "name" )[ 0 ].InnerText.Trim();
		          object paramValue = policyParams[ name ];
		          XmlNode valueNode = ( (XmlElement)paramElement ).GetElementsByTagName( "value" )[ 0 ];
		          valueNode.InnerText = paramValue.ToString();
		          }
		        }
		      }
		    }
		  SaveConfig();

		  ILogger logger = policy.getLogger();
		  logger.setLogDateTime( bool.Parse( dateForamatter ), null );
		  logger.setLogThreadName( bool.Parse( logThreadName ) );      
      logger.setMask( Log.getLogger( Log.DEFAULTLOGGER ).getMask() );
      
		  Log.addLogger( Log.DEFAULTLOGGER, logger );
		  }
	}
}
