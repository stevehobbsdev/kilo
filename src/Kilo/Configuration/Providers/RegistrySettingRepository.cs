using Microsoft.Win32;

namespace Kilo.Configuration.Providers
{
	public class RegistrySettingRepository : ISettingRepository
	{
		private RegistryKey _root;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrySettingRepository"/> class.
		/// </summary>
		/// <param name="rootKey">The root key.</param>
		public RegistrySettingRepository(string rootKey)
		{
			_root = Registry.CurrentUser.CreateSubKey(rootKey);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrySettingRepository"/> class.
		/// </summary>
		/// <param name="rootKey">The root key.</param>
		public RegistrySettingRepository(RegistryKey rootKey)
		{
			_root = rootKey;
		}

		/// <summary>
		/// Writes the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		public void WriteSetting(string name, object value, string group = null, string options = null)
		{
			var rootKey = _root;

			if (!string.IsNullOrWhiteSpace(group))
			{
				rootKey = _root.CreateSubKey(group);
			}

			rootKey.SetValue(name, value);
		}

		/// <summary>
		/// Reads the setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		public object ReadSetting(string name, string group = null, string options = null)
		{
			var rootKey = _root;

			if (!string.IsNullOrWhiteSpace(group))
			{
				rootKey = _root.CreateSubKey(group);
			}

			return rootKey.GetValue(name);
		}

		/// <summary>
		/// Determines whether the specified name has setting.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="group">The group.</param>
		/// <param name="options">The options.</param>
		/// <returns>
		///   <c>true</c> if the specified name has setting; otherwise, <c>false</c>.
		/// </returns>
		public bool HasSetting(string name, string group = null, string options = null)
		{
			var rootKey = _root;

			if (!string.IsNullOrWhiteSpace(group))
			{
				rootKey = _root.CreateSubKey(group);
			}

			object value = rootKey.GetValue(name, null);

			return value != null;
		}

	}
}
