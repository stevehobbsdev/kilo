using System.Collections.Generic;
using System.Reflection;

namespace Kilo.Reflection
{
	public class DictionaryBuilder
	{
		/// <summary>
		/// Creates a dictionary from an object, converting public properties and their values into the
		/// key/value pairs of the dictionary. Useful for use with anonymous types.
		/// </summary>
		/// <param name="input">The input object to build the dictionary from</param>
		public static IDictionary<string, object> FromObject(object input)
		{
			var dict = new Dictionary<string, object>();

			PropertyInfo[] properties = input.GetType().GetProperties();

			foreach (var prop in properties)
			{
				dict.Add(prop.Name, prop.GetValue(input, null));
			}

			return dict;
		}
	}
}
