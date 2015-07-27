using System;

namespace NParametrizer
{
	/// <summary>
	///     Configuration paremeter
	/// </summary>
	public class ConfigAttribute : Attribute
	{
		/// <summary>
		///     Configuration type
		/// </summary>
		public ConfigType Type { get; set; }

		/// <summary>
		///     Key name in configuration dictioary
		/// </summary>
		public string KeyName { get; set; }

		/// <summary>
		///     Default constructor of configuration parameter
		/// </summary>
		/// <param name="key">Key name in configuration dictioary</param>
		/// <param name="type">Configuration type</param>
		public ConfigAttribute(string key, ConfigType type)
		{
			KeyName = key;
			Type = type;
		}

		/// <summary>
		///
		/// </summary>
		public ConfigAttribute(string configSection)
		{
			KeyName = configSection;
			Type = ConfigType.CustomSection;
		}
	}
}