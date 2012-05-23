using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.Data.Caching
{
	public interface ICacheProvider
	{
		/// <summary>
		/// Gets an object from the cache with the specified key
		/// </summary>
		/// <param name="key">The key.</param>
		object Get(string key);

		/// <summary>
		/// Sets the value into the cache using the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiry">The expiry.</param>
		void Set(string key, object value, DateTime? expiry = null);

		/// <summary>
		/// Determines whether the specified cache item is set based on its key
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>True if the object has been set, false if not.</returns>
		bool IsSet(string key);
	}
}
