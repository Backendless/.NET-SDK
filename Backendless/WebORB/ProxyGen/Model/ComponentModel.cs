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

namespace Weborb.ProxyGen.Core
{
	using System;
	using System.Collections;
    using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using Weborb.ProxyGen.Core.Configuration;

	#region Enums

	public enum LifestyleType
	{
		Undefined,
		Singleton,
		Thread,
		Transient,
		Pooled,
		Custom,
		PerWebRequest
	}

	public enum PropertiesInspectionBehavior
	{
		Undefined,
		None,
		All,
		DeclaredOnly
	}

	#endregion

	[DebuggerDisplay("{Implementation} / {Service}")]
	public sealed class ComponentModel : GraphNode
	{
		public const string SkipRegistration = "skip.registration";

		#region Fields

		private String name;

		private Type service;

		private Type implementation;

		private IDictionary extended;

		private LifestyleType lifestyleType;

		private PropertiesInspectionBehavior inspectionBehavior;

		private Type customLifestyle;

		private Type customComponentActivator;

		private DependencyModelCollection dependencies;

		private ConstructorCandidateCollection constructors;

		private PropertySetCollection properties;

		//private MethodMetaModelCollection methodMetaModels;

		private LifecycleStepCollection lifecycleSteps;

		private ParameterModelCollection parameters;

		private IConfiguration configuration;

		private InterceptorReferenceCollection interceptors;

		private bool requiresGenericArguments;

		#endregion

		public ComponentModel(String name, Type service, Type implementation)
		{
			this.name = name;
			this.service = service;
			this.implementation = implementation;
			lifestyleType = LifestyleType.Undefined;
			inspectionBehavior = PropertiesInspectionBehavior.Undefined;
		}

		public String Name
		{
			get { return name; }
			set { name = value; }
		}

		public Type Service
		{
			get { return service; }
			set { service = value; }
		}

		public Type Implementation
		{
			get { return implementation; }
			set { implementation = value; }
		}

		public bool RequiresGenericArguments
		{
			get { return requiresGenericArguments; }
			set { requiresGenericArguments = value; }
		}

		public IDictionary ExtendedProperties
		{
			get
			{
				lock(this)
				{
					if (extended == null) extended = new Dictionary<Object,Object>();
				}
				return extended;
			}
			set { extended = value; }
		}

		public ConstructorCandidateCollection Constructors
		{
			get
			{
				lock(this)
				{
					if (constructors == null) constructors = new ConstructorCandidateCollection();
				}
				return constructors;
			}
		}

		public PropertySetCollection Properties
		{
			get
			{
				lock(this)
				{
					if (properties == null) properties = new PropertySetCollection();
				}
				return properties;
			}
		}

		public IConfiguration Configuration
		{
			get { return configuration; }
			set { configuration = value; }
		}

		public LifecycleStepCollection LifecycleSteps
		{
			get
			{
				lock(this)
				{
					if (lifecycleSteps == null) lifecycleSteps = new LifecycleStepCollection();
				}
				return lifecycleSteps;
			}
		}

		public LifestyleType LifestyleType
		{
			get { return lifestyleType; }
			set { lifestyleType = value; }
		}

		public PropertiesInspectionBehavior InspectionBehavior
		{
			get { return inspectionBehavior; }
			set { inspectionBehavior = value; }
		}

		public Type CustomLifestyle
		{
			get { return customLifestyle; }
			set { customLifestyle = value; }
		}

		public Type CustomComponentActivator
		{
			get { return customComponentActivator; }
			set { customComponentActivator = value; }
		}

		public InterceptorReferenceCollection Interceptors
		{
			get
			{
				lock(this)
				{
					if (interceptors == null) interceptors = new InterceptorReferenceCollection();
				}
				return interceptors;
			}
		}

		public ParameterModelCollection Parameters
		{
			get
			{
				lock(this)
				{
					if (parameters == null) parameters = new ParameterModelCollection();
				}
				return parameters;
			}
		}

		public DependencyModelCollection Dependencies
		{
			get
			{
				lock(this)
				{
					if (dependencies == null) dependencies = new DependencyModelCollection();
				}
				return dependencies;
			}
		}
	}
}