using System;
using System.Reflection;
using System.Collections;

using Weborb.Config;
using Weborb.Service;
using Weborb.Types;

namespace Weborb.Util
  {
	/// <summary>
	/// 
	/// </summary>
	public class MethodLookup
	  {
    public delegate bool MatchDelegate( object argument, Type formalArg );

    public static MethodInfo findMethod( Type type, string methodName, object[] arguments )
      {
      return findMethod( type, methodName, arguments, looseMatch );
      }

		public static MethodInfo findMethod( Type type, string methodName, object[] arguments, MatchDelegate match )
		  {
		  // TODO: implement lookup in the hierarchy
		  //MethodInfo[] methods = type.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy );
		  if( type == null )
		    throw new MissingMethodException( "unable to find method with name " + methodName + ". Type is null" );

		  SerializationConfigHandler serializationConfig =
		    (SerializationConfigHandler)ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

		  if( methodName.StartsWith( serializationConfig.PrefixForKeywords ) )
		    methodName = methodName.Substring( serializationConfig.PrefixForKeywords.Length );

      ArrayList selectedMethods = new ArrayList();
      MethodInfo[] methods;

      try
      {
        MethodInfo quickMethodSearch = type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.FlattenHierarchy );
        
        if( quickMethodSearch == null )
          throw new MissingMethodException( "unable to find method with name " + methodName + " and the given argument types" );

        selectedMethods.Add( quickMethodSearch );
      }
      catch( AmbiguousMatchException )
      {
      }

      if( selectedMethods.Count == 0 )
      {
        methods = type.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Static );
        int argCount = arguments.Length;

        for( int i = 0; i < methods.Length; i++ )
        {
          MethodInfo method = methods[ i ];
          ParameterInfo[] formalArgs = method.GetParameters();

          if( method.Name.Equals( methodName ) && formalArgs.Length == argCount )
          {
            int j = 0;
            for( ; j < formalArgs.Length; j++ )
              if( !match( arguments[ j ], formalArgs[ j ].ParameterType ) )
                break;

            if( j == formalArgs.Length )
              selectedMethods.Add( method );
          }
        }

        // for some reason base interfaces doesn't push thier methods to Type#GetMethods
        if( type.IsInterface )
          foreach( Type @interface in type.GetInterfaces() )
          {
            MethodInfo method = null;
            try
            {
              method = findMethod( @interface, methodName, arguments, match );
            }
            catch { }

            if( method != null )
              selectedMethods.Add( method );
          }
      }

		  methods = new MethodInfo[selectedMethods.Count];
		  selectedMethods.CopyTo( methods );

		  if( methods.Length == 0 )
		    throw new MissingMethodException( "unable to find method with name " + methodName +
		                                      " and the given argument types" );
		  else if( methods.Length > 1 )
		    return resolveAmbiguity( type, methods, arguments );

		  return methods[ 0 ];
		  }

	  private static MethodInfo resolveAmbiguity( Type type, MethodInfo[] methods, object[] arguments )
	  {         
	      MethodAmbiguityResolverAttribute[] resolverTypes =
              (MethodAmbiguityResolverAttribute[])type.GetCustomAttributes(typeof(MethodAmbiguityResolverAttribute), false);
          if (resolverTypes != null && resolverTypes.Length > 0)
          {
              object resolverObj = Activator.CreateInstance(resolverTypes[0].ResolverType);
              IMethodAmbiguityResolver resolver = resolverObj as IMethodAmbiguityResolver;
              if (resolver != null)
              {
                  return resolver.ResolveMethod(methods, arguments);
              }
          }

		  throw new Exception( "unable to resolve method ambiguity. Method - " + methods[ 0 ].Name );
		  }

	  public static bool looseMatch( object argument, Type formalArg )
		  {
		  Type argType = argument == null ? typeof( Nullable ) : argument.GetType();

		  if( !argType.IsAssignableFrom( formalArg ) )
		    if( argument is IAdaptingType )
		      return ( (IAdaptingType)argument ).canAdaptTo( formalArg );

		  return true;
		  }
	  }
  }
