// Copyright 2004-2007 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Weborb.ProxyGen.DynamicProxy
{
	using System;
	using System.Collections;
    using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Resources;
	using System.Threading;
	using Weborb.ProxyGen.DynamicProxy.Generators;

	public class ModuleScope
	{
		public static readonly String DEFAULT_FILE_NAME = "CastleDynProxy2.dll";

		public static readonly String DEFAULT_ASSEMBLY_NAME = "DynamicProxyGenAssembly2";

		// Avoid leaks caused by non disposal of generated types.
		private ModuleBuilder moduleBuilderWithStrongName = null;
		private ModuleBuilder moduleBuilder = null;

		// The names to use for the generated assemblies and the paths (including the names) of their manifest modules
		private string strongAssemblyName;
		private string weakAssemblyName;
		private string strongModulePath;
		private string weakModulePath;

		// Keeps track of generated types
        private IDictionary<CacheKey, Type> typeCache = new Dictionary<CacheKey, Type>();

		// Users of ModuleScope should use this lock when accessing the cache
		//private ReaderWriterLock readerWriterLock = new ReaderWriterLock ();

		// Used to lock the module builder creation
		private object _lockobj = new object();

		// Specified whether the generated assemblies are intended to be saved
		private bool savePhysicalAssembly;

		public ModuleScope() : this (false)
		{
		}

		public ModuleScope (bool savePhysicalAssembly)
			: this (savePhysicalAssembly, DEFAULT_ASSEMBLY_NAME, DEFAULT_FILE_NAME, DEFAULT_ASSEMBLY_NAME, DEFAULT_FILE_NAME)
		{
		}

		public ModuleScope (bool savePhysicalAssembly, string strongAssemblyName, string strongModulePath, string weakAssemblyName, string weakModulePath)
		{
			this.savePhysicalAssembly = savePhysicalAssembly;
			this.strongAssemblyName = strongAssemblyName;
			this.strongModulePath = strongModulePath;
			this.weakAssemblyName = weakAssemblyName;
			this.weakModulePath = weakModulePath;
		}

		public Type GetFromCache (CacheKey key)
		{
			// no lock needed, typeCache is synchronized
            Type t = null;
            typeCache.TryGetValue( key, out t );
            return t;
		}

		public void RegisterInCache (CacheKey key, Type type)
		{
			// no lock needed, typeCache is synchronized
			typeCache[key] = type;
		}

		public static byte[] GetKeyPair ()
		{
			byte[] keyPair;

			using (Stream stream = Assembly.GetExecutingAssembly ().GetManifestResourceStream ("Castle.DynamicProxy.DynProxy.snk"))
			{
				if (stream == null)
					throw new MissingManifestResourceException (
						"Should have a Castle.DynamicProxy.DynProxy.snk as an embedded resource, so Dynamic Proxy could sign generated assembly");

				int length = (int) stream.Length;
				keyPair = new byte[length];
				stream.Read (keyPair, 0, length);
			}

			return keyPair;
		}

		public ModuleBuilder StrongNamedModule
		{
			get
			{
				lock (_lockobj)
				{
					return moduleBuilderWithStrongName;
				}
			}
		}

		public string StrongNamedModuleName
		{
			get
			{
				return Path.GetFileName (strongModulePath);
			}
		}

		public string StrongNamedModuleDirectory
		{
			get
			{
				string directory = Path.GetDirectoryName (strongModulePath);
				if (directory == "")
					return null;
				else
					return directory;
			}
		}

		public ModuleBuilder WeakNamedModule
		{
			get
			{
				lock (_lockobj)
				{
					return moduleBuilder;
				}
			}
		}

		public string WeakNamedModuleName
		{
			get
			{
				return Path.GetFileName (weakModulePath);
			}
		}

		public string WeakNamedModuleDirectory
		{
			get
			{
				string directory = Path.GetDirectoryName (weakModulePath);
				if (directory == "")
					return null;
				else
					return directory;
			}
		}

		public ModuleBuilder ObtainDynamicModule (bool isStrongNamed)
		{
			lock (_lockobj)
			{
				if (isStrongNamed)
					return ObtainDynamicModuleWithStrongName ();
				else
					return ObtainDynamicModuleWithWeakName ();
			}
		}

		public ModuleBuilder ObtainDynamicModuleWithStrongName()
		{
			lock (_lockobj)
			{
				if (moduleBuilderWithStrongName == null)
				{
					moduleBuilderWithStrongName = CreateModule (true);
				}
				return moduleBuilderWithStrongName;
			}
		}

		public ModuleBuilder ObtainDynamicModuleWithWeakName ()
		{
			lock (_lockobj)
			{
				if (moduleBuilder == null)
				{
					moduleBuilder = CreateModule (false);
				}
				return moduleBuilder;
			}
		}

		private ModuleBuilder CreateModule(bool signStrongName)
		{
			AssemblyName assemblyName = GetAssemblyName(signStrongName);

			string moduleName = signStrongName ? StrongNamedModuleName : WeakNamedModuleName;
			string moduleDirectory = signStrongName ? StrongNamedModuleDirectory : WeakNamedModuleDirectory;

            /*
			if (savePhysicalAssembly)
			{
				AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
					assemblyName, AssemblyBuilderAccess.RunAndSave, moduleDirectory);

				return assemblyBuilder.DefineDynamicModule(moduleName, moduleName, true);
			}
			else*/
			{
				AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
					assemblyName,
					AssemblyBuilderAccess.Run);

				return assemblyBuilder.DefineDynamicModule(moduleName, true);
			}
		}

		private AssemblyName GetAssemblyName (bool signStrongName)
		{
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = signStrongName ? strongAssemblyName : weakAssemblyName;

			if (signStrongName)
			{
				byte[] keyPairStream = GetKeyPair();

                /*
				if (keyPairStream != null)
				{
					assemblyName..KeyPair = new StrongNameKeyPair(keyPairStream);
				}*/
			}
			return assemblyName;
		}

		public void SaveAssembly ()
		{
			if (!savePhysicalAssembly)
				return;

			if (StrongNamedModule != null && WeakNamedModule != null)
					throw new InvalidOperationException ("Both a strong-named and a weak-named assembly have been generated.");
			else if (StrongNamedModule != null)
				SaveAssembly (true);
			else if (WeakNamedModule != null)
				SaveAssembly (false);
			else
				throw new InvalidOperationException ("No assembly has been generated.");
		}

		public void SaveAssembly (bool strongNamed)
		{
            /*
			if (!savePhysicalAssembly)
				return;

			AssemblyBuilder assemblyBuilder;
			string assemblyFileName;
      string assemblyFilePath;

			if (strongNamed)
			{
				if (StrongNamedModule == null)
					throw new InvalidOperationException ("No strong-named assembly has been generated.");
				else
				{
					assemblyBuilder = (AssemblyBuilder) StrongNamedModule.Assembly;
					assemblyFileName = StrongNamedModuleName;
          assemblyFilePath = StrongNamedModule.FullyQualifiedName;
				}
			}
			else
			{
				if (WeakNamedModule == null)
					throw new InvalidOperationException ("No weak-named assembly has been generated.");
				else
				{
					assemblyBuilder = (AssemblyBuilder) WeakNamedModule.Assembly;
					assemblyFileName = WeakNamedModuleName;
          assemblyFilePath = WeakNamedModule.FullyQualifiedName;
				}
			}

			if (File.Exists (assemblyFilePath))
			{
        File.Delete (assemblyFilePath);
			}

			assemblyBuilder.Save (assemblyFileName);
             */
        }
             
	}
}