using System.Collections.Generic;

namespace Kilo.ObjectValidation
{
	public interface IValidationRunner
	{
		/// <summary>
		/// Validates the specified subject.
		/// </summary>
		/// <param name="subject">The subject.</param>
		IEnumerable<RuleViolation> Validate(object subject);
	}
}
