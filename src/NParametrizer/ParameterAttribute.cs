using System;
using System.Reflection;

namespace NParametrizer
{
    /// <summary>
    ///     Class for paramter definition
    /// </summary>
    public class ParameterAttribute : Attribute
    {
        /// <summary>
        ///     Default Parameter constructor
        /// </summary>
        /// <param name="pars">Array of all possible arguments belongs to property</param>
        public ParameterAttribute(params string[] pars)
        {
            ParameterVariants = pars;
        }

        /// <summary>
        ///     Array of all possible arguments belongs to property
        /// </summary>
        public string[] ParameterVariants { get; private set; }

        /// <summary>
        ///     Parameter description. Can be used for usage generation eg. in case if no command argument is specified but program
        ///     needs some or required or in help/? command. :)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Property to parameter belongs
        /// </summary>
        public PropertyInfo BelongsTo { get; set; }
    }
}