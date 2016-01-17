using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

using Weborb.Config;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Util
{
	public class ObjectFactories
	{
        private Dictionary<String, IServiceObjectFactory> serviceObjectFactories = new Dictionary<String, IServiceObjectFactory>();
        private Dictionary<String, IArgumentObjectFactory> argumentObjectFactories = new Dictionary<String, IArgumentObjectFactory>();

#if (FULL_BUILD)
        public static Object GetObjectOfType( String basePath, Type baseType )
        {
            AppDomain domain = AppDomain.CurrentDomain;
            String path;

            try
            {
                if( basePath == null )
                    basePath = domain.BaseDirectory;

                path = Path.Combine( basePath, "bin" );
                DirectoryInfo assemlyDirectory = new DirectoryInfo( path );

                if( !assemlyDirectory.Exists )
                    assemlyDirectory = new DirectoryInfo( basePath );

                if( Log.isLogging( LoggingConstants.DEBUG ) )
                    Log.log( LoggingConstants.DEBUG, "loading asseblies from " + assemlyDirectory.ToString() );

                FileInfo[] files = assemlyDirectory.GetFiles( "*.dll" );

                if( Log.isLogging( LoggingConstants.DEBUG ) )
                    Log.log( LoggingConstants.DEBUG, "got " + files.Length + " assembly files" );

                foreach( FileInfo file in files )
                {
                    string assemblyName = file.Name.Substring( 0, file.Name.ToLower().IndexOf( ".dll" ) );

                    if( Log.isLogging( LoggingConstants.DEBUG ) )
                        Log.log( LoggingConstants.DEBUG, "loading assembly " + assemblyName );

                    try
                    {
                        AssemblyName assemblyNameObj = AssemblyName.GetAssemblyName( file.FullName );
                        Assembly assembly = domain.Load( assemblyNameObj );
                        Type[] types = assembly.GetTypes();

                        foreach( Type type in types )
                            if( baseType.IsAssignableFrom( type ) )
                                return CreateServiceObject( type );                            
                    }
                    catch( Exception exception )
                    {
                        Log.log( LoggingConstants.ERROR, "could not load assembly - " + assemblyName, exception );
                    }
                }
            }
            catch( Exception exception )
            {
                Log.log( LoggingConstants.ERROR, "could not load assembly", exception );
            }

            return null;
        }
#endif

		public static object CreateServiceObject( string className )
		{
            //return ThreadContext.getORBConfig().getObjectFactories()._CreateServiceObject( className );
            return ORBConfig.GetInstance().getObjectFactories()._CreateServiceObject( className );
		}

        public object _CreateServiceObject( string className )
        {
            IServiceObjectFactory factory;
            serviceObjectFactories.TryGetValue( className, out factory );

            if( factory == null )
            {
                //Type type = Type.GetType( className );
                Type type = TypeLoader.LoadType( className );

                if( type == null )
                  throw new Exception( String.Format( "Unable to find type {0}", className ) );

                try
                {
                    return Activator.CreateInstance( type );
                }
                catch( TargetInvocationException exception )
                {
                    if( Log.isLogging( LoggingConstants.EXCEPTION ) )
                        Log.log( LoggingConstants.EXCEPTION, "Unable to create object instance", exception.InnerException );

                    throw exception.InnerException;
                }
            }
            else
            {
                return factory.createObject();
            }
        }

        public static object CreateServiceObject( Type type )
        {
            //return ThreadContext.getORBConfig().getObjectFactories()._CreateServiceObject( type );
            return ORBConfig.GetInstance().getObjectFactories()._CreateServiceObject( type );
        }

		public object _CreateServiceObject( Type type )
		{
            bool logDebug = Log.isLogging( LoggingConstants.DEBUG );

            if( logDebug )
                Log.log( LoggingConstants.DEBUG, "looking up service factory for " + type.FullName );

			IServiceObjectFactory factory;
            serviceObjectFactories.TryGetValue( type.FullName, out factory );

            if( factory == null )
            {
                if( logDebug )
                    Log.log( LoggingConstants.DEBUG, "factory is null" );

                if( type.IsInterface || type.IsAbstract )
                {
                    if( logDebug )
                        Log.log( LoggingConstants.DEBUG, "type is an interface or abstract/static" );

                    Type mappedType = Weborb.Types.Types.GetAbstractClassMapping( type );

                    if( mappedType == null )
                        throw new Exception( "unable to create an instance of abstract class/interface. Abstract/Interface class mapping is missing for type " + type.FullName );
                    else
                        type = mappedType;
                }

                return Activator.CreateInstance( type );
            }
            else
            {
                if( logDebug )
                    Log.log( LoggingConstants.DEBUG, "factory is " + factory.GetType().FullName );

                return factory.createObject();
            }
		}

        public static object CreateArgumentObject( Type type, IAdaptingType argument )
        {
            return ORBConfig.GetInstance().getObjectFactories()._CreateArgumentObject( type, argument );
        }

        public static object CreateArgumentObject( string typeName, IAdaptingType argument )
        {
            Type type = TypeLoader.LoadType( typeName );
            return ORBConfig.GetInstance().getObjectFactories()._CreateArgumentObject( type, argument );
        }

		public object _CreateArgumentObject( Type type, IAdaptingType argument )
		{
            String typeName = type.FullName;

            if( Log.isLogging( LoggingConstants.DEBUG ) )
                Log.log( LoggingConstants.DEBUG, "checking argument factory for " + typeName );

            if( !argumentObjectFactories.ContainsKey( typeName ) )
            {
                /*
                if( type.IsInterface || type.IsAbstract )
                {
                    if( Log.isLogging( LoggingConstants.DEBUG ) )
                        Log.log( LoggingConstants.DEBUG, "type is an interface or abstract/static" );

                    Type mappedType = Weborb.Types.Types.GetAbstractClassMapping( type );

                    if( mappedType != null )
                        return ObjectFactories.CreateServiceObject( mappedType );
                }*/

                return null;
            }

			IArgumentObjectFactory objectFactory;
            argumentObjectFactories.TryGetValue( typeName, out objectFactory );

            if( Log.isLogging( LoggingConstants.DEBUG ) )
                Log.log( LoggingConstants.DEBUG, "will use argument factory " + objectFactory.ToString() );

            return objectFactory.createObject( argument );
		}

		public void AddServiceObjectFactory( string typeName, IServiceObjectFactory objectFactory )
		{
			serviceObjectFactories[ typeName ] = objectFactory;
		}

		public void AddArgumentObjectFactory( string typeName, IArgumentObjectFactory objectFactory )
		{
			argumentObjectFactories[ typeName ] = objectFactory;
		}

        public static string[] GetMappedServiceClasses()
        {
            //return ThreadContext.getORBConfig().getObjectFactories()._GetMappedServiceClasses();
            return ORBConfig.GetInstance().getObjectFactories()._GetMappedServiceClasses();
        }

		public string[] _GetMappedServiceClasses()
		{
            List<String> serviceTypes = new List<String>();

			foreach( string typeName in serviceObjectFactories.Keys )
				serviceTypes.Add( typeName );

			return serviceTypes.ToArray();
		}

        public static IServiceObjectFactory GetServiceObjectFactory( string serviceTypeName )
        {
            //return ThreadContext.getORBConfig().getObjectFactories()._GetServiceObjectFactory( serviceTypeName );
            return ORBConfig.GetInstance().getObjectFactories()._GetServiceObjectFactory( serviceTypeName );
        }

		public IServiceObjectFactory _GetServiceObjectFactory( string serviceTypeName )
		{
            IServiceObjectFactory factory;
            serviceObjectFactories.TryGetValue( serviceTypeName, out factory );
            return factory;
		}

		public void RemoveServiceFactoryFor( string serviceTypeName )
		{
			serviceObjectFactories.Remove( serviceTypeName );
		}

        public static string[] GetMappedArgumentClasses()
        {
            //return ThreadContext.getORBConfig().getObjectFactories()._GetMappedArgumentClasses();
            return ORBConfig.GetInstance().getObjectFactories()._GetMappedArgumentClasses();
        }

		public string[] _GetMappedArgumentClasses()
		{
            List<String> argumentTypes = new List<String>();

			foreach( string typeName in argumentObjectFactories.Keys )
				argumentTypes.Add( typeName );

			return argumentTypes.ToArray();
		}

        public static IArgumentObjectFactory GetArgumentObjectFactory( string argumentTypeName )
        {
            //return ThreadContext.getORBConfig().getObjectFactories()._GetArgumentObjectFactory( argumentTypeName );
            return ORBConfig.GetInstance().getObjectFactories()._GetArgumentObjectFactory( argumentTypeName );
        }

		public IArgumentObjectFactory _GetArgumentObjectFactory( string argumentTypeName )
		{
			IArgumentObjectFactory factory;
            argumentObjectFactories.TryGetValue( argumentTypeName, out factory );
            return factory;
		}

		public void RemoveArgumentFactoryFor( string argumentTypeName )
		{
			argumentObjectFactories.Remove( argumentTypeName );
		}
	}
}
