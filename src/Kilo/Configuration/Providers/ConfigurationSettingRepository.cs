using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Kilo.Configuration.Providers
{
	public class ConfigurationSettingRepository : ISettingRepository
	{
		public void WriteSetting(string name, object value, string group = null, string options = null)
		{			
		}

		public object ReadSetting(string name, string group = null, string options = null)
		{
			string keyName = GetKeyName(name, group);

			if (string.Equals(group, "ConnectionStrings", StringComparison.CurrentCultureIgnoreCase))
				return ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
			else
				return ConfigurationManager.AppSettings[keyName];
		}

		public bool HasSetting(string name, string group = null, string options = null)
		{
			string keyName = GetKeyName(name, group);

			if (string.Equals(group, "ConnectionStrings", StringComparison.CurrentCultureIgnoreCase))
				return ConfigurationManager.ConnectionStrings[keyName] != null;
			else
				return ConfigurationManager.AppSettings[keyName] != null;
		}

		public void OnSave()
		{
		}

		private static string GetKeyName(string name, string group)
		{
			string keyName = name;

			if (!string.IsNullOrWhiteSpace(group))
				keyName = string.Format("{0}.{1}", group, name);

			return keyName;
		}

		public void LoadSettings()
		{
		}

		public void PersistSettings()
		{
		}
	}
}
