using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo
{
	public static partial class Collections
	{
		/// <summary>
		/// Peforms a for-each loop on the enumerable and executes the supplied action on each iteration
		/// </summary>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="action">The action.</param>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}
