using System;

namespace Kilo.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
	public class SettingAttribute : Attribute
	{
		/// <summary>
		/// The setting name
		/// </summary>
		public string Name;

		/// <summary>
		/// The setting group
		/// </summary>
		public string Group;

		/// <summary>
		/// The options
		/// </summary>
		public string Options;

        /// <summary>
        /// A flag indicating whether or not this property should be ignored
        /// </summary>
        public bool Ignore;

		/// <summary>
		/// Initializes a new instance of the <see cref="SettingAttribute"/> class.
		/// </summary>
		public SettingAttribute()
		{
		}
	}
}
