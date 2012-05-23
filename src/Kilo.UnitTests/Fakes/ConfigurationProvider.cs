using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kilo.Configuration;
using System.Collections.Specialized;
using Kilo.Configuration.Providers;

namespace Kilo.UnitTests.Fakes
{
	public class ConfigurationProvider : IConfigurationProvider
	{
		NameValueCollection settings;
		NameValueCollection connectionStrings;

		public ConfigurationProvider()
		{
			settings = new NameValueCollection();
			settings["testSetting"] = "Setting Value";

			connectionStrings = new NameValueCollection();
			connectionStrings["TestConnection"] = "Test Connection String";
		}

		public ConfigurationProvider(NameValueCollection appSettings, NameValueCollection connectionStrings)
		{
			this.settings = appSettings;
			this.connectionStrings = connectionStrings;
		}

		public string GetSetting(string key)
		{
			return settings[key];
		}

		public string GetConnectionString(string connectionStringName)
		{
			return connectionStrings[connectionStringName];
		}

		public bool HasSetting(string key)
		{
			return this.settings[key] != null;
		}

		public bool HasConnectionString(string connectionStringName)
		{
			return this.connectionStrings[connectionStringName] != null;
		}
	}
}
