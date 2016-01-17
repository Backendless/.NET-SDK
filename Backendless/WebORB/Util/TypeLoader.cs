using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Weborb.Util.Logging;
using Weborb.Service;

namespace Weborb.Util
  {
  public class TypeLoader
    {
    private static Dictionary<String, Type> cachedTypes = new Dictionary<String, Type>();
    private static Dictionary<String, String> notFoundTypes = new Dictionary<string, string>();

    public static bool TypeExists( string typeName )
      {
      return LoadType( typeName ) != null;
      }

    public static Type LoadType( string typeName )
      {
      if ( Log.isLogging( LoggingConstants.INFO ) )
        Log.log( LoggingConstants.INFO, "loading type: " + typeName );

      if ( notFoundTypes.ContainsKey( typeName ) )
        return null;

      Type type;

      cachedTypes.TryGetValue( typeName, out type );

      if ( type != null )
        return type;

      //Following line has been commented out and new code added replacing 'Type.GetType' with 'BuildManager.GetType' 
      type = Type.GetType( typeName, false );
      //			Type type = System.Web.Compilation.BuildManager.GetType( typeName, false );

      if ( type != null )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type is found" );

        cachedTypes[ typeName ] = type;
        return type;
        }

      // blob assemblies loading is turned off
#if (CLOUD)
            try
              {
              type = LoadType( CloudAssembliesRegistry.CloudAssembliesArray, typeName );

              if ( type != null )
                {
                cachedTypes[ typeName ] = type;
                return type;
                }
              }
            catch ( Exception exception )
              {
              Log.log( LoggingConstants.ERROR, "could not load assembly from CloudAssembliesRegistry", exception );
              }
#endif

#if (FULL_BUILD)
      AppDomain domain = AppDomain.CurrentDomain;
      type = LoadType( domain.GetAssemblies(), typeName );

      if ( type != null )
        {
        cachedTypes[ typeName ] = type;
        return type;
        }
      //load assemblies from bin directory

      try
        {
        DirectoryInfo binDirectory = new DirectoryInfo( Path.Combine( domain.BaseDirectory, "bin" ) );

        if ( !binDirectory.Exists )
          binDirectory = new DirectoryInfo( domain.BaseDirectory );

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "loading asseblies from " + binDirectory.ToString() );

        FileInfo[] files = binDirectory.GetFiles( "*.dll" );

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "got " + files.Length + " assembly files" );

        foreach ( FileInfo file in files )
          {
          string assemblyName = file.Name.Substring( 0, file.Name.ToLower().IndexOf( ".dll" ) );

          if ( assemblyName.Equals( "cpuinfo" ) )
            continue;

          if ( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "loading assembly " + assemblyName );

          try
            {
            domain.Load( assemblyName );
            }
          catch ( BadImageFormatException )
            {
            if ( Log.isLogging( LoggingConstants.INFO ) )
              Log.log( LoggingConstants.INFO, "Unable to load type from assembly, assembly was created with a different version of .NET. Assembly name - " + assemblyName );
            }
          catch ( Exception exception )
            {
            if ( Log.isLogging( LoggingConstants.ERROR ) )
              Log.log( LoggingConstants.ERROR, "could not load assembly - " + assemblyName, exception );
            }
          }
        }
      catch ( Exception exception )
        {
        if ( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "could not load assembly", exception );
        }

      type = LoadType( domain.GetAssemblies(), typeName );

      if ( type == null )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "type cannot be found - trying Type.GetType" );

        type = Type.GetType( typeName );

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          {
          if ( type == null )
            Log.log( LoggingConstants.DEBUG, "type cannot be found - " + typeName );
          else
            Log.log( LoggingConstants.DEBUG, "type found " + type.FullName );
          }
        }
#endif
      if ( type != null )
        cachedTypes[ typeName ] = type;
      else
        notFoundTypes[ typeName ] = typeName;

      return type;
      }

    private static Type LoadType( Assembly[] assemblies, String typeName )
      {
      foreach ( Assembly assembly in assemblies )
        {
        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "loading type from " + assembly.FullName );

        Type t = assembly.GetType( typeName, false );

        if ( t != null )
          {
          if ( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "type is found - returning type" );

          return t;
          }
        }

      return null;
      }

    //#if( FULL_BUILD )   
    //      public static void LoadAllTypes()
    //        {
    //        AppDomain domain = AppDomain.CurrentDomain;
    //        String path;

    //        try
    //          {
    //          path = Path.Combine( domain.BaseDirectory, "bin" );
    //          DirectoryInfo assemlyDirectory = new DirectoryInfo( path );
    //          if( !assemlyDirectory.Exists )
    //            assemlyDirectory = new DirectoryInfo( domain.BaseDirectory );

    //          FileInfo[] files = assemlyDirectory.GetFiles( "*.dll" );

    //          if( Log.isLogging( LoggingConstants.DEBUG ) )
    //          {
    //          Log.log( LoggingConstants.DEBUG, "loading asseblies from " + assemlyDirectory.ToString() );            
    //          Log.log( LoggingConstants.DEBUG, "got " + files.Length + " assembly files" );
    //          }

    //          foreach( FileInfo file in files )
    //            {
    //              string assemblyName = file.Name.Substring( 0, file.Name.ToLower().IndexOf( ".dll" ) );

    //              Log.log( LoggingConstants.DEBUG, "loading assembly " + assemblyName );

    //              try
    //                {
    //                  AssemblyName assemblyNameObj = AssemblyName.GetAssemblyName( file.FullName );
    //                  Assembly assembly = domain.Load( assemblyNameObj );
    //                  Type[] types = assembly.GetTypes();

    //                  foreach ( Type type in types )
    //                    {
    //                    lock ( cachedTypes )
    //                      {
    //                      cachedTypes[ type.FullName ] = type;
    //                      }
    //                    }                     
    //                }
    //              catch( Exception exception )
    //                {
    //                Log.log( LoggingConstants.ERROR, "could not load assembly - " + assemblyName, exception );
    //                }
    //            }
    //          }
    //        catch( Exception exception )
    //          {
    //          Log.log( LoggingConstants.ERROR, "could not load assembly", exception );
    //          }      
    //        }      
    //#endif

#if (CLOUD)
        public static void clearTypeCache()
        {
            cachedTypes = new Dictionary<String, Type>();
        }
#endif
    }
  }
