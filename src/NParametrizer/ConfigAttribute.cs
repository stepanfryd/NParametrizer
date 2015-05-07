using System;

namespace NParametrizer
{
    public class ConfigAttribute : Attribute
    {
        public ConfigAttribute(string key, ConfigType type)
        {
            KeyName = key;
            Type = type;
        }

        public ConfigType Type { get; set; }
        public string KeyName { get; set; }
    }
}