
namespace Kilo.Configuration
{
	public interface IConfigSettingsProxy
	{
		/// <summary>
		/// Gets the connection string with the specified name
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		string GetConnectionString(string connectionStringName);

		/// <summary>
		/// Gets the setting with the specified key
		/// </summary>
		/// <param name="key">The key.</param>
		string GetSetting(string key);
	}
}
