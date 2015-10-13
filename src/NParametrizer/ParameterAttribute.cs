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
using System.Reflection;

namespace NParametrizer
{
	/// <summary>
	///   Class for paramter definition
	/// </summary>
	public class ParameterAttribute : Attribute
	{
		#region Public members

		/// <summary>
		///   Array of all possible arguments belongs to property
		/// </summary>
		public string[] ParameterVariants { get; private set; }

		/// <summary>
		///   Parameter description. Can be used for usage generation eg. in case if no command argument is specified but program
		///   needs some or required or in help/? command. :)
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///   Property to parameter belongs
		/// </summary>
		public PropertyInfo BelongsTo { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		///   Default Parameter constructor
		/// </summary>
		/// <param name="pars">Array of all possible arguments belongs to property</param>
		public ParameterAttribute(params string[] pars)
		{
			ParameterVariants = pars;
		}

		#endregion
	}
}