using System;
using System.Collections;
using Weborb.Util.Logging;
using Weborb.Handler;
using Weborb.Util;

namespace Weborb.Config
{
	public class Administration
	{
		// *********************** INVOCATION HANDLERS **********************************

		public string[] getInvocationHandlers()
		{
			IInvocationHandler[] handlers = Handlers.GetInvocationHandlers();
			string[] handlerTypeNames = new string[ handlers.Length ];

			for( int i = 0; i < handlerTypeNames.Length; i++ )
				handlerTypeNames[ i ] = handlers[ i ].GetType().FullName;

			return handlerTypeNames;
		}

		public string addInvocationHandler( string typeName )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			InvokersConfigHandler config = (InvokersConfigHandler) orbconfig.GetConfig( "weborb/invokers" );
			config.AddInvoker( typeName );
			return typeName;
		}

		public string removeInvocationHandler( string typeName )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			InvokersConfigHandler config = (InvokersConfigHandler) orbconfig.GetConfig( "weborb/invokers" );
			config.RemoveInvoker( typeName );
			return typeName;
		}

		// *********************** INVOCATION HANDLERS **********************************

		public string[] getInspectionHandlers()
		{
			IInspectionHandler[] handlers = Handlers.GetInspectionHandlers();
			string[] handlerTypeNames = new string[ handlers.Length ];

			for( int i = 0; i < handlerTypeNames.Length; i++ )
				handlerTypeNames[ i ] = handlers[ i ].GetType().FullName;

			return handlerTypeNames;
		}

		public string addInspectionHandler( string typeName )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			InspectorsConfigHandler config = (InspectorsConfigHandler) orbconfig.GetConfig( "weborb/inspectors" );
			config.AddInspector( typeName );
			return typeName;
		}

		public string removeInspectionHandler( string typeName )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			InspectorsConfigHandler config = (InspectorsConfigHandler) orbconfig.GetConfig( "weborb/inspectors" );
			config.RemoveInspector( typeName );
			return typeName;
		}

		// *********************** SERVICE FACTORIES ************************************

		public string[,] getServiceFactoriesMappings()
		{
			string[] typeNames = ObjectFactories.GetMappedServiceClasses();
			string[,] mappings = new string[ typeNames.Length , 2 ];

			for( int i = 0; i < typeNames.Length; i++ )
			{
				mappings[ i , 0 ] = typeNames[ i ];
				mappings[ i , 1 ] = ObjectFactories.GetServiceObjectFactory( typeNames[ i ] ).GetType().FullName;
			}

			return mappings;
		}

		public string[] addServiceFactory( string serviceFactoryTypeName, string serviceTypeName )
		{
			serviceFactoryTypeName = serviceFactoryTypeName.Trim();
			serviceTypeName = serviceTypeName.Trim();

            //ORBConfig config = ThreadContext.getORBConfig();
            ORBConfig config = ORBConfig.GetInstance();
			ServiceFactoriesConfigHandler configHandler = (ServiceFactoriesConfigHandler) config.GetConfig( "weborb/serviceFactories" );
			configHandler.AddServiceFactory( serviceFactoryTypeName, serviceTypeName );

			String[] returnArray = new String[ 2 ];
			returnArray[ 0 ] =  serviceFactoryTypeName;
			returnArray[ 1 ] =  serviceTypeName;
			return returnArray;
		}

		public void removeServiceFactory( string typeName )
		{
            //ORBConfig config = ThreadContext.getORBConfig();
            ORBConfig config = ORBConfig.GetInstance();
			ServiceFactoriesConfigHandler configHandler = (ServiceFactoriesConfigHandler) config.GetConfig( "weborb/serviceFactories" );
			configHandler.RemoveServiceFactoryFor( typeName );
		}

		// *********************** ARGUMENT FACTORIES ***********************************

		public string[,] getArgumentFactoriesMappings()
		{
			string[] typesNames = ObjectFactories.GetMappedArgumentClasses();
			string[,] mappings = new String[ typesNames.Length, 2 ];

			for( int i = 0; i < typesNames.Length; i++ )
			{
				mappings[ i, 0 ] = typesNames[ i ];
				mappings[ i, 1 ] = ObjectFactories.GetArgumentObjectFactory( typesNames[ i ] ).GetType().FullName;
			}

			return mappings;
		}

		public string[] addArgumentFactory( string argumentFactoryTypeName, string argumentTypeName )
		{
			argumentFactoryTypeName = argumentFactoryTypeName.Trim();
			argumentTypeName = argumentTypeName.Trim();

            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			ArgumentFactoriesConfigHandler configHandler = (ArgumentFactoriesConfigHandler) orbconfig.GetConfig( "weborb/argumentFactories" );
			configHandler.AddArgumentFactory( argumentFactoryTypeName, argumentTypeName );

			String[] returnArray = new String[ 2 ];
			returnArray[ 0 ] =  argumentFactoryTypeName;
			returnArray[ 1 ] =  argumentTypeName;
			return returnArray;
		}

		public void removeArgumentFactory( string typeName )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			ArgumentFactoriesConfigHandler configHandler = (ArgumentFactoriesConfigHandler) orbconfig.GetConfig( "weborb/argumentFactories" );
			configHandler.RemoveServiceFactoryFor( typeName );
		}

		// *********************** LOGGING ***********************************

		public ArrayList getLoggingCategories()
		{
			IEnumerator catEnum = Log.getCategories();
			ArrayList categories = new ArrayList();

			while( catEnum.MoveNext() )
			{
				string category = (string) catEnum.Current;
				bool isLogging = Log.isLogging( category );
				string[] catArray = new string[ 2 ];
				catArray[ 0 ] = category;
				catArray[ 1 ] = isLogging.ToString();
				categories.Add( catArray );
			}

			return categories;
		}

		public Hashtable getLoggingPolicies()
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			LoggingConfigHandler loggingConfig = (LoggingConfigHandler) orbconfig.GetConfig( "weborb/logging" );
			return loggingConfig.getLoggingPolicies();
		}

		public string setLoggingPolicy( ILoggingPolicy policy )
		{
            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
			LoggingConfigHandler loggingConfig = (LoggingConfigHandler) orbconfig.GetConfig( "weborb/logging" );
			loggingConfig.setLoggingPolicy( Log.DEFAULTLOGGER, policy.getLogger() );
			loggingConfig.setCurrentPolicy( policy );
			return policy.getPolicyName();
		}

		public void enableCategory( string category, bool enabled )
		{
			if( enabled )
				Log.startLogging( category );
			else
				Log.stopLogging( category );

            //ORBConfig orbconfig = ThreadContext.getORBConfig();
            ORBConfig orbconfig = ORBConfig.GetInstance();
            LoggingConfigHandler loggingConfig = (LoggingConfigHandler) orbconfig.GetConfig( "weborb/logging" );
            loggingConfig.EnableCategory( category, enabled );
		}
	}
}
