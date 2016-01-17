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

	public class MutableConfiguration : AbstractConfiguration
	{
		public MutableConfiguration(String name) : this(name, null)
		{
		}

		public MutableConfiguration(String name, String value)
		{
			internalName = name;
			internalValue = value;
		}

		public static MutableConfiguration Create(string name)
		{
			return new MutableConfiguration(name);
		}

		public new string Value
		{
			set { internalValue = value; }
		}

		public MutableConfiguration Attribute(string name, string value)
		{
			Attributes[name] = value;
			return this;
		}

		public MutableConfiguration CreateChild(string name)
		{
			MutableConfiguration child = new MutableConfiguration(name);
			Children.Add(child);
			return child;
		}

		public MutableConfiguration CreateChild(string name, string value)
		{
			MutableConfiguration child = new MutableConfiguration(name, value);
			Children.Add(child);
			return child;
		}
	}
}