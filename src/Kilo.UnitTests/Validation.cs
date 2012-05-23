using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kilo.Data.Validation;
using Kilo.UnitTests.Fakes;

namespace Kilo.UnitTests
{
	[TestClass]
	public class Validation
	{
		[TestMethod]
		public void Valid_object_passes_validation()
		{
			IValidationRunner runner = new DataAnnotationsValidationRunner();

			ValidationTestObject obj = new ValidationTestObject()
			{
				Id = 1,
				Name = "Test Object",
				Email = "This is a test object"
			};

			IEnumerable<RuleViolation> errors = runner.Validate(obj);

			Assert.IsNotNull(errors);
			Assert.AreEqual(0, errors.Count());
		}

		[TestMethod]
		public void Invalid_object_fails_validation()
		{
			IValidationRunner runner = new DataAnnotationsValidationRunner();

			ValidationTestObject obj = new ValidationTestObject();

			IEnumerable<RuleViolation> errors = runner.Validate(obj);

			Assert.IsNotNull(errors);
			Assert.AreEqual(2, errors.Count());
		}

		[TestMethod]
		public void Object_fails_custom_validation()
		{
			IValidationRunner runner = new DataAnnotationsValidationRunner();

			CustomValidationTestObject obj = new CustomValidationTestObject();

			IEnumerable<RuleViolation> errors = runner.Validate(obj);

			Assert.IsNotNull(errors);
			Assert.AreEqual(1, errors.Count());
		}
	}
}
