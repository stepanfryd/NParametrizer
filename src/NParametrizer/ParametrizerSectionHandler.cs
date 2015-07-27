using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NParametrizer
{
	/// <summary>
	/// Handles generic configuration section using XML serialization
	/// </summary>
	/// <typeparam name="T">The type of the section</typeparam>
	public class ParametrizerSectionHandler<T> : IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, XmlNode section)
		{
			var ser = new XmlSerializer(typeof(T));
			return ser.Deserialize(new StringReader(section.OuterXml));
		}
	}
}
