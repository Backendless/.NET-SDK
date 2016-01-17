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

namespace Weborb.ProxyGen.Core.Configuration
{
	using System;
    using System.Collections.Generic;
	using System.Collections.Specialized;
    using System.Globalization;

	public abstract class AbstractConfiguration : IConfiguration
	{
		protected String internalName;
		protected String internalValue;
        private IDictionary<String, Object> attributes = new Dictionary<String, Object>();
		private ConfigurationCollection children = new ConfigurationCollection();

		public virtual String Name
		{
			get { return internalName; }
		}

		public virtual String Value
		{
			get { return internalValue; }
		}

		public virtual ConfigurationCollection Children
		{
			get { return children; }
		}

		public virtual IDictionary<String, Object> Attributes
		{
			get { return attributes; }
		}

		public virtual object GetValue(Type type, object defaultValue)
		{
			try
			{
				return Convert.ChangeType(Value, type, CultureInfo.InvariantCulture);
			}
			catch(Exception)
			{
				return defaultValue;
			}
		}
	}
}