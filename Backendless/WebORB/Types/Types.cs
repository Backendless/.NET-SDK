using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Config;

namespace Weborb.Types
{
	public class Types
	{
        private Dictionary<Type, Type> abstractMappings = new Dictionary<Type, Type>();
        private Dictionary<String, Type> clientMappings = new Dictionary<String, Type>();
        private Dictionary<String, String> serverMappings = new Dictionary<String, String>();

        public static Type GetAbstractClassMapping( Type type )
        {
            //return ThreadContext.getORBConfig().getTypeMapper()._GetAbstractClassMapping( type );
            return ORBConfig.GetInstance().getTypeMapper()._GetAbstractClassMapping( type );
        }

		public Type _GetAbstractClassMapping( Type type )
		{
            Type abstractType;
            abstractMappings.TryGetValue( type, out abstractType );

            if( abstractType == null && type.IsGenericType )
            {
                Type genericTypeDef = type.GetGenericTypeDefinition();
                abstractMappings.TryGetValue( genericTypeDef, out abstractType );

                if( abstractType != null )
                {
                    Type[] genericArgTypes = type.GetGenericArguments();
                    abstractType = abstractType.MakeGenericType( genericArgTypes );
                }
            }

            return abstractType;
		}

        public static void AddAbstractTypeMapping( Type abstractType, Type mappedType )
        {
            //ThreadContext.getORBConfig().getTypeMapper()._AddAbstractTypeMapping( abstractType, mappedType );
            ORBConfig.GetInstance().getTypeMapper()._AddAbstractTypeMapping( abstractType, mappedType );
        }

		public void _AddAbstractTypeMapping( Type abstractType, Type mappedType )
		{
			abstractMappings[ abstractType ] = mappedType;
		}

        public static void AddClientClassMapping( string clientClass, Type mappedServerType )
        {
            //ThreadContext.getORBConfig().getTypeMapper()._AddClientClassMapping( clientClass, mappedServerType );
            ORBConfig.GetInstance().getTypeMapper()._AddClientClassMapping( clientClass, mappedServerType );
        }

        public void _AddClientClassMapping( string clientClass, Type mappedServerType )
        {
            clientMappings[ clientClass ] = mappedServerType;
            serverMappings[ mappedServerType.FullName ] = clientClass;
        }

        public static Type getServerTypeForClientClass( string clientClass )
        {
            //return ThreadContext.getORBConfig().getTypeMapper()._getServerTypeForClientClass( clientClass );
            return ORBConfig.GetInstance().getTypeMapper()._getServerTypeForClientClass( clientClass );
        }

        public Type _getServerTypeForClientClass( string clientClass )
        {
            Type type;

            clientMappings.TryGetValue( clientClass, out type );

            if( type == null )
            {
                try
                {
                    type = TypeLoader.LoadType( clientClass );

                    if( type != null )
                        clientMappings[ clientClass ] = type;
                }
                catch( Exception )
                {
                    if( Log.isLogging( LoggingConstants.ERROR ) )
                        Log.log( LoggingConstants.ERROR, "unable to find matching server-side type for " + clientClass );
                }
            }

            return type;
        }

        public static string getClientClassForServerType( string serverClassName )
        {
            //return ThreadContext.getORBConfig().getTypeMapper()._getClientClassForServerType( serverClassName );
            return ORBConfig.GetInstance().getTypeMapper()._getClientClassForServerType( serverClassName );
        }

        public string _getClientClassForServerType( string serverClassName )
        {
            String clientClass;
            serverMappings.TryGetValue( serverClassName, out clientClass );
            return clientClass;
        }
	}
}
