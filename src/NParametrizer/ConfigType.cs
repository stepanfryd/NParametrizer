namespace NParametrizer
{
    /// <summary>
    /// Configuration parameter location
    /// </summary>
    public enum ConfigType
    {
        /// <summary>
        /// AppSettings collection key
        /// </summary>
        AppSettings,

        /// <summary>
        /// Connection strings collection key
        /// </summary>
        ConnectionString,

        /// <summary>
        /// Configuration is defined in custom configuration section
        /// </summary>
        CustomSection
    }
}