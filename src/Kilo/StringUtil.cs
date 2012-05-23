using System;
using System.Text.RegularExpressions;

namespace Kilo
{
	public static class StringUtil
	{
		/// <summary>
		/// Strips the HTML from the input string
		/// </summary>
		/// <param name="input">The input.</param>
		public static string StripHtml(string input)
		{
			return Regex.Replace(input, @"</?[^>]+/?>", string.Empty);
		}
	}
}
