using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Kilo.ObjectValidation
{
	public class DataAnnotationsValidationRunner : IValidationRunner
	{

		#region Symbol helper class
		class Symbol
		{
			public string Name { get; set; }
			
			public IEnumerable<ValidationAttribute> Attributes { get; set; }
			
			public DisplayAttribute DisplayAttribute { get; set; }
			
			public MemberInfo MemberInfo { get; set; }

			public string GetDisplayName()
			{
				if (DisplayAttribute == null)
					return this.Name;
				else
					return DisplayAttribute.Name;
			}

			public object GetValue(object instance, params object [] args)
			{
				if (this.MemberInfo != null)
				{
					if (this.MemberInfo is PropertyInfo)
						return (this.MemberInfo as PropertyInfo).GetValue(instance, args);
					else if (this.MemberInfo is FieldInfo)
						return (this.MemberInfo as FieldInfo).GetValue(instance);
					else
						return null;
				}
				else
				{
					throw new InvalidOperationException(string.Format("Property name '{0}' not found", this.Name));
				}
			}

		}
		#endregion

		public IEnumerable<RuleViolation> Validate(object subject)
		{
			var errors = new List<RuleViolation>();

			Type modelType = subject.GetType();
			Type metaType = GetMetadataType(modelType);

			var modelSymbols = GetPropertiesAndFields(modelType).ToDictionary(m => m.Name);
			var metaSymbols = GetPropertiesAndFields(metaType);

			var symbols = (from s in metaSymbols
						  select new Symbol()
						  {
							  Name = s.Name,
							  Attributes = s.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>(),
							  DisplayAttribute = s.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().SingleOrDefault(),
							  MemberInfo = modelSymbols.ContainsKey(s.Name) ? modelSymbols[s.Name] : null
						  }).ToList();

			var query = (from s in symbols
						 join modelProp in modelSymbols.Values on s.Name equals modelProp.Name
						 from attrib in s.Attributes
						 where !attrib.IsValid(s.GetValue(subject, null))
						 select new RuleViolation(attrib.GetValidationKey(modelProp), attrib.FormatErrorMessage(s.GetDisplayName()), null)).ToList();

			errors.AddRange(query);

			var customAttributes = metaType.GetCustomAttributes(typeof(CustomValidationAttribute), true).Cast<CustomValidationAttribute>();

			foreach (var attrib in customAttributes)
			{
				if (!attrib.IsValid(subject))
				{
					errors.Add(new RuleViolation(modelType.Name, attrib.FormatErrorMessage(modelType.Name), null));
				}
			}

			return errors;
		}

		private static List<MemberInfo> GetPropertiesAndFields(Type modelType)
		{
			var symbols = new List<MemberInfo>();
			symbols.AddRange(modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
			symbols.AddRange(modelType.GetFields(BindingFlags.Public | BindingFlags.Instance));

			return symbols;
		}

		private static Type GetMetadataType(Type modelType)
		{
			Type metaType = null;

			MetadataTypeAttribute metaTypeAttribute
				= modelType.GetCustomAttributes(typeof(MetadataTypeAttribute), true).Cast<MetadataTypeAttribute>().SingleOrDefault();

			if (metaTypeAttribute != null)
			{
				metaType = metaTypeAttribute.MetadataClassType;
			}
			else
			{
				metaType = modelType;
			}
			return metaType;
		}
	}
}
