using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kilo
{
	public static partial class Collections
	{
		/// <summary>
		/// Glues the contents of a name value collection together
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="glue">The glue.</param>
		public static string GlueContents(this NameValueCollection collection, string glue)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var key in collection)
			{
				sb.Append(string.Format("{0}={1}{2}", key, collection[key.ToString()], glue));
			}

			string result = sb.ToString();

			if (!string.IsNullOrEmpty(glue) & result.Length >= glue.Length)
			{
				result = result.Substring(0, result.Length - glue.Length);
			}

			return result;
		}

		/// <summary>
		/// Glues the contents of a name value collection together
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="glue">The glue.</param>
		public static string GlueContents<TKey, TValue>(this IDictionary<TKey, TValue> collection, string glue, Func<TValue, object> selector = null)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var entry in collection)
			{
				TValue value = entry.Value;
				object trueValue = value;

				if (selector != null)
					trueValue = selector(value);

				sb.Append(string.Format("{0}={1}{2}", entry.Key, trueValue, glue));
			}

			string result = sb.ToString();

			if (!string.IsNullOrEmpty(glue) & result.Length >= glue.Length)
			{
				result = result.Substring(0, result.Length - glue.Length);
			}

			return result;
		}

		/// <summary>
		/// Glues the contents together using a glue string
		/// </summary>
		/// <typeparam name="T">The collection type</typeparam>
		/// <param name="enumerable">The enumerable to glue</param>
		/// <param name="glue">The glue string</param>
		/// <param name="lastElementGlue">The last element glue string</param>
		/// <param name="selector">The selector.</param>
		public static string GlueContents<T>(this IEnumerable<T> enumerable, string glue, string lastElementGlue = null, Func<T, object> selector = null)
		{
			var sb = new StringBuilder();
			int maxIndex = enumerable.Count() - 1;
			var enumerator = enumerable.GetEnumerator();
			int index = 0;

			while ((enumerator.MoveNext()))
			{
				object text = null;
				T value = enumerator.Current;

				if (selector != null)
				{
					text = selector(value);
				}
				else
				{
					text = value.ToString();
				}

				if (index == (maxIndex - 1) && lastElementGlue != null)
				{
					sb.Append(string.Format("{0}{1}", text, lastElementGlue));
				}
				else if (index != maxIndex)
				{
					sb.Append(string.Format("{0}{1}", text, glue));
				}
				else
				{
					sb.Append(string.Format("{0}", text));
				}

				index += 1;

			}

			return sb.ToString();
		}

	}
}
