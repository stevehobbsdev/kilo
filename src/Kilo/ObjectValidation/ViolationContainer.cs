using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.ObjectValidation
{
	public class ViolationContainer : IViolationContainer
	{
		private Dictionary<string, List<RuleViolation>> _violations = null;

		/// <summary>
		/// Gets a value indicating whether this instance has violations.
		/// </summary>
		public bool HasViolations
		{
			get { return _violations.Values.Any(); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViolationContainer"/> class.
		/// </summary>
		public ViolationContainer()
		{
			_violations = null;
		}

		/// <summary>
		/// Adds the specified violation.
		/// </summary>
		/// <param name="violation">The violation.</param>
		public void Add(RuleViolation violation)
		{
			if (!_violations.ContainsKey(violation.Key))
			{
				_violations.Add(violation.Key, new List<RuleViolation>());
			}

			_violations[violation.Key].Add(violation);
		}

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public IEnumerable<RuleViolation> GetErrors(string key = null)
		{
			if (!string.IsNullOrWhiteSpace(key))
			{
				return _violations[key];
			}
			else
			{
				return _violations.Values.SelectMany(v => v);
			}
		}
	}
}
