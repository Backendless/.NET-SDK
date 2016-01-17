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
	using System.Collections;
    using System.Collections.Generic;

	public class ConfigurationCollection : List<IConfiguration>
	{
		public ConfigurationCollection()
		{
		}

		public ConfigurationCollection(ConfigurationCollection value)
		{
			AddRange(value);
		}

		public ConfigurationCollection(IConfiguration[] value)
		{
			AddRange(value);
		}

		public IConfiguration this[String name]
		{
			get
			{
				foreach(IConfiguration config in this)
				{
					if (name.Equals(config.Name))
					{
						return config;
					}
				}

				return null;
			}
		}


		public void AddRange(IConfiguration[] value)
		{
			foreach(IConfiguration configuration in value)
			{
				Add(configuration);
			}
		}

		public void AddRange(ConfigurationCollection value)
		{
			foreach(IConfiguration configuration in value)
			{
				Add(configuration);
			}
		}

	}
}