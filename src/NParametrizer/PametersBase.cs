using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace NParametrizer
{
    public abstract class ParametersBase
    {
        private readonly string[] _arguments;
        private readonly IDictionary<string, ParameterAttribute> _parameters;
        private string _valueArgumentPrefix;

        protected ParametersBase(string[] args, string argPrefix = "--")
        {
            _valueArgumentPrefix = argPrefix ?? "" ;


            _parameters = new Dictionary<string, ParameterAttribute>();
            _arguments = args;

            SetDefaults();
            ProcessParameters();
            ProcessArguments();
            ValidateArguments();
        }

        protected string ValueArgumentPrefix
        {
            get { return _valueArgumentPrefix; }
            set { _valueArgumentPrefix = value; }
        }

        private void SetDefaults()
        {
            // set defaults from attributes
            foreach (var prop in GetType()
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof (DefaultValueAttribute))))
            {
                var parAttr = prop.GetCustomAttributes(typeof (DefaultValueAttribute), true) as DefaultValueAttribute[];
                if (parAttr != null && parAttr.Length > 0)
                {
                    if (parAttr[0].Value != null)
                    {
                        prop.SetValue(this, parAttr[0].Value, null);
                    }
                }
            }

            // overwrite by app.config
            foreach (var prop in GetType()
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof (ConfigAttribute))))
            {
                var parAttrs = prop.GetCustomAttributes(typeof (ConfigAttribute), true) as ConfigAttribute[];
                if (parAttrs != null && parAttrs.Length > 0)
                {
                    var parAttr = parAttrs[0];
                    if (!string.IsNullOrEmpty(parAttr.KeyName))
                    {
                        if (parAttr.Type == ConfigType.AppSettings &&
                            ConfigurationManager.AppSettings[parAttr.KeyName] != null)
                        {
                            var strVal = ConfigurationManager.AppSettings[parAttr.KeyName];

                            try
                            {
                                prop.SetValue(this, Convert.ChangeType(strVal, prop.PropertyType), null);
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(
                                    string.Format("Config parameter [{0}] input is not in expected format",
                                        parAttr.KeyName), ex);
                            }
                        }
                        else if (parAttr.Type == ConfigType.ConnectionString &&
                                 ConfigurationManager.ConnectionStrings[parAttr.KeyName] != null)
                        {
                            prop.SetValue(this, ConfigurationManager.ConnectionStrings[parAttr.KeyName].ConnectionString,
                                null);
                        }
                    }
                }
            }
        }

        protected abstract void ValidateArguments();

        private void ProcessParameters()
        {
            foreach (var prop in GetType()
                .GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof (ParameterAttribute))))
            {
                var attrs = prop.GetCustomAttributes(typeof (ParameterAttribute), true) as ParameterAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    var parAttr = attrs.First();
                    parAttr.BelongsTo = prop;
                    foreach (var pa in parAttr.ParameterVariants)
                    {
                        if (pa.StartsWith("-"))
                        {
                            _parameters.Add(pa + "=", parAttr);
                        }
                        else
                        {
                            _parameters.Add(pa, parAttr);
                        }
                    }
                }
            }
        }

        private void ProcessArguments()
        {
            foreach (var argument in _arguments)
            {
                SetParam(argument);
            }
        }

        private void SetParam(string argument)
        {
            if (argument.StartsWith(_valueArgumentPrefix))
            {
                foreach (var par in _parameters)
                {
                    if (argument.StartsWith(par.Key))
                    {
                        try
                        {
                            var strVal = argument.Substring(par.Key.Length);
                            par.Value.BelongsTo.SetValue(this,
                                Convert.ChangeType(strVal, par.Value.BelongsTo.PropertyType), null);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                if (_parameters.ContainsKey(argument))
                {
                    if (_parameters[argument] != null)
                    {
                        try
                        {
                            // expecting, that non-dash value are boolean and if is set, than set to true.
                            _parameters[argument].BelongsTo.SetValue(this,
                                !((bool) _parameters[argument].BelongsTo.GetValue(this, null)), null);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}