using System;
using System.Reflection;

namespace NParametrizer
{
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(params string[] pars)
        {
            ParameterVariants = pars;
        }

        public string[] ParameterVariants { get; private set; }
        public string Description { get; set; }
        public PropertyInfo BelongsTo { get; set; }
    }
}