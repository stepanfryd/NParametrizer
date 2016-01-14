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
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace NParametrizer
{
	/// <summary>
	///   Base abstract clase for parameter store
	/// </summary>
	public abstract class ParametersBase
	{
		#region Public members

		/// <summary>
		///   Defined argument prefix. Default value is -- which means, that value parameters looks like --PARAMETER=
		/// </summary>
		protected string ValueArgumentPrefix { get; }

		#endregion

		#region Fields and constants

		private readonly List<string> _arguments = new List<string>();
		private readonly IDictionary<string, ParameterAttribute> _parameters;

		#endregion

		#region Constructors

		/// <summary>
		///   Base class constructor
		/// </summary>
		protected ParametersBase() : this(null, null)
		{
		}

		/// <summary>
		///   Class constructor
		/// </summary>
		/// <param name="args">Configuration arguments, mostly from comand line argument array</param>
		/// <param name="argPrefix">
		///   Defined argument prefix. Default value is -- which means, that value parameters looks like
		///   --PARAMETER=
		/// </param>
		protected ParametersBase(string[] args, string argPrefix = "--")
		{
			ValueArgumentPrefix = argPrefix ?? "";
			if (args != null)
			{
				_arguments.AddRange(args);
			}
			_parameters = new Dictionary<string, ParameterAttribute>();

			SetDefaults();
			ProcessParameters();
			ProcessArguments();
			// ReSharper disable once VirtualMemberCallInContructor
			ValidateArguments();
		}

		#endregion

		#region Private and protected

		/// <summary>
		///   Get configuration section by name with specified type
		/// </summary>
		/// <param name="sectionName"></param>
		/// <returns></returns>
		protected object GetSection(string sectionName)
		{
			return ConfigurationManager.GetSection(sectionName);
		}

		/// <summary>
		///   Get configuration section by name with specified type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sectionName"></param>
		/// <returns></returns>
		protected T GetSection<T>(string sectionName)
		{
			return (T) ConfigurationManager.GetSection(sectionName);
		}

		/// <summary>
		///   Get section by generic type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		protected T GetSection<T>()
		{
			return (T) ConfigurationManager.GetSection(typeof (T).Name);
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
									$"Config parameter [{parAttr.KeyName}] input is not in expected format", ex);
							}
						}
						else if (parAttr.Type == ConfigType.ConnectionString &&
						         ConfigurationManager.ConnectionStrings[parAttr.KeyName] != null)
						{
							prop.SetValue(this, ConfigurationManager.ConnectionStrings[parAttr.KeyName].ConnectionString,
								null);
						}
						else if (parAttr.Type == ConfigType.CustomSection)
						{
							prop.SetValue(this, GetSection(parAttr.KeyName), null);
						}
					}
				}
			}

			// overwrite by Environment variables
			foreach (var prop in GetType()
				.GetProperties()
				.Where(p => Attribute.IsDefined(p, typeof (EnvironmentAttribute))))
			{
				var parAttrs = prop.GetCustomAttributes(typeof (EnvironmentAttribute), true) as EnvironmentAttribute[];
				if (parAttrs != null && parAttrs.Length > 0)
				{
					var parAttr = parAttrs[0];
					if (!string.IsNullOrEmpty(parAttr.KeyName))
					{
						if (parAttr.Type != null)
						{
							SetPropertyValue(Environment.GetEnvironmentVariable(parAttr.KeyName, parAttr.Type.Value), prop);
						}
						else
						{
							var flow = new[]
							{EnvironmentVariableTarget.Process, EnvironmentVariableTarget.User, EnvironmentVariableTarget.Machine};

							foreach (var evt in flow)
							{
								var value = Environment.GetEnvironmentVariable(parAttr.KeyName, evt);
								if (!string.IsNullOrEmpty(value))
								{
									SetPropertyValue(value, prop);
									break;
								}
							}
						}
					}
				}
			}
		}

		private void SetPropertyValue(string value, PropertyInfo prop)
		{
			if (!string.IsNullOrEmpty(value))
			{
				try
				{
					prop.SetValue(this, Convert.ChangeType(value, prop.PropertyType), null);
				}
					// ReSharper disable once EmptyGeneralCatchClause
				catch
				{
				}
			}
		}

		/// <summary>
		///   Custom parameter validation
		/// </summary>
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
						if (pa.StartsWith(ValueArgumentPrefix))
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
			if (argument.StartsWith(ValueArgumentPrefix))
			{
				foreach (var par in _parameters)
				{
					if (argument.StartsWith(par.Key))
					{
						try
						{
							var strVal = argument.Substring(par.Key.Length);
							var pType = par.Value.BelongsTo.PropertyType;

							if (pType.IsEnum)
							{
								par.Value.BelongsTo.SetValue(this, Enum.Parse(pType, strVal, true), null);
							}
							else if (pType.IsGenericType && pType.GetGenericArguments().Length > 0 && pType.GetGenericArguments()[0].IsEnum)
							{
								par.Value.BelongsTo.SetValue(this, Enum.Parse(pType.GetGenericArguments()[0], strVal, true), null);
							}
							else
							{
								par.Value.BelongsTo.SetValue(this,
									Convert.ChangeType(strVal, pType), null);
							}
						}
						catch
						{
							// ignored
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
								!(bool) _parameters[argument].BelongsTo.GetValue(this, null), null);
						}
							// ReSharper disable once EmptyGeneralCatchClause
						catch
						{
						}
					}
				}
			}
		}

		#endregion
	}
}