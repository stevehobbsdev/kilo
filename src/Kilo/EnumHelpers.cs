using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kilo
{
    public static class EnumHelpers
    {
        /// <summary>
        /// Returns the string description of an enum value, taking into account any DescriptionAttributes
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="value">The value</param>
        /// <returns>The value of the DescriptionAttribute on the enum item, or the enum label if none found.</returns>
        public static string GetDescription<T>(T value)
        {
            return GetDescription(value.GetType(), value);
        }

        /// <summary>
        /// Returns the string description of an enum value, taking into account any DescriptionAttributes
        /// </summary>
        /// <param name="enumType">The type of the enum</param>
        /// <param name="value">The value</param>
        /// <returns>The value of the DescriptionAttribute on the enum item, or the enum label if none found.</returns>
        public static string GetDescription(Type enumType, object value)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (value == null)
                throw new ArgumentNullException("value");

            FieldInfo fInfo = enumType.GetField(value.ToString());
            var attr = fInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (attr.Length > 0)
            {
                return (attr[0] as DescriptionAttribute).Description;
            }
            else
            {
                return string.Format("{0:g}", value);
            }
        }

        /// <summary>
        /// Retrieves a list of pairs representing each item in the enum, and the string value, taking into account any DescriptionAttributes present.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>A list of KeyValuePair<T, string>, where the Key is the enum value, and the value is the string representation</returns>
        public static IEnumerable<KeyValuePair<T, string>> GetNameValues<T>()
        {
            Type enumType = typeof(T);
            var names = System.Enum.GetNames(enumType);
            var values = System.Enum.GetValues(enumType);

            var pairs = new List<KeyValuePair<T, string>>();

            for (int i = 0; i < names.Length - 1; i++)
            {
                T value = (T)values.GetValue(i);
                string description = GetDescription(value);

                pairs.Add(new KeyValuePair<T, string>(value, description));
            }

            return pairs;
        }
    }
}
