using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Kilo.UnitTests.Fakes
{
	public class ValidationTestObject
	{
		public int Id { get; set; }

		[Required, StringLength(20)]
		public string Name { get; set; }

		[Required]
		public string Email { get; set; }
	}

	[CustomValidation(typeof(CustomValidationTestObject), "PerformCustomValidation")]
	public class CustomValidationTestObject
	{
		public int Id { get; set; }

		public static ValidationResult PerformCustomValidation(CustomValidationTestObject obj, ValidationContext context)
		{
			if (obj.Id == 0)
				return new ValidationResult("Id cannot be 0");
			else
				return ValidationResult.Success;
		}
	}
}
