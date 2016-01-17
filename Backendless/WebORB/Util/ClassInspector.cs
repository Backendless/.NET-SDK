using System;
using System.Reflection;
using Weborb.Inspection;

namespace Weborb.Util
{
	/// <summary>
	/// 
	/// </summary>
	public class ClassInspector
	{
		public static ServiceDescriptor inspectClass( Type type )
		{
      BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
      if( ThreadContext.getProperties()[ Cache.Cache.CURRENT_PROTOCOL ] != "wolf" )
        flags |= BindingFlags.DeclaredOnly;

			MethodInfo[] methods = type.GetMethods( flags );
			ServiceDescriptor serviceDescriptor = new ServiceDescriptor();

			for( int i = 0; i < methods.Length; i++ )
			{
				MethodDescriptor descriptor = new MethodDescriptor();
				descriptor.name = methods[ i ].Name;
				Type returnType = methods[ i ].ReturnType;;
				descriptor.returns = ( returnType.IsArray ? "Array of " + returnType.GetElementType().FullName : returnType.FullName );
				ParameterInfo[] args = methods[ i ].GetParameters();

				for( int k = 0; k < args.Length; k++ )
				{
					Type paramType = args[k].ParameterType;
					ArgumentDescriptor argDesc = new ArgumentDescriptor();
					argDesc.name = "arg" + k;
					argDesc.type = ( paramType.IsArray ? "Array of " + paramType.GetElementType().FullName : paramType.FullName );
					descriptor.addArgument( argDesc );
				}

				serviceDescriptor.addMethod( descriptor );
			}

			return serviceDescriptor;
		}
	}
}
