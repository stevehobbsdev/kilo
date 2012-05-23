using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Kilo
{
	public static partial class Collections
	{
		/// <summary>
		/// Gets the value at the specified key, converting it into the specified type. Supports nullable types.
		/// </summary>
		/// <typeparam name="T">The type to convert to</typeparam>
		/// <param name="nvc">The collection to interrogate</param>
		/// <param name="key">The key to fetch</param>
		public static T GetValueAsType<T>(this NameValueCollection nvc, string key)
		{
			Type conversionType = typeof(T);
			string value = null;

			if (!string.IsNullOrEmpty(nvc.Get(key)))
			{
				value = nvc.Get(key);
			}

			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				if (string.IsNullOrEmpty(value))
					return default(T);

				// Type is nullable but we actually have a value to convert. So, grab the type that the nullable is wrapping and
				// use that to convert instead.
				NullableConverter converter = new NullableConverter(conversionType);
				conversionType = converter.UnderlyingType;
			}

			return (T)Convert.ChangeType(value, conversionType);
		}
	}
}
