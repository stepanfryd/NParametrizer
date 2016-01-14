/*
Copyright 2015 Stepan Fryd (stepan.fryd@gmail.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/
using System;

namespace NParametrizer
{
	/// <summary>
	///     Environment value paremeter
	/// </summary>
	public class EnvironmentAttribute : Attribute
	{
		/// <summary>
		///     EnvironmentVariableTarget process/user/machine, If null parameter value then search of value goes Process => User => Machine
		/// </summary>
		public EnvironmentVariableTarget? Type { get; set; }

		/// <summary>
		///     Key name in configuration dictioary
		/// </summary>
		public string KeyName { get; set; }

		/// <summary>
		///     Default constructor of configuration parameter
		/// </summary>
		/// <param name="key">Key name in configuration dictioary</param>
		/// <param name="type">EnvironmentVariableTarget process/user/machine</param>
		public EnvironmentAttribute(string key, EnvironmentVariableTarget type)
		{
			KeyName = key;
			Type = type;
		}

		/// <summary>
		///     Default constructor of configuration parameter
		/// </summary>
		/// <param name="key">Key name in configuration dictioary</param>
		public EnvironmentAttribute(string key)
		{
			KeyName = key;
		}
	}
}