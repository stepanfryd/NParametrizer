using System;

namespace NParametrizer
{
    /// <summary>
    ///     Configuration paremeter
    /// </summary>
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        ///     Default instance of configuration parameter
        /// </summary>
        /// <param name="key">Key name in configuration dictioary</param>
        /// <param name="type">Configuration type</param>
        public ConfigAttribute(string key, ConfigType type)
        {
            KeyName = key;
            Type = type;
        }

        /// <summary>
        ///     Configuration type
        /// </summary>
        public ConfigType Type { get; set; }

        /// <summary>
        ///     Key name in configuration dictioary
        /// </summary>
        public string KeyName { get; set; }
    }
}