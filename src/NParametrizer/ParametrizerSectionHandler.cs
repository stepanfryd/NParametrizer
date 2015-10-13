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

using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace NParametrizer
{
	/// <summary>
	///   Handles generic configuration section using XML serialization
	/// </summary>
	/// <typeparam name="T">The type of the section</typeparam>
	public class ParametrizerSectionHandler<T> : IConfigurationSectionHandler
	{
		#region Interface Implementations

		/// <summary>
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			var ser = new XmlSerializer(typeof (T));
			return ser.Deserialize(new StringReader(section.OuterXml));
		}

		#endregion
	}
}