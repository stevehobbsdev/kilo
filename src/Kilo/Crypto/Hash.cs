using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Kilo.Crypto
{
	public class Hash
	{
		/// <summary>
		/// Calculates an MD5 hash based on the input string. If a salt string is provided, it is appended to the input prior to hashing.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <param name="salt">The salt.</param>
		public static string CalculateMD5(string input, string salt = "")
		{
			if (string.IsNullOrWhiteSpace(input))
				throw new ArgumentNullException("input");

			ASCIIEncoding encoding = new ASCIIEncoding();

			string stringToHash = input;

			if (!string.IsNullOrWhiteSpace(salt))
				stringToHash += salt;

			MD5 md5 = MD5CryptoServiceProvider.Create();
			byte[] hashedBytes = md5.ComputeHash(encoding.GetBytes(stringToHash));
			string output = string.Empty;
			int i = 0;

			while (i <= (hashedBytes.Length - 1))
			{
				output = (output + string.Format("{0:x2}", hashedBytes[i]));
				i++;
			}

			return output;
		}
	}
}
