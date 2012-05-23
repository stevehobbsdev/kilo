using System.Configuration;

namespace Kilo.Configuration.Providers
{
	public class ConfigFileProvider : IConfigurationProvider
	{
		/// <summary>
		/// Gets the setting value for the specified key
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public string GetSetting(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		/// <summary>
		/// Gets the connection string for the specified connection string name
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		/// <returns></returns>
		public string GetConnectionString(string connectionStringName)
		{
			return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}

		/// <summary>
		/// Determines whether the specified key has setting.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public bool HasSetting(string key)
		{
			return GetSetting(key) != null;
		}

		/// <summary>
		/// Determines whether the configuration knows about the specified connection string
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		/// <returns></returns>
		public bool HasConnectionString(string connectionStringName)
		{
			return ConfigurationManager.ConnectionStrings[connectionStringName] != null;
		}
	}
}
