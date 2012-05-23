using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kilo
{
	public static partial class Collections
	{
		public static IDictionary<string, object> ConvertObjectToDictionary(object obj)
		{
			var dict = new Dictionary<string, object>();

			PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance);

			foreach (var prop in properties)
			{
				dict.Add(prop.Name, prop.GetValue(obj, null));
			}

			return dict;
		}
	}
}
