using System;

namespace Kilo.ObjectValidation
{
	public class RuleViolation
	{
		/// <summary>
		/// Gets or sets the error key
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the exception which caused the error, if any.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleViolation"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public RuleViolation(string key, string message, Exception exception = null)
		{
			this.Key = key;
			this.ErrorMessage = message;
			this.Exception = exception;
		}
	}
}
