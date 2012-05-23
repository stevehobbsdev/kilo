
namespace Kilo.Configuration.Providers
{
	public interface IConfigurationProvider
	{
		/// <summary>
		/// Gets the setting value for the specified key
		/// </summary>
		/// <param name="key">The key.</param>
		string GetSetting(string key);

		/// <summary>
		/// Gets the connection string for the specified connection string name
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		string GetConnectionString(string connectionStringName);

		/// <summary>
		/// Determines whether the specified key has setting.
		/// </summary>
		/// <param name="key">The key.</param>
		bool HasSetting(string key);

		/// <summary>
		/// Determines whether [has connection string] [the specified connection string name].
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		bool HasConnectionString(string connectionStringName);
	}
}
