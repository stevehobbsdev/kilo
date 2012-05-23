using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.Data.Validation
{
	public class ValidationException : ApplicationException
	{
		/// <summary>
		/// Gets the rule violations.
		/// </summary>
		public IEnumerable<RuleViolation> RuleViolations { get; private set; }

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		public override string Message { get { return BuildExceptionMessage(this.RuleViolations); } }

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="violations">The violations.</param>
		public ValidationException(IEnumerable<RuleViolation> violations)
		{
			if (violations == null) throw new ArgumentNullException("violations");

			this.RuleViolations = violations;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="message">The message.</param>
		public ValidationException(string key, string message)
			: this(new RuleViolation(key, message))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationException"/> class.
		/// </summary>
		/// <param name="violation">The violation.</param>
		public ValidationException(RuleViolation violation)
		{
			if (violation == null) throw new ArgumentNullException("violations");

			this.RuleViolations = new RuleViolation[] { violation };
		}

		/// <summary>
		/// Builds the exception message using the supplied set of rule violations.
		/// </summary>
		/// <param name="violations">The violations.</param>
		private static string BuildExceptionMessage(IEnumerable<RuleViolation> violations)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("One or more violation errors have occured");

			foreach (var violation in violations)
			{
				builder.AppendFormat("{0}: {1}", violation.Key, violation.ErrorMessage);
				builder.AppendLine();
			}

			string result = builder.ToString();

			return result;
		}

	}
}
