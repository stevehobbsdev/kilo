using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kilo.UnitTests.Fakes;
using Kilo.Configuration;
using System.Collections.Specialized;

namespace Kilo.UnitTests
{
	/// <summary>
	/// Summary description for Configuration
	/// </summary>
	[TestClass]
	public class Configuration
	{
		[TestMethod]
		public void Can_get_app_setting_from_proxy()
		{
			var proxy = new ConfigSettingsProxy(new ConfigurationProvider());

			string setting = proxy.GetSetting("testSetting");

			Assert.AreEqual("Setting Value", setting);
		}

		[TestMethod]
		public void Invalid_setting_returns_null()
		{
			var proxy = new ConfigSettingsProxy(new ConfigurationProvider());

			string setting = proxy.GetSetting("some bogus setting");

			Assert.IsNull(setting);
		}

		[TestMethod]
		public void Can_get_connection_string()
		{
			var proxy = new ConfigSettingsProxy(new ConfigurationProvider());

			string connString = proxy.GetConnectionString("TestConnection");

			Assert.AreEqual("Test Connection String", connString);
		}

		[TestMethod]
		public void Invalid_connection_string_returns_null()
		{
			var proxy = new ConfigSettingsProxy(new ConfigurationProvider());

			string connString = proxy.GetConnectionString("Bogus connection string");

			Assert.IsNull(connString);
		}

		[TestMethod]
		public void App_settings_can_take_precedence()
		{
			var settings1 = new NameValueCollection();
			settings1["value1"] = "This is value 1";
			
			var settings2 = new NameValueCollection();
			settings2["value1"] = "This is value 2";

			var provider1 = new ConfigurationProvider(settings1, null);
			var provider2 = new ConfigurationProvider(settings2, null);

			var proxy = new ConfigSettingsProxy(provider1, provider2);

			string result = proxy.GetSetting("value1");

			Assert.AreEqual("This is value 1", result);
		}

		[TestMethod]
		public void App_settings_can_fall_back()
		{
			var settings1 = new NameValueCollection();
			settings1["value1"] = "This is value 1";

			var settings2 = new NameValueCollection();
			settings2["someOtherValue"] = "This is value 2";

			var provider1 = new ConfigurationProvider(settings1, null);
			var provider2 = new ConfigurationProvider(settings2, null);

			var proxy = new ConfigSettingsProxy(provider1, provider2);

			string result = proxy.GetSetting("someOtherValue");

			Assert.AreEqual("This is value 2", result);
		}
	}
}
