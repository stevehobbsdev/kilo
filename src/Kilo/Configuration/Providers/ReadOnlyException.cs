using System;

namespace Kilo.Configuration.Providers
{
	public class ReadOnlyException : Exception
	{
		public ReadOnlyException()
			: base("The operation you are trying to perform is read only")
		{
		}

		public ReadOnlyException(string message)
			: base(message)
		{
		}
	}
}
