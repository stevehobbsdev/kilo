using System.Linq;
using System.Collections.Generic;
using Kilo.Configuration.Providers;

namespace Kilo.Configuration
{
	public class ConfigSettingsProxy : IConfigSettingsProxy
	{
        List<IConfigurationProvider> _providers;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigSettingsProxy"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public ConfigSettingsProxy(IConfigurationProvider provider, params IConfigurationProvider[] additionalProviders)
		{
            _providers = new List<IConfigurationProvider>(additionalProviders);
            _providers.Insert(0, provider);

            _providers.ForEach((p) =>
            {
                if (p is ILoadable && ((ILoadable)p).CanLoad)
                    ((ILoadable)p).Load();
            });
		}

		/// <summary>
		/// Gets the setting.
		/// </summary>
		/// <param name="key">The key.</param>
		public string GetSetting(string key)
		{
			foreach (var provider in _providers)
			{
				if (provider.HasSetting(key))
				{
					return provider.GetSetting(key);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <param name="connectionStringName">Name of the connection string.</param>
		public string GetConnectionString(string connectionStringName)
		{
			foreach (var provider in _providers)
			{
				if (provider.HasConnectionString(connectionStringName))
				{
					return provider.GetConnectionString(connectionStringName);
				}
			}

			return null;
		}
	}
}
