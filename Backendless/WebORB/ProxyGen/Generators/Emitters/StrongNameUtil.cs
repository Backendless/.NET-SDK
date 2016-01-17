using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Weborb.ProxyGen.DynamicProxy.Generators.Emitters
{
	public static class StrongNameUtil
	{
        private static readonly Dictionary<Assembly, bool> signedAssemblyCache = new Dictionary<Assembly, bool>();
		private static readonly object lockObject = new object ();

		public static bool IsAssemblySigned(Assembly assembly)
		{
			lock (lockObject)
			{
				if (signedAssemblyCache.ContainsKey (assembly) == false)
				{
					bool isSigned = ContainsPublicKey (assembly);
					signedAssemblyCache.Add (assembly, isSigned);
				}
				return (bool) signedAssemblyCache[assembly];
			}
		}

		private static bool ContainsPublicKey (Assembly assembly)
		{
            try
            {
              AssemblyName assemblyName = new AssemblyName(assembly.FullName);
              byte[] key = assemblyName.GetPublicKey();
              return key != null && key.Length != 0;
            }
            catch( Exception )
            {
                return false;
            }
		}

		public static bool IsAnyTypeFromUnsignedAssembly (IEnumerable<Type> types)
		{
			foreach (Type t in types)
			{
				if (!IsAssemblySigned (t.Assembly))
					return true;
			}
			return false;
		}

		public static bool IsAnyTypeFromUnsignedAssembly (Type baseType, Type[] interfaces)
		{
			return !IsAssemblySigned (baseType.Assembly) || IsAnyTypeFromUnsignedAssembly (interfaces);
		}
	}
}