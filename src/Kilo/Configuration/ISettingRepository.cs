
namespace Kilo.Configuration
{
	public interface ISettingRepository
	{
		/// <summary>
		/// Writes the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		void WriteSetting(string name, object value, string group = null, string options = null);

		/// <summary>
		/// Reads the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		object ReadSetting(string name, string group = null, string options = null);

		/// <summary>
		/// Determines whether the specified name has setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns>
		///   <c>true</c> if the specified name has setting; otherwise, <c>false</c>.
		/// </returns>
		bool HasSetting(string name, string group = null, string options = null);
	}
}
