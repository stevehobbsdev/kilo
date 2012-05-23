using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Kilo.Data.Validation
{
	internal static class ValidationUtil
	{
		/// <summary>
		/// Determines a validation key for the specified object.
		/// </summary>
		/// <param name="attrib">The attrib.</param>
		/// <param name="target">The target.</param>
		public static string GetValidationKey(this ValidationAttribute attrib, object target)
		{
			string validatioKey = null;

			if (target is PropertyInfo)
				validatioKey = (target as PropertyInfo).Name;
			else if (target is FieldInfo)
				validatioKey = (target as FieldInfo).Name;
			else if (target is Type)
				validatioKey = (target as Type).Name;
			else
			{
				Type targetType = target.GetType();
				validatioKey = targetType.Name;
			}

			return validatioKey;
		}
	}
}
