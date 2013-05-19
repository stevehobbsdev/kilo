using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.ObjectValidation
{
	public interface IViolationContainer
	{
		/// <summary>
		/// Adds the specified violation.
		/// </summary>
		/// <param name="violation">The violation.</param>
		void Add(RuleViolation violation);

		/// <summary>
		/// Gets a value indicating whether this instance has violations.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has violations; otherwise, <c>false</c>.
		/// </value>
		bool HasViolations { get; }

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		IEnumerable<RuleViolation> GetErrors(string key = null);
	}
}
